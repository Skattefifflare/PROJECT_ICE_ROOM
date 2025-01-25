using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Enemies;


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
        direction = player_distance.Normalized();
        Velocity = direction * speed;
    }
    private void WalkToPlayerEnd() {
        Velocity = Vector2.Zero;
    }



    public override void _Ready() {
        base._Ready();

        Idle = new(IdleStart);
        WalkToPlayer = new(WalkToPlayerStart, WalkToPlayerRunning, WalkToPlayerEnd);

        Idle.BindStates(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => Math.Abs(player_distance.Length()) > 60, WalkToPlayer)
        });

        WalkToPlayer.BindStates(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => player_distance.Length() < 60, Idle)
        });

        current_state = Idle;
    }
}
