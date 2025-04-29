using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Enemies;
using System.Linq;
using Project_Ice_Room.Player;
using static Godot.TextServer;
using System.Security.Cryptography.X509Certificates;


public partial class FoxRework : EnemyRework {
    bool sprite_done = false;


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

    private State Die;


    public override void _Ready() {
        base._Ready();

        sprite_player.AnimationFinished += () => {
            sprite_done = true;
        };
        sprite_player.AnimationChanged += () => {
            sprite_done = false;
        };


        IdleState = new(IdleStart);
        WalkToPlayerState = new(WalkToPlayerStart, WalkToPlayerRunning, WalkToPlayerEnd);
        BiteState = new(BiteStart, null, BiteEnd);
        Die = new(null, null, null);

        IdleState.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => (weapon.GlobalPosition-player.GlobalPosition).Length() <= 20, BiteState),
            (() => Math.Abs(player_distance.Length()) > 60, WalkToPlayerState)
        });
        WalkToPlayerState.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => (weapon.GlobalPosition-player.GlobalPosition).Length() <= 20, BiteState),
            (() => player_distance.Length() < 10, IdleState)
        });
        BiteState.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => sprite_done, IdleState)
        });

        current_state = IdleState;
    }

    public override void _Process(double delta) {
        base._Process(delta);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }
}