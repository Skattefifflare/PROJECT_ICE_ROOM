using Godot;
using System;
using System.Collections.Generic;
using System.Threading;


public partial class PlayerClass : CreatureClass {
	public const float Speed = 100f;
	private Vector2 direction;
    public override void _Ready() {
        base._Ready();

		hp = 100;
		speed = 90;
		AddStates(new Dictionary<string, Action>() {
			{"walk", Walk }
		});
    }


	protected override void StateMachine() {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (Input.IsActionJustPressed("attack")) {
			CallState("attack");
		}
		else if (direction != Vector2.Zero) {
			CallState("walk");
		}
		else {
			CallState("idle");		
		}
	}

	private void Walk() {
		sprite_player.Play("walk");
		is_busy = false;		
		Velocity = direction * Speed;
	}
	

	
}