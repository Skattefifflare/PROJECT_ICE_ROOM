using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AI : KillableThing
{
	Area2D view_field;
	public bool sees_player = false;
	CharacterBody2D player; // bad idea, how is the player supposed to find the fox?

	public Dictionary<string, Action> state_dict;
	public bool state_is_busy = false;
	Action current_state;

	
    public override void _Ready() {
        base._Ready();	

        view_field = (Area2D)FindChild("view_field");
		view_field.AreaEntered += (object_that_entered) => {
			if (object_that_entered.Name.ToString() == "visibility_box") {
                sees_player = true;
                player = (CharacterBody2D)object_that_entered.FindParent("player");
            }       
        }; 

		state_dict = new Dictionary<string, Action>() {
			{"idle", Idle},
			{"walk", Walk},
			{"death", Death},
		};
    }

    public virtual void StateMachine() {
		GD.PrintErr("DecideState has to be implemented in a child script.");
	}
	public void CallState(string state) {
		if (state_is_busy) return;
		current_state = state_dict[state];
		try {
			current_state();
		}
		catch {
			GD.PrintErr("state '" + state + "' could not be started. Make sure animations and nodes needed exist and are properly named");
		}
	}

	public virtual void Idle() {
		state_is_busy = false;
		sprite_player.Play("idle");
	}
    public virtual void Death() {
		state_is_busy = true;
		sprite_player.Play("death");
	}
    public virtual void Walk() {
		state_is_busy = false;
		sprite_player.Play("walk");
	}
	public virtual void DamageReaction() {

	}
}
