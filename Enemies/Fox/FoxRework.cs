using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Enemies;
using System.Linq;
using Project_Ice_Room.Player;
using static Godot.TextServer;


public partial class FoxRework : EnemyRework {
    private State IdleState;

    private void IdleStart() {
        sprite_player.Play("idle");
        Velocity = Vector2.Zero;
    }

    private State WalkToPlayerState;
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

    private State BiteState;
    private void BiteStart() {
        Velocity = Vector2.Zero;
        sprite_player.Play("bite");
        weapon.MakeDangerous();
    }
    private void BiteEnd() {
        weapon.MakeHarmless();
    }
}
