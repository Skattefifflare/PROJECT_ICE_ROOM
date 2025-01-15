
using Godot;



namespace Project_Ice_Room.Scriptbin {
    [Tool]
    public partial class Weapon : Sprite2D {

        [Export]
        public int DMG = 0;

        protected Area2D DMG_BOX;

        public bool is_attacking = false;
        public bool finished_attack = false;
        float start_angle = 40f;
        private float rotation = 5f;
        bool first_half = false;


        public override void _Ready() {
            base._Ready();

            DMG_BOX = (Area2D)FindChild("dmg_box");
            DMG_BOX.Monitorable = false;
            Rotation = start_angle;
        }

        public void MakeDangerous() {
            DMG_BOX.Monitorable = true;
        }
        public void MakeHarmLess() {
            DMG_BOX.Monitorable = false;
        }

        public void Attack(float delta) {
            finished_attack = false;
            Rotation += rotation * delta * (first_half ? 1 : -1); // should clamp change in rotation
            if (Rotation >= rotation) {
                first_half = true;
            }
            if (Rotation <= start_angle) {
                finished_attack = true;
                first_half = false;
                Rotation = start_angle;
                is_attacking = false;
            }
        }
        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);
            if (is_attacking) Attack((float)delta);
        }
    }
}
