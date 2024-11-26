using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.LVL_GENERATOR {
    internal class NewerLvlShapeObject {

        (int, int) base_size;
        Vector2[] complete_shape;
        Polygon2D[] sub_shapes;

        static (int, int) expand_size = (100, 100);

        List<int> unusable_indexes;


        NewerLvlShapeObject(int width, int height, int rect_num) {
            base_size = (width, height);
            complete_shape = new Vector2[rect_num];
            sub_shapes = new Polygon2D[rect_num];

            unusable_indexes = new List<int>();

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
            

            var pni = GetPrevAndNextIndex(attach_index);

            (int, int) expansion_dir = (
                complete_shape[pni.Item1].X == complete_shape[attach_index].X ? :
                
            );

            

            (int, int) GetPrevAndNextIndex(int index) {
                if (attach_index == 0)
                    return (complete_shape.Length - 1, 1);
                if (attach_index == complete_shape.Length - 1)
                    return (complete_shape.Length - 2, 0);

                return (index - 1, index + 1);
            }

        }

        void FillShape() { // _ to indicate that is a separate rect_num
            Random rand = new Random();

            CreateRect(-1); 
            for (int i = 0; i < complete_shape.Length-1; i++) {
                CreateRect(GetRandomIndex());
            }

            int GetRandomIndex() {

            }
        }
    }
}
