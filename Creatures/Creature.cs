using Godot;
using System;
using Project_Ice_Room.Scripts;
using System.Collections.Generic;

public partial class Creature : CharacterBody2D
{
    [Export]
    public int speed = 100;
    [Export]
    public int hp = 100;


    protected Vector2 direction = new();
    protected Area2D hitbox;
    protected AnimatedSprite2D sprite_player;

    internal Dictionary<string, Flag> flags;
    
    protected Area2D enemy_hitbox;


    public override void _Ready() {
        base._Ready();

        flags = new Dictionary<string, Flag>() {
        { "idle", new Flag() },
        { "walk", new Flag() },
        { "attack", new Flag() },
        { "damaged", new Flag() },
        { "dead", new Flag() },
        };

        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");


        
        hitbox = (Area2D)FindChild("hitbox");
        hitbox.AreaEntered += (b) => HitboxHit(b);
        
    }

    void HitboxHit(Area2D b) {
        flags["damaged"].Set(true);
        flags["idle"].Set(false);
        flags["walk"].Set(false);
        enemy_hitbox = b;
        GD.Print("HitboxHit");
    }

    public virtual void FlagMan() {
        GD.Print("FlagMan not overridden by child class");
    }
    public virtual void ActionMan() {
        GD.Print("ActionMan not overridden by child class");
    }
    public virtual void UpdateMan() {
        foreach (Flag flag in flags.Values) {
            flag.Update();
        }
    }

    public override void _Process(double delta) {
        base._Process(delta);
        FlagMan();
        ActionMan();
        UpdateMan();
    }

}
