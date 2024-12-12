using Godot;
using System;

public partial class Fox : Enemy
{
    public override void _Ready() {
        hp = 50;
        base._Ready();

        state_dict["run_towards_player"] = RunTowardsPlayer;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        MoveAndSlide();
    }

    public override void StateMachine() {
        if (sees_player) {
            CallState("run_towards_player");
        }
        else {
            CallState("idle");
        }
    }

    void RunTowardsPlayer() {
        state_is_busy = true;
        sprite_player.Play("walk");

        Vector2 velocity = Velocity;
        velocity.X -= 20;
        velocity.Y += 20;
        Velocity = velocity;
        GD.Print("walk");
    }
}
