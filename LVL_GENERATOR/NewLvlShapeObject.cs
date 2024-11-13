using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


internal class NewLvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    Vector2[] POINTS;
    List<int> USABLE_INDEXES;
    List<int> UNUSABLE_INDEXES;

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
            USABLE_INDEXES.ToList().ForEach(i => {
                GD.Print(i.ToString());
            });
            int[] filled_indexes = USABLE_INDEXES.GetRange(0, max_index + 1).ToArray();
            filled_indexes.ToList().ForEach(i => {
                GD.Print(i.ToString());
            });
            int random_i = filled_indexes[r.Next(filled_indexes.Length)];
            GD.Print(random_i.ToString());
            int prev_i = (random_i == 0) ? max_index : random_i - 1;
            int next_i = (random_i == max_index) ? 0 : random_i + 1;
            GD.Print("random: " + random_i);
            GD.Print("next" + next_i);
            GD.Print("prev: " + prev_i);

            float parent_width = NonZeroMax(
               POINTS[random_i].X - POINTS[prev_i].X, // lb-, rt+
               POINTS[random_i].X - POINTS[next_i].X); // rb+, lt- 

            float parent_height = NonZeroMax(
                POINTS[random_i].Y - POINTS[prev_i].Y, // rb+, lt-
                POINTS[random_i].Y - POINTS[next_i].Y); // lb+, rt-
            GD.Print("parent dimensions: " + parent_width + " : " + parent_height);
            // the parent dimensions +- will tell which quadrant we are in. ++ is right bottom. -- is top left
            // this will probably work even when all points are in the ++ quadrant
            Vector2 quadrant = new Vector2( // unnecessary, can just use parent dimensions
                parent_width > 0 ? 1 : -1,
                parent_height > 0 ? 1 : -1
            );


            float inset_width = parent_width * (((float)r.NextDouble() / 3) + 0.10f);
            float inset_height = parent_height * (((float)r.NextDouble() / 3) + 0.10f);
            float expansion_width = parent_width * (((float)r.NextDouble() / 4) + 0.35f);
            float expansion_height = parent_height * (((float)r.NextDouble() / 4) + 0.35f);

            GD.Print("inset dimensions: " + inset_width + " : " + inset_height);
            GD.Print("expansion dimensions: " + expansion_width + " : " + expansion_height);

            Vector2 exp_start = POINTS[random_i];
            Vector2 exp_end = POINTS[random_i];
            Vector2 exp_middle = POINTS[random_i];

            if ((quadrant.X == 1 && quadrant.Y == 1) || (quadrant.X == -1 && quadrant.Y == -1)) { // ++ and -- quadrant will move the start along Y-axis
                exp_start.Y -= inset_height;
                exp_end.X -= inset_width;
            }
            else {
                exp_start.X -= inset_width;
                exp_end.Y -= inset_height;
            }
            Vector2 exp_second = exp_start;
            Vector2 exp_fourth = exp_end;
            if (exp_start.Y != POINTS[random_i].Y) {
                exp_second.X += expansion_width;
                exp_fourth.Y += expansion_height;
            }
            else {
                exp_second.Y += expansion_height;
                exp_fourth.X += expansion_width;
            }
            exp_middle += new Vector2(expansion_width, expansion_height);

            // making space

            Array.Copy(POINTS, random_i + 1, POINTS, random_i + 5, (POINTS.Length - 1 - random_i - 4));



            POINTS[random_i] = exp_start;
            POINTS[random_i + 1] = exp_second;
            POINTS[random_i + 2] = exp_middle;
            POINTS[random_i + 3] = exp_fourth;
            POINTS[random_i + 4] = exp_end;

            
            max_index += 2;
            USABLE_INDEXES.Remove(random_i);
            USABLE_INDEXES.Remove(random_i + 4); // THE CULPRIT!!!!
            
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
        int index = 0;
        POINTS.ToList().ForEach(i => {
            GD.Print(index + ": " + i.ToString());
            index++;
        });
    }
}

