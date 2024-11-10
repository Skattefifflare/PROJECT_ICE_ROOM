using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


internal class LvlShapeObject { // contains all the bounding points of the level and some other stuff. 
    Vector2[] POINTS;
    List<int> USABLE_INDEXES;


    LvlShapeObject(int rect_num, Vector2 init_size) {

        
        POINTS = new Vector2[rect_num*4];
        Vector2 half = init_size / 2;
        POINTS[0] = new Vector2( half.X,  half.Y);
        POINTS[1] = new Vector2(-half.X,  half.Y);
        POINTS[2] = new Vector2(-half.X, -half.Y);
        POINTS[3] = new Vector2( half.X, -half.Y);
        USABLE_INDEXES = (List<int>)Enumerable.Range(0, rect_num * 4);


        Random r = new Random();
        int max_index = 3;
        for (int i = 0; i < rect_num; i++) {
            int[] unlocked_indexes = USABLE_INDEXES.GetRange(0, max_index).ToArray();          
            int random_i = unlocked_indexes[r.Next(unlocked_indexes.Length)];

            //create rect extension

            Vector2[] POINTS_COPY = POINTS;
            foreach (Vector2 p in POINTS) {
                if (p == null) continue;
                POINTS_COPY[Array.IndexOf(POINTS, p) + 4] = p;
            }
            POINTS = POINTS_COPY;

            int posnegX = POINTS[random_i].X >= POINTS
            int posnegY;


            POINTS[random_i] = new Vector2(0, 0);
            POINTS[random_i + 1] = new Vector2(0, 0);
            POINTS[random_i + 2] = new Vector2(0, 0);
            POINTS[random_i + 3] = new Vector2(0, 0);
            POINTS[random_i + 4] = new Vector2(0, 0);
            
            max_index += 4;
        }

    }
    
}