using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        private State Walk;
        private State Idle;
        private State Attack;

        public override void _Ready() {
            base._Ready();

            Walk = new(
                () => DIRECTION != Vector2.Zero,
                () => true,
                () => {
                    Velocity = DIRECTION * SPEED;
                },
                true,
                "walk"
            );
            Idle = new(
                () => DIRECTION == Vector2.Zero,
                () => DIRECTION != Vector2.Zero,
                () => {
                    Velocity = Vector2.Zero;
                },
                true,
                "idle"
            );
            Attack = new(
                () => Input.IsActionPressed("attack"),
                () => true,
                () => {
                    WHAP.Rotate(1f * (float)GetPhysicsProcessDeltaTime());
                    //SPRITE_PLAYER.Play("attack");
                    //SPRITE_PLAYER.AnimationFinished += () => Attack.END_CONDITION = () => true;
                },
                false,
                "idle"
            );


            SH.SetStates(new List<State> { Attack, Walk, Idle });
        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);

            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }


        
    }
}
