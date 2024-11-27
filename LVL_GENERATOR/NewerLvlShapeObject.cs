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
            if (attach_index == -1) { // base rectangle
                complete_shape[0] = new Vector2(base_size.Item1 / 2, base_size.Item2 / 2);
                complete_shape[0] = new Vector2(-base_size.Item1 / 2, base_size.Item2 / 2);
                complete_shape[0] = new Vector2(-base_size.Item1 / 2, -base_size.Item2 / 2);
                complete_shape[0] = new Vector2(base_size.Item1 / 2, -base_size.Item2 / 2);
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
                complete_shape[attach_index + 3].X -= exp.Item3 -=exp.Item1;

                complete_shape[attach_index + 4] = complete_shape[attach_index + 3];
                complete_shape[attach_index + 4].Y += exp.Item2 -= exp.Item3;
            }
            

            
            // I just put this in methods for cleanliness
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
                return (50, 50, 100, 100);
            }
            void ShiftArray() {
                for (int i = complete_shape.Length-1; i > attach_index + 4; i--) {
                    complete_shape[i] = complete_shape[i-5];
                }
            }
        }

        void FillShape() {
            CreateRect(-1); 
            for (int i = 0; i < complete_shape.Length-1; i++) {
                CreateRect(GetRandomIndex());
            }

            int GetRandomIndex() {
                List<int> usable_indexes = Enumerable.Range(0, complete_shape.Length).ToList();
                usable_indexes.RemoveAll(i => unusable_indexes.Contains(i));
                return usable_indexes[rand.Next(0, usable_indexes.Count)];
            }
        }

        internal void PrintShape() {
            foreach (var p in complete_shape) {
                GD.Print(p);
            }
        }
    }
}
