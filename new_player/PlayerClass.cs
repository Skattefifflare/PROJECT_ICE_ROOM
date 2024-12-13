using Godot;
using System;
using System.Collections.Generic;


public partial class PlayerClass : CreatureClass {
	public const float Speed = 300.0f;

    public override void _Ready() {
        base._Ready();

		hp = 100;
		AddStates(new Dictionary<string, Action>() {
			{"walk", Walk }
		});
    }


    public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
	}


	protected override void StateMachine() {
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero) {
			CallState("walk");
		}
		else {
			CallState("idle");
		}
	}

	private void Walk() {
		is_busy = false;
		sprite_player.Play("walk");
	}
}