using Godot;
using System;

public partial class Fox : CreatureClass
{
    public override void _Ready() {
        base._Ready();
        hp = 50;

    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }

    protected override void StateMachine() {
        
        CallState("idle");
        
    }

    private void RunTowardsPlayer() {        
        sprite_player.Play("walk");

        Vector2 velocity = Velocity;
        velocity.X -= 20;
        velocity.Y += 20;
        Velocity = velocity;
        GD.Print("walk");
    }
}
