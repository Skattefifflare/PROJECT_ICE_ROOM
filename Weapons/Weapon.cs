
using Godot;



namespace Project_Ice_Room.Scriptbin {
    [Tool]
    public partial class Weapon : Sprite2D {

        [Export]
        public int dmg = 0;

        protected Area2D dmg_box;

        public bool attacking = false;

        public override void _Ready() {
            base._Ready();

            dmg_box = (Area2D)FindChild("dmg_box");
            dmg_box.Monitorable = false;
        }

        public void MakeDangerous() {
            dmg_box.Monitorable = true;
        }
        public void MakeHarmLess() {
            dmg_box.Monitorable = false;
        }
    }
}
