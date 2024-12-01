using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


internal class LvlShapeObject {
    private int rect_num;
    private int width;
    private int height;

    private Vector2[] shape;
    private List<Polygon2D> sub_shapes; 
    private Random rand;


    internal LvlShapeObject(int width, int height, int rect_num) {
        this.rect_num = rect_num;
        this.width = width;
        this.height = height;

        shape = new Vector2[rect_num * 4];
        sub_shapes = new List<Polygon2D>();
        rand = new Random();

        CreateShape();
    }


    void CreateShape() {

        shape[0] = new Vector2(width / 2, height / 2);
        shape[1] = new Vector2(-width / 2, height / 2);
        shape[2] = new Vector2(-width / 2, -height / 2);
        shape[3] = new Vector2(width / 2, -height / 2);
        sub_shapes.Add(new Polygon2D {
            Polygon = new Vector2[] {
                shape[0], shape[1], shape[2], shape[3]
            },
            Color = new Color(1, 1, 1, 0.5f)
        });

        int index;
        List<int> unusable_indexes = new List<int>();

        Vector2[] new_points = new Vector2[5];
        Polygon2D new_sub_shape;

        for (int i = 1; i < rect_num; i++) {
            
            while (true) {
                index = ChooseIndex(i);
                new_points = CreateExpansion();

                if (!CheckOverlap(new_sub_shape, index)) {
                    GD.Print("no overlap at index: " + index);
                    ShiftArray(index);
                    sub_shapes.Add(new_sub_shape);
                    foreach (var p in new_sub_shape.Polygon) {
                        GD.Print(p);
                    }


                    UpdateUnusables();
                    Array.Copy(new_points, 0, shape, index, new_points.Length);
                    break;
                }
                else {
                    GD.Print("overlap at index: " + index);
                    unusable_indexes.Add(index);
                }
            }
        }
        GD.Print("___");
        GD.Print("subshapecount: " + sub_shapes.Count);

        foreach (var sub_shape in sub_shapes) {
            GD.Print("subshape number " + sub_shapes.IndexOf(sub_shape));
            foreach (var p in sub_shape.Polygon) {
                GD.Print(p);
            }
            GD.Print("___");
        }
        GD.Print("\n\n");



        int ChooseIndex(int rects_created) {
            List<int> usable_indexes = new List<int>();
            for (int i = 0; i < (rects_created * 4); i++) {
                if (!unusable_indexes.Contains(i)) {
                    usable_indexes.Add(i);
                }
            }
            index = usable_indexes[rand.Next(0, usable_indexes.Count)];
            return index;
        }
        Vector2[] CreateExpansion() {

            var dir = GetDirection();

            Vector2 expansion = new Vector2(100, 100) * dir;
            Vector2 inset = new Vector2(30, 30) * dir;


            
            Vector2[] new_points = new Vector2[5];

            if (Math.Sign(dir.X) == Math.Sign(dir.Y)) {

                new_points[0] = shape[index];
                new_points[0].Y -= inset.Y;

                new_points[1] = new_points[0];
                new_points[1].X += expansion.X;

                new_points[2] = new_points[1];
                new_points[2].Y += expansion.Y;
                new_points[2].Y += inset.Y;

                new_points[3] = new_points[2];
                new_points[3].X -= expansion.X;
                new_points[3].X -= inset.X;

                new_points[4] = new_points[3];
                new_points[4].Y -= expansion.Y;

                new_sub_shape =  CreateSubShape(new Vector2(0, inset.Y));
            }
            else {
                new_points[0] = shape[index];
                new_points[0].X -= inset.X;

                new_points[1] = new_points[0];
                new_points[1].Y += expansion.Y;

                new_points[2] = new_points[1];
                new_points[2].X += expansion.X;
                new_points[2].X += inset.X;

                new_points[3] = new_points[2];
                new_points[3].Y -= expansion.Y;
                new_points[3].Y -= inset.Y;

                new_points[4] = new_points[3];
                new_points[4].X -= expansion.X;

                new_sub_shape = CreateSubShape(new Vector2(inset.X, 0));
            }
            return new_points;

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
        Polygon2D CreateSubShape(Vector2 first_inset) {
            return new Polygon2D() {
                Polygon = new Vector2[] {
                    new_points[1], new_points[2], new_points[3], new_points[4],
                },
                Color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 0.5f)
            };
        }
        void UpdateUnusables() {
            unusable_indexes.Add(index);
            unusable_indexes.Add(index + 4);
            for (int i = 0; i < unusable_indexes.Count; i++) {
                if (unusable_indexes[i] > index && unusable_indexes[i] != index + 4) {
                    unusable_indexes[i] += 4;
                }
            }
        }
    }
    void ShiftArray(int index) {
        for (int i = shape.Length - 1; i > index + 4; i--) {
            shape[i] = shape[i - 4];
        }
    }

    bool CheckOverlap(Polygon2D new_sub_shape, int attach_index) { // false means no overlap
        if (sub_shapes.Count < 2 ) return false;
        foreach ( var sub_shape in sub_shapes) {
            var shape_points = sub_shape.Polygon;

            bool parent_flag = false;
            foreach (var p in shape_points) {
                if (p == shape[attach_index]) {
                    parent_flag = true; break;
                }
            }
            if (parent_flag) continue;

            var top_left = new Vector2(
                Math.Min(Math.Min(shape_points[0].X, shape_points[1].X), Math.Min(shape_points[2].X, shape_points[3].X)),
                Math.Min(Math.Min(shape_points[0].Y, shape_points[1].Y), Math.Min(shape_points[2].Y, shape_points[3].Y)));

            var bottom_right = new Vector2(
                Math.Max(Math.Max(shape_points[0].X, shape_points[1].X), Math.Max(shape_points[2].X, shape_points[3].X)),
                Math.Max(Math.Max(shape_points[0].Y, shape_points[1].Y), Math.Max(shape_points[2].Y, shape_points[3].Y)));

            foreach (var p in new_sub_shape.Polygon) {
                var clampedX = Math.Clamp(p.X, top_left.X, bottom_right.X);
                var clampedY = Math.Clamp(p.Y, top_left.Y, bottom_right.Y);

                if (p.X == clampedX && p.Y == clampedY) {
                    return true;
                }
            }
        }
        return false;
    }

    internal Vector2[] GetShape() {
        return shape;
    }
    internal List<Polygon2D> GetSubShapes() {
        return sub_shapes;
    }
}

