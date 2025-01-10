using System;
using Godot;


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

    // shared states across creatures
    protected State DieState;
    protected State WalkState;
    protected State IdleState;
    protected State AttackMovingState;


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
        Velocity = DIRECTION * SPEED;
    }
    public virtual void Idle() {
        Velocity = Vector2.Zero;
    }
    public virtual void AttackMoving() {
        WHAP.MakeDangerous();
        SPRITE_PLAYER.AnimationFinished += WHAP.MakeHarmLess; // we can later on exchange this for when the arm's animation is finished, allowing us to only need the Attack method
    }
    public virtual void Attack() {
        WHAP.MakeDangerous();
        SPRITE_PLAYER.AnimationFinished += WHAP.MakeHarmLess;
    }
    public virtual void TakeDamage() {
        
    }
}

