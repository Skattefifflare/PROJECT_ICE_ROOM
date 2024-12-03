using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


internal class NewLvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    private int rect_num;
    private Vector2[] full_shape;
    private List<Polygon2D> sub_shapes;
    private Random rand;

    float width;
    float height;


    internal NewLvlShapeObject(int width, int height, int rect_num) {
        this.width = width;
        this.height = height;

        this.rect_num = rect_num;
        full_shape = new Vector2[rect_num * 4];
        sub_shapes = new List<Polygon2D>();
        rand = new Random();


        CreateBaseShape();

        CreateExpansions();
    }

    void CreateBaseShape() {
        full_shape[0] = new Vector2(width / 2, height / 2);
        full_shape[1] = new Vector2(-width / 2, height / 2);
        full_shape[2] = new Vector2(-width / 2, -height / 2);
        full_shape[3] = new Vector2(width / 2, -height / 2);

        sub_shapes.Add(new Polygon2D {
            Polygon = new Vector2[] {
                full_shape[0], full_shape[1], full_shape[2], full_shape[3]
            },
            Color = new Color(1, 1, 1, 0.5f)
        });
    }

    void CreateExpansions() {
        int insert_index;
        List<int> unusable_indexes = new List<int>();

        int rects_created;


        Vector2[] new_points = new Vector2[5];
        Polygon2D new_sub_shape;

        for (rects_created = 1; rects_created < rect_num; rects_created++) {
            width *= 0.65f + (float)rand.NextDouble() / 3;
            height *= 0.65f + (float)rand.NextDouble() / 3;
            while (true) {
                insert_index = ChooseIndex();
                GD.Print("index is " + insert_index);
                CreatePoints();
                new_sub_shape = CreateSubShape();

                if (ShapeOverlaps()) {
                    unusable_indexes.Add(insert_index);
                    GD.Print("overlapping, trying again");
                }
                else {
                    ShiftArray();
                    UpdateUnusables();
                    Array.Copy(new_points, 0, full_shape, insert_index, new_points.Length);
                    sub_shapes.Add(new_sub_shape);
                    break;
                }
            }
        }

        int ChooseIndex() {
            List<int> usable_indexes = new List<int>();
            for (int i = 0; i < (rects_created * 4); i++) {
                if (!unusable_indexes.Contains(i)) {
                    usable_indexes.Add(i);
                }
            }
            return usable_indexes[rand.Next(0, usable_indexes.Count)];
        }

        void CreatePoints() {
            Vector2 dir = GetDirection();

            //Vector2 expansion = new Vector2(100, 100) * dir;
            //Vector2 inset = new Vector2(30, 30) * dir;

            Vector2 expansion = new Vector2(width, height) * dir;
            Vector2 inset = expansion * 0.2f;


            Vector2 odd = (Math.Sign(dir.X) == Math.Sign(dir.Y)) ? new Vector2(0, 1) : new Vector2(1, 0);
            Vector2 even = new Vector2(odd.Y, odd.X); 

            Array.Fill(new_points, full_shape[insert_index]);
            var operations = new Func<Vector2, Vector2>[] {
                x => x - (inset * odd),
                x => x + (expansion * even),
                x => x + ((expansion + inset) * odd),
                x => x - ((expansion + inset) * even),
                x => x - (expansion * odd)
            };
            for (int i = 4; i >= 0 ; i--) {
                for (int j = i; j <= 4; j++) {
                    new_points[j] = operations[i](new_points[j]);
                }
            }
            

            GD.Print("new_points:");
            foreach(var point in new_points) {
                GD.Print(point);
            }
            GD.Print("___");

            Vector2 GetDirection() {
                var prev_next = GetPrevAndNextIndex();
                Vector2 prev_to_i = full_shape[insert_index] - full_shape[prev_next.Item1];
                Vector2 next_to_i = full_shape[insert_index] - full_shape[prev_next.Item2];
                Vector2 dir = prev_to_i + next_to_i;
                dir = dir.Normalized();
                dir = new Vector2(Math.Sign(dir.X), Math.Sign(dir.Y));
                return dir;

                (int, int) GetPrevAndNextIndex() {
                    int current_max_index = (rects_created * 4) - 1; 
                    if (insert_index == 0) return (current_max_index, insert_index + 1);                   
                    if (insert_index == current_max_index) return (insert_index - 1, 0);                   
                    else return (insert_index - 1, insert_index + 1);                
                }
            }
        }

        Polygon2D CreateSubShape() {

            var sub_shape_points = new Vector2[6];
            Array.Copy(new_points, sub_shape_points, new_points.Length);
            sub_shape_points[5] = full_shape[insert_index];

            return new Polygon2D() {
                Polygon = sub_shape_points,
                Color = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 222f)
            };
        }

        bool ShapeOverlaps() { // returns true if it overlaps
            foreach (var polygon in sub_shapes) {
                if (Geometry2D.IntersectPolygons(polygon.Polygon, new_sub_shape.Polygon).Any(a => a.Any(p => !new_sub_shape.Polygon.Contains(p)))) {
                    GD.Print("intersecting points: ");
                    foreach( var a in Geometry2D.IntersectPolygons(polygon.Polygon, new_sub_shape.Polygon).ToArray()) {
                        foreach (var p in a) {
                            GD.Print(p);
                        }
                    }
                    return true;
                }
            }
            return false;         
        }

        void ShiftArray() { // shifts the 4 elements after index
            for (int i = full_shape.Length - 1; i > insert_index + 4; i--) {
                full_shape[i] = full_shape[i - 4];
            }
        }

        void UpdateUnusables() {
            unusable_indexes.Add(insert_index);
            unusable_indexes.Add(insert_index + 4);
            for (int i = 0; i < unusable_indexes.Count; i++) {
                if (unusable_indexes[i] > insert_index && unusable_indexes[i] != insert_index + 4) {
                    unusable_indexes[i] += 4;
                }
            }
        }
    }


    internal Vector2[] GetShape() {
        return full_shape;
    }
    internal List<Polygon2D> GetSubShapes() {
        return sub_shapes;
    }
}

