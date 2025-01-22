using System;
using Godot;
using Project_Ice_Room.Scriptbin;
using System.Collections.Generic;

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

    internal State Walk;
    internal State Idle;


    public override void _Ready() {
        base._Ready();

        SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");
        WHAP = (Weapon)FindChild("weapon");
        FEET = (Area2D)FindChild("feet");
        HITBOX = (Area2D)FindChild("hitbox");
        DIRECTION = Vector2.Zero;
        SH = new(SPRITE_PLAYER);

        
        Idle = new(
                () => true,
                () => DIRECTION != Vector2.Zero,
                () => Velocity = Vector2.Zero,
                () => { return; },
                "idle",
                false
            );
        Walk = new(
            () => DIRECTION != Vector2.Zero,
            () => true,
            () => Velocity = DIRECTION * SPEED,
            () => { return; },
            "walk",
            true
        );
    }
    public override void _Process(double delta) {
        base._Process(delta);       
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        SH.CallStateHandler();
        if (DIRECTION.X > 0) SPRITE_PLAYER.FlipH = true;
        else SPRITE_PLAYER.FlipH = false;
        MoveAndSlide();
    }
}

