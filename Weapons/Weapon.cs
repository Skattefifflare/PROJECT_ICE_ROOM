
using Godot;



namespace Project_Ice_Room.Scriptbin {
    [Tool]
    public partial class Weapon : Sprite2D {

        [Export]
        public int DMG = 0;

        protected Area2D DMG_BOX;

        public bool is_attacking = false;
        public bool finished_attack = false;
        private float t;


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

        public void Attack(float delta) {
            GD.Print("attacking"); 

            if (t <= 0.5f) Rotation += delta * 10;

            t += delta;

            if (t >= 1) {
                finished_attack = true;
                t = 0;
                is_attacking = false;
                Rotation = 0;
            }

            /*
            finished_attack = false;
            Rotation += (rotation - Rotation)  * delta * (first_half ? 1 : -1); // should clamp change in rotation
            if (Rotation >= rotation) {
                first_half = true;
            }
            if (Rotation <= start_angle) {
                finished_attack = true;
                first_half = false;
                Rotation = start_angle;
                is_attacking = false;
            }
            */
        }
        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);
            if (is_attacking) Attack((float)delta);
        }
    }
}
