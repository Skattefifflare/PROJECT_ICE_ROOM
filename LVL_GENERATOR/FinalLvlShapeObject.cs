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
        private Polygon2D[] sub_shapes;
        private Random rand;


        internal FinalLvlShapeObject(int width, int height, int A_rect_num) {
            rect_num = A_rect_num;
            base_size = (width, height);
            shape = new Vector2[rect_num * 4];
            sub_shapes = new Polygon2D[rect_num];
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
                CreateSubShape();
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

                if (Math.Sign(dir.X) == Math.Sign(dir.Y)) {
                    
                    shape[index].Y -= inset.Y;

                    shape[index + 1] = shape[index];
                    shape[index + 1].X += expansion.X;

                    shape[index + 2] = shape[index + 1];
                    shape[index + 2].Y += expansion.Y;
                    shape[index + 2].Y += inset.Y;

                    shape[index + 3] = shape[index + 2];
                    shape[index + 3].X -= expansion.X;
                    shape[index + 3].X -= inset.X;

                    shape[index + 4] = shape[index + 3];
                    shape[index + 4].Y -= expansion.Y;                                  
                }
                else {
                    
                    shape[index].X -= inset.X;

                    shape[index + 1] = shape[index];
                    shape[index + 1].Y += expansion.Y;

                    shape[index + 2] = shape[index + 1];
                    shape[index + 2].X += expansion.X;
                    shape[index + 2].X += inset.X;

                    shape[index + 3] = shape[index + 2];
                    shape[index + 3].Y -= expansion.Y;
                    shape[index + 3].Y -= inset.Y;

                    shape[index + 4] = shape[index + 3];
                    shape[index + 4].X -= expansion.X;                                  
                }
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
            }

            void CreateSubShape() {
                return;
            }
        }

        internal Vector2[] GetShape() {
            return shape;
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
