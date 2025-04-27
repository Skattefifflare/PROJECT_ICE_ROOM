using Godot;
using Project_Ice_Room.Creatures;
using Project_Ice_Room.Scriptbin;
using System;


public partial class PlayerRework : CreatureRework {
    private AnimationPlayer animation_player;
    private Weapon weapon;
    private Node weapon_slot;
    protected Area2D feet;

    private State IdleState;
    private void IdleStart() {
        Velocity = Vector2.Zero;
        animation_player.Play("idle");
    }
    private void IdleEnd() {
        animation_player.Stop();
    }
    private State RunState;
    private void RunStart() {
        animation_player.Play("run");
    }
    private void RunRunning() {
        Velocity = direction * speed;
    }
    private void RunEnd() {
        Velocity = Vector2.Zero;
        animation_player.Stop();
    }
    private State DieState;
    private void DieStart() {
        animation_player.Play("die");
        Velocity = Vector2.Zero;
    }

    public override void _Ready() {
        animation_player = (AnimationPlayer)FindChild("animation_player");
        weapon_slot = (Node)FindChild("weapon_slot");
        if (weapon_slot.GetChildCount() != 0) weapon = (Weapon)weapon_slot.GetChild(0);
        flip_node = (Node2D)FindChild("flip_node");
        feet = (Area2D)FindChild("feet");


        DieState = new(DieStart, null, null);
        IdleState = new(IdleStart, null, IdleEnd);
        RunState = new(RunStart, RunRunning, RunEnd);

        IdleState.BindConditions(new (Func<bool>, State)[] {
                (() => hp <= 0, DieState),
                (() =>direction != Vector2.Zero, RunState),
            });


        RunState.BindConditions(new (Func<bool>, State)[] {
                (() => hp <= 0, DieState),
                (() => direction == Vector2.Zero, IdleState)
            });


        current_state = IdleState;
    }


    public override void _Process(double delta) {
        base._Process(delta);
        ZIndex = (int)(feet.GlobalPosition.Y);
    }

    public override void _PhysicsProcess(double delta) {

        direction = Input.GetVector("left", "right", "up", "down");
        base._PhysicsProcess(delta);
        GD.Print(this.GlobalPosition);
        GD.Print("v: " + this.Velocity);
        FlipFlop();
    }


    private Node2D flip_node;

    private void FlipFlop() {
        if (direction.X < 0) {
            flip_node.Scale = new Vector2(1, 1);

        }
        else if (direction.X > 0) {
            flip_node.Scale = new Vector2(-1, 1);

        }
    }
}
