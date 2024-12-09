using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AI : CharacterBody2D
{
	CollisionShape2D view_field;
	AnimatedSprite2D sprite_player;
	Dictionary<string, Action> state_dict;

	public bool state_is_busy = false;
	Action current_state;


    public override void _Ready() {
        base._Ready();	
        view_field = (CollisionShape2D)FindChild("view_field");
		sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
		state_dict = new Dictionary<string, Action>() {
			{"idle", Idle},
			{"walk", Walk},
			{"death", Death},
		};
    }

    public override void _Process(double delta) {
        base._Process(delta);
		GD.PrintErr("You must implement an overridden _Process without calling base._Process.");
    }

    internal virtual void DecideState() {
		GD.PrintErr("DecideState has to be implemented in an extended script.");
	}
	void StateCaller(string state) {
		if (state_is_busy) return;
		current_state = state_dict[state];
		try {
			current_state();
		}
		catch {
			GD.PrintErr("state '" + state + "' could not be started. Make sure animations and nodes needed exist and are properly named");
		}
	}

	void Idle() {
		state_is_busy = false;
		sprite_player.Play("idle");
	}
	void Death() {
		state_is_busy = true;
		sprite_player.Play("death");
	}
	void Walk() {
		state_is_busy = false;
		sprite_player.Play("walk");
	}
}
