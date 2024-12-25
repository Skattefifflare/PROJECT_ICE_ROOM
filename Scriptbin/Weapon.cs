using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.NativeInterop;


namespace Project_Ice_Room.Scriptbin {
    [Tool]
    public partial class Weapon : Sprite2D {

        [Export]
        public int DMG = 0;

        protected Area2D DMG_BOX;


        public override void _Ready() {
            base._Ready();

            DMG_BOX = (Area2D)FindChild("dmg_box");
            DMG_BOX.Monitorable = false;
        }

        public void MakeDangerous() {
            DMG_BOX.Monitorable = true;
        }
        public void MakeHarmLess() {
            DMG_BOX.Monitorable = false;
        }
    }
}
