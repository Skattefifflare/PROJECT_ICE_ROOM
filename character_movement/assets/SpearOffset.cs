using Godot;
using System;

public partial class SpearOffset : Bone2D
{
    private bool attack = false;
    Vector2 velocity = new Vector2;
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
            for (int i = 0; i < 6000; i++)
            {
                Position = 
            }
        }
        GD.Print(Position.X);
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
