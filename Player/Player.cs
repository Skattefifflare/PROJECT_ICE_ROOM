using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        private State Attack;

        public override void _Ready() {
            base._Ready();
         
            Attack = new(
                () => Input.IsActionPressed("attack"),
                () => WHAP.finished_attack,
                () => WHAP.is_attacking = true,
                () => { return; },
                "attack",
                true
            );
            SH.SetStates(new List<State> {Attack, Walk, Idle });
        }

        public override void _PhysicsProcess(double delta) {
            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            base._PhysicsProcess(delta);
        } 
    }
}
