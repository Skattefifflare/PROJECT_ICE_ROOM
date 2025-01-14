using Godot;
using System;

public partial class SpearOffset : Bone2D
{
    private bool attack = false;
    private int velocity = 2;
    private int ratio = 4;

    public override void _Process(double delta)
    {
        base._Process(delta);
        LookAt(GetGlobalMousePosition());

        if (Input.IsActionPressed("ui_attack"))
        {
            attack = true;
        }
        if (attack)
        {
            
        }
        GD.Print(Position.X);
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
