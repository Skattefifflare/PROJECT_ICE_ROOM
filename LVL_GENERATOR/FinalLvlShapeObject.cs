using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Godot.OpenXRInterface;

namespace Project_Ice_Room.LVL_GENERATOR {
    internal class FinalLvlShapeObject {

        private int rect_num;
        private (int, int) base_size;
        private Vector2[] shape;
        private List<Polygon2D> sub_shapes;
        private Random rand;


        internal FinalLvlShapeObject(int width, int height, int A_rect_num) {
            rect_num = A_rect_num;
            base_size = (width, height);
            shape = new Vector2[rect_num * 4];
            sub_shapes = new List<Polygon2D>();
            rand = new Random();

            CreateShape();
        }

        private void CreateShape() {
            List<int> unusable_indexes = new List<int>();

            CreateBaseShape();

            for (int i = 1; i < rect_num; i++) {
                GD.Print($"creating expansion number {i}");
                int index = ChooseInsertion(i);
                MakeSpace(index);
                ExpandShape(index);
                GD.Print("\n\n\n");
            }

            void CreateBaseShape() {
                GD.Print("...Creating Base Shape...");
                shape[0] = new Vector2(base_size.Item1 / 2, base_size.Item2 / 2);
                shape[1] = new Vector2(-base_size.Item1 / 2, base_size.Item2 / 2);
                shape[2] = new Vector2(-base_size.Item1 / 2, -base_size.Item2 / 2);
                shape[3] = new Vector2(base_size.Item1 / 2, -base_size.Item2 / 2);
            }

            int ChooseInsertion(int rects_created) {
                GD.Print("_____ChooseInsertion_____");
                List<int> usable_indexes = new List<int>();  //Enumerable.Range(0, rects_created * 4).ToList();
                
                for (int i = 0; i < (rects_created * 4); i++) {
                    if (!unusable_indexes.Contains(i)) {
                        usable_indexes.Add(i);
                    }
                }
                
                int index = usable_indexes[rand.Next(0, usable_indexes.Count)];
                PrintIntList(usable_indexes, "usable indexes: ");
                GD.Print("index is " + index);
                UpdateUnusables(index);                               

                void UpdateUnusables(int index) {
                    unusable_indexes.Add(index);
                    unusable_indexes.Add(index + 4);
                    for (int i = 0; i < unusable_indexes.Count; i++) {
                        if (unusable_indexes[i] > index) {
                            unusable_indexes[i] += 4;
                        }
                    }
                    PrintIntList(unusable_indexes, "unusable_indexes: ");
                }
                return index;
            }

            void MakeSpace(int index) {
                GD.Print("_____MakeSpace_____");

                PrintVectorArray(shape, "shape pre-shifting:");
                for (int i = shape.Length - 5; i > index; i--) {
                    shape[i + 4] = shape[i];
                }
                PrintVectorArray(shape, "shape post-shifting:");
            }

            void ExpandShape(int index) {
                GD.Print("_____ExpandShape_____");
                Vector2 dir = GetDirection();
                Vector2 inset = new Vector2(30, 30) * dir;
                Vector2 expansion = new Vector2(100, 100) * dir;

                Vector2[] new_points = new Vector2[5];



                if (Math.Sign(dir.X) == Math.Sign(dir.Y)) {

                    new_points[index] = shape[index];
                    new_points[index].Y -= inset.Y;

                    new_points[index + 1] = new_points[index];
                    new_points[index + 1].X += expansion.X;

                    new_points[index + 2] = new_points[index + 1];
                    new_points[index + 2].Y += expansion.Y;
                    new_points[index + 2].Y += inset.Y;

                    new_points[index + 3] = new_points[index + 2];
                    new_points[index + 3].X -= expansion.X;
                    new_points[index + 3].X -= inset.X;

                    new_points[index + 4] = new_points[index + 3];
                    new_points[index + 4].Y -= expansion.Y;

                    CreateSubShape(inset.Y, false);
                }
                else {
                    new_points[index] = shape[index];
                    new_points[index].X -= inset.X;

                    new_points[index + 1] = new_points[index];
                    new_points[index + 1].Y += expansion.Y;

                    new_points[index + 2] = new_points[index + 1];
                    new_points[index + 2].X += expansion.X;
                    new_points[index + 2].X += inset.X;

                    new_points[index + 3] = new_points[index + 2];
                    new_points[index + 3].Y -= expansion.Y;
                    new_points[index + 3].Y -= inset.Y;

                    new_points[index + 4] = new_points[index + 3];
                    new_points[index + 4].X -= expansion.X;

                    CreateSubShape(inset.X, true);
                }
                shape[index] = new_points[index];
                shape[index + 1] = new_points[index + 1];
                shape[index + 2] = new_points[index + 2];
                shape[index + 3] = new_points[index + 3];
                shape[index + 4] = new_points[index + 4];


                GD.Print("dir was " + dir.X + ", " + dir.Y);
                GD.Print("expansion was " + expansion.X + ", " + expansion.Y);
                Vector2 GetDirection() {
                    var prev_next = GetPrevAndNextIndex();
                    Vector2 prev_to_i = shape[index] - shape[prev_next.Item1];
                    Vector2 next_to_i = shape[index] - shape[prev_next.Item2];
                    Vector2 dir = prev_to_i + next_to_i;
                    dir = dir.Normalized();
                    dir = new Vector2(Math.Sign(dir.X), Math.Sign(dir.Y));
                    return dir;

                    (int, int) GetPrevAndNextIndex() {
                        int current_max_index = 3 + (unusable_indexes.Count / 2 * 4); // 3 for the base rect, for each new rect 2 points are added to unusables

                        if (index == 0) {
                            return (current_max_index, index + 1);
                        }
                        if (index == current_max_index) {
                            return (index - 1, 0);
                        }
                        else {
                            return (index - 1, index + 1);
                        }
                    }
                }

                void CreateSubShape(float inset, bool first_inset_is_X) {

                    Vector2 vec4 = new_points[index + 4];

                    if (first_inset_is_X) {
                        vec4.X -= inset;
                    }
                    else {
                        vec4.Y -= inset;
                    }

                    Polygon2D new_sub_shape = new Polygon2D {
                        Polygon = new Vector2[] {
                            new_points[index + 1],
                            new_points[index + 2],
                            new_points[index + 3],
                            vec4
                        },
                        Color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 0.7f)
                    };
                    //CheckOverlap();

                    sub_shapes.Add(new_sub_shape);

                    void CheckOverlap() {
                        for (int i = 0; i < sub_shapes.Count; i++) {

                            if ((index - (index % 4)) / 4 == i) {
                                continue;
                            }

                            foreach (var p in new_sub_shape.Polygon) {

                                Vector2[] shape = sub_shapes[i].Polygon;
                                var top_left = new Vector2(
                                    Math.Min(Math.Min(shape[0].X, shape[1].X), Math.Min(shape[2].X, shape[3].X)),
                                    Math.Min(Math.Min(shape[0].Y, shape[1].Y), Math.Min(shape[2].Y, shape[3].Y)));

                                var bottom_right = new Vector2(
                                    Math.Max(Math.Max(shape[0].X, shape[1].X), Math.Max(shape[2].X, shape[3].X)),
                                    Math.Max(Math.Max(shape[0].Y, shape[1].Y), Math.Max(shape[2].Y, shape[3].Y)));

                                var clampedX = Math.Clamp(p.X, top_left.X, bottom_right.X);
                                var clampedY = Math.Clamp(p.Y, top_left.Y, bottom_right.Y);

                                if (clampedX == top_left.X || clampedX == bottom_right.X) {
                                    if (clampedY == top_left.Y || clampedY == bottom_right.Y) {
                                        unusable_indexes.Add(index);
                                        return;
                                    }
                                }
                                
                            }
                        }
                        sub_shapes.Add(new_sub_shape);
                    }
                }
            }

        }

        internal Vector2[] GetShape() {
            return shape;
        }
        internal List<Polygon2D> GetSubShapes() {
            return sub_shapes;
        }

        void PrintIntList(List<int> list, string text) {
            string list_text = "";
            foreach (var i in list) {
                list_text += (i + ":");
            }
            GD.Print(text);
            GD.Print(list_text);
        }
        void PrintVectorArray(Vector2[] array, string text) {
            string array_text = "";
            foreach (var v in array) {
                array_text += (" (" + v.X + "," + v.Y + ")");
            }
            GD.Print(text);
            GD.Print(array_text);
        }
    }
}
