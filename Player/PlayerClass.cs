using Godot;
using System;
using System.Collections.Generic;


public partial class PlayerClass : CreatureClass {
	public const float Speed = 80f;
	private Vector2 direction;
    public override void _Ready() {
        base._Ready();

		hp = 100;
		AddStates(new Dictionary<string, Action>() {
			{"walk", Walk }
		});
    }


	protected override void StateMachine() {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero) {
			CallState("walk");
		}
		else {
			//CallState("idle");
		}

		if (hp < 0) {
			GD.Print("player died");
		}
	}

	private void Walk() {
		is_busy = false;
		// sprite_player.Play("walk");
		Velocity = direction * Speed;
	}
}