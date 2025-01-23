using Godot;
using System;
using Project_Ice_Room.Scripts;

public partial class Creature : CharacterBody2D
{
    [Export]
    public int speed = 100;
    [Export]
    public int hp = 100;


    protected Vector2 direction = new();
    protected Area2D hitbox;
    protected AnimatedSprite2D sprite_player;


    internal Flag idle_flag;
    internal Flag walk_flag;
    internal Flag attack_flag;
    internal Flag damaged_flag;
    internal Flag dead_flag;


    protected Area2D enemy_hitbox;


    public override void _Ready() {
        base._Ready();
        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");

        idle_flag = new Flag();
        walk_flag = new Flag();
        attack_flag = new Flag();
        damaged_flag = new Flag();
        dead_flag = new Flag();


        hitbox = (Area2D)FindChild("hitbox");
        hitbox.AreaEntered += (b) => {
            damaged_flag.Update(true);
            enemy_hitbox = b;
        };
    }

    public virtual void FlagMan() {
        GD.Print("FlagMan not overridden by child class");
    }
    public virtual void ActionMan() {
        GD.Print("ActionMan not overridden by child class");
    }

    public override void _Process(double delta) {
        base._Process(delta);
        FlagMan();
        ActionMan();
    }

}
