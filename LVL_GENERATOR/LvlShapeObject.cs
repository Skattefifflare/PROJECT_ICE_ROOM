using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


internal class LvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    Vector2[] points;
    List<int> usable_indexes;
    List<int> unusable_indexes;

    internal LvlShapeObject(int rect_num, Vector2 init_size) {
        points = new Vector2[rect_num * 4];

        // starting rectangle
        points[0] = new Vector2(init_size.X/2, init_size.Y / 2);
        points[1] = new Vector2(-init_size.X / 2, init_size.Y / 2);
        points[2] = new Vector2(-init_size.X / 2, -init_size.Y / 2);
        points[3] = new Vector2(init_size.X / 2, -init_size.Y / 2);
        usable_indexes = Enumerable.Range(0, rect_num * 4).ToList();


        Random r = new Random();
        int max_index = 3;
        while(rect_num > 1) { //wont run if we just want the main rectangle
            
            int[] filled_indexes = usable_indexes.GetRange(0, max_index + 1).ToArray();

            // random index of a corner of the shape.
            int r_index = filled_indexes[r.Next(filled_indexes.Length)];
            int next_index = (r_index == max_index) ? 0 : r_index + 1;
            int prev_index = (r_index == 0) ? max_index : r_index - 1;
            
            // if our random point is on the bottom right corner of a rectangle the width and height will both be +.
            // if the point is top left both will be -.
            // this can be exploited to simplify the expansion.
            Vector2 parent_size = new Vector2(
                NonZeroMax(points[r_index].X - points[prev_index].X, points[r_index].X - points[next_index].X),
                NonZeroMax(points[r_index].Y - points[prev_index].Y, points[r_index].Y - points[next_index].Y)
            );
            Vector2 inset = new Vector2(
                parent_size.X * (((float)r.NextDouble() / 3) + 0.10f),
                parent_size.Y * (((float)r.NextDouble() / 3) + 0.10f)
            );
            Vector2 expansion = new Vector2(
                parent_size.X * (((float)r.NextDouble() / 4) + 0.35f),
                parent_size.Y * (((float)r.NextDouble() / 4) + 0.35f)
            );

            // does stuff
            (Vector2, Vector2) VectorMagic(Vector2 vctr) {
                return (parent_size.X > 0 && parent_size.Y > 0) || (parent_size.X < 0 && parent_size.Y < 0) ?
                (new Vector2(0, vctr.Y), new Vector2(vctr.X, 0)) :
                (new Vector2(vctr.X, 0), new Vector2(0, vctr.Y));
            }

            /*
            Vector2 corner_one = points[r_index] -= VectorMagic(inset).Item1;
            Vector2 corner_two = corner_one += VectorMagic(expansion).Item2;
            Vector2 corner_three = points[r_index] + expansion;
            Vector2 corner_five = points[r_index] -= VectorMagic(inset).Item2;
            Vector2 corner_four = corner_five += VectorMagic(expansion).Item1;
            */

            Vector2[] corners = new Vector2[] {
                points[r_index] - VectorMagic(inset).Item1,               // corner_one
                points[r_index] - VectorMagic(inset).Item1 + VectorMagic(expansion).Item2, // corner_two
                points[r_index] + expansion,                              // corner_three
                points[r_index] - VectorMagic(inset).Item2 + VectorMagic(expansion).Item1,  // corner_four
                points[r_index] - VectorMagic(inset).Item2                // corner_five
            };

            // making space
            Array.Copy(points, r_index + 1, points, r_index + 5, (points.Length - 1 - r_index - 4));
            // filling space
            Array.Copy(corners, 0, points, r_index, corners.Length);

            max_index += 2;
            usable_indexes.Remove(r_index);
            usable_indexes.Remove(r_index + 4); // THE CULPRIT!!!!
            rect_num--;
        }
    }
    float NonZeroMax(float a, float b) {
        if (a != 0 && b == 0) return a;
        if (b != 0 && a == 0) return b;
        if (a != 0 && b != 0) return Math.Max(a, b);
        return 0; // Only if both are zero
    }
    internal Vector2[] GetShape() {
        return points;
    }
    internal void PrintArray() {
        int index = 0;
        points.ToList().ForEach(i => {
            GD.Print(index + ": " + i.ToString());
            index++;
        });
    }
}

