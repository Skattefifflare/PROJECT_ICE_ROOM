using Godot;
using Project_Ice_Room.Scriptbin;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        private State Walk;
        private State Idle;

        public override void _Ready() {
            base._Ready();

            Walk = new(
                () => DIRECTION != Vector2.Zero,
                () => true,
                () => {
                    Velocity = DIRECTION * SPEED;
                },
                false,
                "walk"
            );
            Idle = new(
                () => DIRECTION == Vector2.Zero,
                () => DIRECTION != Vector2.Zero,
                () => {
                    Velocity = Vector2.Zero;
                },
                false,
                "idle"
            );

            SH.SetStates(new List<State> { Walk, Idle });
        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);

            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }


        
    }
}
