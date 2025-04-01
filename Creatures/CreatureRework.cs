using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Ice_Room.Scriptbin;

namespace Project_Ice_Room.Creatures;

public partial class CreatureRework : CharacterBody2D {
    [Export]
    public int hp = 1;
    [Export]
    protected int speed = 1;

    private bool facing_right = true;
    

    protected Area2D hitbox;
    protected Vector2 direction;

    protected State current_state;

    public override void _Ready() {
        base._Ready();
        hitbox = (Area2D)FindChild("hitbox");
        direction = Vector2.Zero;

    }
    public override void _Process(double delta) {
        base._Process(delta);
        current_state.Call(ref current_state);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        MoveAndSlide();
    }
}

