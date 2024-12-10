using Godot;
using System;

public partial class FoxAI : AI
{
    public override void _Ready() {
        base._Ready();

        state_dict["run_towards_player"] = RunTowardsPlayer;
        dmgh = new DamageHandler(hitbox, 50, ref taken_dmg_flag, ref death_flag);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        StateMachine();

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
        base.Walk();
        state_is_busy = true;


        Vector2 velocity = Velocity;
        velocity.X -= 20;
        velocity.Y += 20;
        Velocity = velocity;
        GD.Print("walk");
    }
}
