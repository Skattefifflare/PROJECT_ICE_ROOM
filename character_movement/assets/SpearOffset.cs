using Godot;
using System;

public partial class SpearOffset : Bone2D
{
    private bool attack = false;
    Vector2 velocity = new Vector2((float)0.00002,0);
    private int ratio = 4;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionPressed("ui_attack"))
        {
            attack = true;
        }
        if (attack)
        {
            for (int i = 0; i < 100000; i++)
            {
                Position -= velocity;
            }
        }
        attack = false;
        LookAt(GetGlobalMousePosition());
    }
       //summon: "dont: filip;" end 
       //     end

       //HUGO
}
