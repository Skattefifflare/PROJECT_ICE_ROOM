using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using static Godot.TextServer;
using Project_Ice_Room;

namespace Project_Ice_Room.Scriptbin;
public partial class Creature : CharacterBody2D {

    [Export]
    protected int HP = 1;
    [Export]
    protected int SPEED = 1;

    protected AnimatedSprite2D SPRITE_PLAYER;
    protected Weapon WHAP;
    protected Area2D FEET;
    protected Area2D HITBOX;
    protected Action CURRENT_ACTION;
    protected Vector2 DIRECTION;
    protected StateHandler SH;


    public State DieState;
    public State WalkState;
    public State IdleState;
    public State AttackMovingState;


    public override void _Ready() {
        base._Ready();

        SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");
        WHAP = (Weapon)FindChild("weapon");
        FEET = (Area2D)FindChild("feet");
        HITBOX = (Area2D)FindChild("hitbox");
        DIRECTION = Vector2.Zero;
        SH = new(SPRITE_PLAYER);

        DieState = new State(
            () => HP <= 0,
            Die,
            true,
            "die"
        );
        WalkState = new State(
            () => DIRECTION != Vector2.Zero,
            Walk,
            true,
            "walk"
        );
        IdleState = new State(
            () => true,
            Idle,
            false,
            "idle"
        );
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        SH.DecideState();
        SH.CallState();
        SH.PlaySprite();

        if (DIRECTION.X > 0) SPRITE_PLAYER.FlipH = true;
        else SPRITE_PLAYER.FlipH = false;

        MoveAndSlide();
    }
    public virtual void Die() {
        SPRITE_PLAYER.AnimationFinished += () => QueueFree();
    }
    public virtual void Walk() {
        Vector2 pos = Position + (DIRECTION * SPEED);
        Position = pos;
    }
    public virtual void Idle() {
        Velocity = Vector2.Zero;
    }


    public virtual void AttackMoving() {
        WHAP.MakeDangerous();
        SPRITE_PLAYER.AnimationFinished += WHAP.MakeHarmLess;
    }
    public virtual void Attack() {
        
    }
    public virtual void TakeDamage() {
        
    }
}

