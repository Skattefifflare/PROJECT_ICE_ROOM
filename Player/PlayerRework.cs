using Godot;
using Project_Ice_Room.Creatures;
using Project_Ice_Room.Scriptbin;
using System;


[Tool]
public partial class PlayerRework : CreatureRework
{
	Node2D frontarmtarget;
	Node2D backarmtarget;
	Node2D frontlegtarget;
	Node2D backlegtarget;

    Node2D local_frontarmtarget;
    Node2D local_backarmtarget;
    Node2D local_frontlegtarget;
    Node2D local_backlegtarget;



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
		frontarmtarget = (Node2D)FindChild("frontarmtarget");
        backarmtarget = (Node2D)FindChild("backarmtarget");
        frontlegtarget = (Node2D)FindChild("frontlegtarget");
        backlegtarget = (Node2D)FindChild("backlegtarget");

		local_frontarmtarget = (Node2D)FindChild("local_frontarmtarget");
        local_backarmtarget = (Node2D)FindChild("local_backarmtarget");
        local_frontlegtarget = (Node2D)FindChild("local_frontlegtarget");
        local_backlegtarget = (Node2D)FindChild("local_backlegtarget");


        animation_player = (AnimationPlayer)FindChild("animation_player");
        if (weapon_slot.GetChildCount() != 0) weapon = (Weapon)weapon_slot.GetChild(0);


        DieState = new(DieStart, null, null);
        IdleState = new(IdleStart, null, IdleEnd);
        IdleState.BindConditions(new (Func<bool>, State)[] {
                (() => hp <= 0, DieState),
                (() =>direction != Vector2.Zero, RunState),
            });
        
        RunState = new(RunStart, RunRunning, RunEnd);
        RunState.BindConditions(new (Func<bool>, State)[] {
                (() => hp <= 0, DieState),
                (() => direction == Vector2.Zero, IdleState)
            });


    }


	public override void _Process(double delta)
	{
        local_frontarmtarget.GlobalPosition = frontarmtarget.Position + new Vector2(50, 50);
        local_backarmtarget.Position = backarmtarget.Position + new Vector2(50, 50);
        local_frontlegtarget.Position = frontlegtarget.Position + new Vector2(50, 50);
        local_backlegtarget.Position = backlegtarget.Position + new Vector2(50, 50);



    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        direction = Input.GetVector("left", "right", "up", "down");
        base._PhysicsProcess(delta);
    }
}
