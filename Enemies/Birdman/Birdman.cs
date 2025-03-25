using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Enemies;
using System.Linq;
public partial class Birdman : Enemy
{
    float idletime = 0;
    float idletime_max = 0;
    float runtime = 0;
    float runtime_max = 0;


    State Idle;
    void IdleStart() {
        sprite_player.Play("idle");
        Velocity = Vector2.Zero;
        idletime_max = (float)GD.RandRange(5, 20) / 5;
    }
    void IdleRunning() {
        idletime += (float)GetProcessDeltaTime();
    }
    void IdleEnd() {
        idletime = 0;
    }


    State RunAround;
    void RunAroundStart() {
        sprite_player.Play("walk");
        runtime_max = (float)GD.RandRange(5, 20)/5;
        nav_agent.TargetPosition = GlobalPosition + new Vector2(speed * runtime_max, 0).Rotated(GD.RandRange(-3, 3));
        //nav_agent.TargetPosition = GlobalPosition + new Vector2(100, 100);
    }
    void RunAroundRunning() {
        runtime += (float)GetProcessDeltaTime();
        direction = (nav_agent.GetNextPathPosition() - GlobalPosition).Normalized();
        Velocity = direction * speed;
    }
    void RunAroundEnd() {
        runtime = 0;
        Velocity = Vector2.Zero;
    }

    State Fly;

    public override void _Ready() {
        base._Ready();

        Idle = new(IdleStart, IdleRunning, IdleEnd);
        RunAround = new(RunAroundStart, RunAroundRunning, RunAroundEnd);

        Idle.BindConditions(new (Func<bool>, State)[] {
             (() => hp <= 0, Die),
             (() => idletime >= idletime_max, RunAround)
        });
        RunAround.BindConditions(new (Func<bool>, State)[] {
            (() => hp <= 0, Die),
            (() => runtime >= runtime_max, Idle) 
        });

        current_state = Idle;
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
    }

}
