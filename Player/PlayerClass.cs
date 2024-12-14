using Godot;
using System;
using System.Collections.Generic;
using System.Threading;


public partial class PlayerClass : CreatureClass {
	public const float Speed = 80f;
	private Vector2 direction;
    public override void _Ready() {
        base._Ready();

		hp = 100;
		AddStates(new Dictionary<string, Action>() {
			{"walk", Walk }
		});

		Thread thread = new Thread( () => {
			CallDeferred(nameof(Increment));
		});
		thread.Start();
    }


	protected override void StateMachine() {
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero) {
			CallStateAsThread("walk");
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

	void Increment() {
		while (true) {
			Thread.Sleep(1000);
			GD.Print(this.Position);			
		}
	}
}