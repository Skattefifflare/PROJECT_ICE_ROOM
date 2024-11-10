using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


internal class NewLvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    Vector2[] POINTS;
    List<int> USABLE_INDEXES;


    internal NewLvlShapeObject(int rect_num, Vector2 init_size) {


        POINTS = new Vector2[rect_num * 4];
        Vector2 half = init_size / 2;
        POINTS[0] = new Vector2(half.X, half.Y);
        POINTS[1] = new Vector2(-half.X, half.Y);
        POINTS[2] = new Vector2(-half.X, -half.Y);
        POINTS[3] = new Vector2(half.X, -half.Y);
        USABLE_INDEXES = Enumerable.Range(0, rect_num * 4).ToList();


        Random r = new Random();
        int max_index = 3;
        for (int i = 1; i < rect_num; i++) {
            int[] filled_indexes = USABLE_INDEXES.GetRange(0, max_index).ToArray();

            int random_i = filled_indexes[r.Next(filled_indexes.Length)];
            int prev_i = (random_i == 0) ? max_index : random_i - 1;
            int next_i = (random_i == max_index) ? 0 : random_i + 1;
            GD.Print(random_i);
            GD.Print(next_i);
            GD.Print(prev_i);

            float parent_width = NonZeroMax(
               POINTS[random_i].X - POINTS[prev_i].X, // lb-, rt+
               POINTS[random_i].X - POINTS[next_i].X); // rb+, lt- 

            float parent_height = NonZeroMax(
                POINTS[random_i].Y - POINTS[prev_i].Y, // rb+, lt-
                POINTS[random_i].Y - POINTS[next_i].Y); // lb+, rt-

           

            //
            
            float inset_width = Math.Abs(parent_width) * (((float)r.NextDouble() / 3) + 0.10f);
            float inset_height = Math.Abs(parent_height) * (((float)r.NextDouble() / 3) + 0.10f);

            Vector2 expansion = new Vector2(
                ((parent_width > 0 ? 1 : -1) * (r.Next() % Math.Abs(parent_width) / 2)) + (Math.Abs(parent_width) * 0.3f), // span between 0.3 and 0.8
                ((parent_height > 0 ? 1 : -1) * (r.Next() % Math.Abs(parent_height) / 2)) + (Math.Abs(parent_height) * 0.3f)
            );


            Vector2 og = POINTS[random_i];
            Vector2 first = POINTS[random_i];
            Vector2 last = POINTS[random_i];
            if (random_i % 2 == 0) {
                int down_up = (POINTS[random_i].Y - POINTS[prev_i].Y > 0 ? 1 : -1);
                first.Y -= inset_height * down_up;
                last.X += inset_width * down_up * -1;
            }
            else {
                int right_left = (POINTS[random_i].X - POINTS[prev_i].X > 0 ? 1 : -1);
                first.X -= inset_width * right_left;
                last.Y += inset_height * right_left;
            }

            // shifts all points after insertion by 4.
            Vector2[] POINTS_COPY = POINTS;
            foreach (Vector2 p in POINTS) {
                if (Array.IndexOf(POINTS, p) + 4 >= POINTS.Length) continue;
                POINTS_COPY[Array.IndexOf(POINTS, p) + 4] = p;
            }
            POINTS = POINTS_COPY;


            POINTS[random_i] = first;
            POINTS[random_i + 1] = first;
            POINTS[random_i + 2] = first + expansion;
            POINTS[random_i + 3] = last;
            POINTS[random_i + 4] = last;

            if (first.Y != og.Y) {
                POINTS[random_i + 1].X += expansion.X;
                POINTS[random_i + 3].Y += expansion.Y;
            }
            else {
                POINTS[random_i + 1].Y += expansion.Y;
                POINTS[random_i + 3].X += expansion.X;
            }

            //
            max_index += 2;
            USABLE_INDEXES.Remove(random_i);
            USABLE_INDEXES.Remove(random_i + 4);
        }
    }
    float NonZeroMax(float a, float b) {
        if (a != 0 && b == 0) return a;
        if (b != 0 && a == 0) return b;
        if (a != 0 && b != 0) return Math.Max(a, b);
        return 0; // Only if both are zero
    }

    internal Vector2[] GetShape() {
        return POINTS;
    }
    internal void PrintArray() {
        POINTS.ToList().ForEach(i => GD.Print(i.ToString()));
    }
}

