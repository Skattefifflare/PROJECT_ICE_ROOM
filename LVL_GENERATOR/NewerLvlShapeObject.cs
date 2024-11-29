using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.LVL_GENERATOR {
    internal class NewerLvlShapeObject {

        (int, int) base_size;
        internal Vector2[] complete_shape;
         Polygon2D[] sub_shapes;
        List<int> unusable_indexes;
        Random rand = new Random();

    

        internal NewerLvlShapeObject(int width, int height, int rect_num) {
            base_size = (width, height);
            complete_shape = new Vector2[rect_num*4];
            sub_shapes = new Polygon2D[rect_num];
            unusable_indexes = new List<int>();
            rand = new Random();

            FillShape();
        }



        void CreateRect(int attach_index) {
            GD.Print("attach index:" + attach_index);
            if (attach_index == -1) { // base rectangle
                complete_shape[0] = new Vector2(base_size.Item1 / 2, base_size.Item2 / 2);
                complete_shape[1] = new Vector2(-base_size.Item1 / 2, base_size.Item2 / 2);
                complete_shape[2] = new Vector2(-base_size.Item1 / 2, -base_size.Item2 / 2);
                complete_shape[3] = new Vector2(base_size.Item1 / 2, -base_size.Item2 / 2);
                return;
            }

            ShiftArray();
            var dir = GetDirection();
            var exp = GetExpansion();

            
            if (Math.Sign(dir.X) == Math.Sign(dir.Y)) { // bottom right or top left
                complete_shape[attach_index].Y -= exp.Item2;

                complete_shape[attach_index + 1] = complete_shape[attach_index];
                complete_shape[attach_index + 1].X += exp.Item3;

                complete_shape[attach_index + 2] = complete_shape[attach_index + 1];
                complete_shape[attach_index + 2].Y += exp.Item4;

                complete_shape[attach_index + 3] = complete_shape[attach_index + 2];
                complete_shape[attach_index + 3].X -= exp.Item3 -= exp.Item1;

                complete_shape[attach_index + 4] = complete_shape[attach_index + 3];
                complete_shape[attach_index + 4].Y += exp.Item2 -= exp.Item4;
            }
            else {
                complete_shape[attach_index].X -= exp.Item1;

                complete_shape[attach_index + 1] = complete_shape[attach_index];
                complete_shape[attach_index + 1].Y += exp.Item4;

                complete_shape[attach_index + 2] = complete_shape[attach_index + 1];
                complete_shape[attach_index + 2].X += exp.Item3;

                complete_shape[attach_index + 3] = complete_shape[attach_index + 2];
                complete_shape[attach_index + 3].Y -= exp.Item4 -= exp.Item2;

                complete_shape[attach_index + 4] = complete_shape[attach_index + 3];
                complete_shape[attach_index + 4].X += exp.Item1 -= exp.Item3;
            }
            
            
            
            
            
            // I just put all this in methods for cleanliness
            (int, int) GetPrevAndNextIndex(int index) {
                if (attach_index == 0)
                    return (complete_shape.Length - 1, 1);
                if (attach_index == complete_shape.Length - 1)
                    return (complete_shape.Length - 2, 0);

                return (index - 1, index + 1);
            }
            Vector2 GetDirection() {
                var pni = GetPrevAndNextIndex(attach_index);

                Vector2 prev_to_i = complete_shape[attach_index] - complete_shape[pni.Item1];
                Vector2 next_to_i = complete_shape[attach_index] - complete_shape[pni.Item2];
                Vector2 dir = prev_to_i + next_to_i;
                // dir.Normalized();

                return dir;
            }
            (int, int, int, int) GetExpansion() { // first 2 are insets, last 2 are expansions
                var dir = GetDirection();
                dir = dir.Normalized();
                (int, int, int, int) expansion = (50 * (int)dir.X, 50 * (int)dir.Y, 100 * (int)dir.X, 100 * (int)dir.Y);

                return expansion;
            }
            void ShiftArray() {
                for (int i = complete_shape.Length-1; i > attach_index + 4; i--) {
                    complete_shape[i] = complete_shape[i-5];
                }
            }
        }

        void FillShape() {
            GD.Print("subshapelen: " + (sub_shapes.Length - 1));
            CreateRect(-1);
            for (int i = 0; i < sub_shapes.Length-1; i++) {
                CreateRect(GetRandomIndex( (i + 1) * 4));
            }

            int GetRandomIndex(int current_max_index) {
                List<int> usable_indexes = Enumerable.Range(0, current_max_index).ToList();
                usable_indexes.RemoveAll(i => unusable_indexes.Contains(i));
                GD.Print("usable indexes");
                foreach(var i in usable_indexes) {
                    GD.Print(i);
                }
                return usable_indexes[rand.Next(0, usable_indexes.Count)];
            }
        }

        internal void PrintShape() {
            GD.Print("_________all points_________");
            foreach (var p in complete_shape) {
                GD.Print(p);
            }
            GD.Print("____________________________");
        }
    }
}
