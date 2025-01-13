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



    public override void _Ready() {
        base._Ready();

        SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");
        WHAP = (Weapon)FindChild("weapon");
        FEET = (Area2D)FindChild("feet");
        HITBOX = (Area2D)FindChild("hitbox");
        DIRECTION = Vector2.Zero;
        SH = new(SPRITE_PLAYER);

    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        
        if (DIRECTION.X > 0) SPRITE_PLAYER.FlipH = true;
        else SPRITE_PLAYER.FlipH = false;
        
        SH.CallStateHandler();

        MoveAndSlide();
    }
}

