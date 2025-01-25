using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        private State Attack;


        public override void _Ready() {
            base._Ready();

            sh.SetStates(new List<State> {Walk, Idle });
        }

        public override void _PhysicsProcess(double delta) {
            direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            base._PhysicsProcess(delta);
        } 
    }
}
