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

            Idle = new(
                () => true,
                () => DIRECTION != Vector2.Zero,
                () => Velocity = Vector2.Zero,
                () => { return; },
                "idle",
                false
            );
            Walk = new(
                () => DIRECTION != Vector2.Zero,
                () => DIRECTION == Vector2.Zero,
                () => Velocity = DIRECTION * SPEED,
                () => { return; },
                "walk",
                true
            );
            Attack = new(
                () => Input.IsActionPressed("attack"),
                () => WHAP.finished_attack,
                () => WHAP.is_attacking = true,
                () => { return; },
                "idle",
                true
            );


            SH.SetStates(new List<State> {Attack, Walk, Idle });
        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);

            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            SH.CallStateHandler();
        }


        
    }
}
