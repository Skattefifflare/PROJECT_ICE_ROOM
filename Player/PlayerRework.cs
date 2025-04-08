using Godot;
using Project_Ice_Room.Creatures;
using Project_Ice_Room.Scriptbin;
using System;


public partial class PlayerRework : CreatureRework
{
    private AnimationPlayer animation_player;
    private Weapon weapon;
    private Node weapon_slot;

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

    public override void _Ready()
	{
        animation_player = (AnimationPlayer)FindChild("animation_player");
        weapon_slot = (Node)FindChild("weapon_slot");
        if (weapon_slot.GetChildCount() != 0) weapon = (Weapon)weapon_slot.GetChild(0);


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


	public override void _Process(double delta)
	{
        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        direction = Input.GetVector("left", "right", "up", "down");
        GD.Print(this.GlobalPosition);
        GD.Print("v: " + this.Velocity);
    }
}
