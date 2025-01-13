using Godot;
using Project_Ice_Room.Scriptbin;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        private State Walk;

        public override void _Ready() {
            base._Ready();

            Walk = new(
                () => DIRECTION != Vector2.Zero,
                true,
                () => {
                    if (DIRECTION.X > 0) SPRITE_PLAYER.FlipH = true;
                    else SPRITE_PLAYER.FlipH = false;
                },



            );

        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);

            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }


        
    }
}
