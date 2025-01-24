using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scripts {
    internal class Flag {
        public bool is_hoisted = false;
        public bool just_hoisted = false;


        private void Hoist() {
            if (is_hoisted) return;
            is_hoisted = true;
            just_hoisted = true;
        }
        private void Lower() {
            is_hoisted = false;
            just_hoisted = false;
        }
        public void Set(bool hoist_or_lower) {
            Update();
            if (hoist_or_lower) Hoist();
            else Lower();
        }
        public void Update() {
            if (just_hoisted) just_hoisted = false;           
        }
        public Flag() {
            is_hoisted = false;
            just_hoisted = false;
        }
    }
}
