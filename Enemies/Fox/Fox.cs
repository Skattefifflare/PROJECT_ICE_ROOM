using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Enemies;
using System.Linq;


public partial class Fox : Enemy
{
    private State Idle;
    private void IdleStart() {
        sprite_player.Play("idle");
        Velocity = Vector2.Zero;
    }
    
    private State WalkToPlayer;
    private void WalkToPlayerStart() {
        sprite_player.Play("walk");
    }
    private void WalkToPlayerRunning() {
        nav_agent.TargetPosition = player.Position;
        direction = (nav_agent.GetNextPathPosition() - GlobalPosition).Normalized();
        Velocity = direction * speed;
    }
    private void WalkToPlayerEnd() {
        Velocity = Vector2.Zero;
    }

    private State Bite;
    private void BiteStart() {
        Velocity = Vector2.Zero;
        sprite_player.Play("bite");
        whap.MakeDangerous();
    }
    private void BiteEnd() {
        whap.MakeHarmless();
    }

    public override void _Ready() {
        base._Ready();

        Idle = new(IdleStart);
        WalkToPlayer = new(WalkToPlayerStart, WalkToPlayerRunning, WalkToPlayerEnd);
        Bite = new(BiteStart, null, BiteEnd);

        Idle.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => (whap.GlobalPosition-player.GlobalPosition).Length() <= 20, Bite),
            (() => Math.Abs(player_distance.Length()) > 60, WalkToPlayer)
        });

        WalkToPlayer.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => (whap.GlobalPosition-player.GlobalPosition).Length() <= 20, Bite),
            (() => player_distance.Length() < 10, Idle)
        });

        Bite.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => sprite_done, Idle)
        });
        

        current_state = Idle;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }
}
