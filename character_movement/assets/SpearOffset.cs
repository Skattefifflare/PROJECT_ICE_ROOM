using Godot;
using System;

[Tool]
public partial class SpearOffset : Bone2D
{
    private bool attack = false;
    Vector2 velocity = new Vector2((float)0.00002,0);
    private int ratio = 4;
    private AnimationPlayer animation;

    public override void _Ready()
    {
        base._Ready();

        animation = (AnimationPlayer)GetChild(2);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionPressed("left_click"))
        {
            attack = true;
        }
        if (attack)
        {
            animation.Play("ATTACKKK");
        }
        attack = false;
        LookAt(GetGlobalMousePosition());
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
