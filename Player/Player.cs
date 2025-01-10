using Godot;
using Project_Ice_Room.Scriptbin;
using System.Collections.Generic;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {

        

        public override void _Ready() {
            base._Ready();

            State AttackState = new State(
            () => Input.IsActionPressed("attack"),
            Attack,
            true,
            "attack"
            );

            SH.SetStates(new List<State>() { DieState, AttackState, WalkState, IdleState });
            
        }

        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);

            DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        }


        
    }
}
