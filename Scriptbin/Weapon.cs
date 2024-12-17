using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.NativeInterop;


namespace Project_Ice_Room.Scriptbin {
    public partial class Weapon : Sprite2D {
        public int dmg = 100;

        public override void _Ready() {
            base._Ready();
        }

    }
}
