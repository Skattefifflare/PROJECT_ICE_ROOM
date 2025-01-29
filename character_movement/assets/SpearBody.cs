using Godot;
using System;

[Tool]
public partial class SpearBody : Bone2D
{
    private double offsetmultiplier = 0.5;
    private double distancemax = 20;

    public override void _Process(double delta)
    {
        base._Process(delta);

        //Make spear slightly move toward cursor
        Vector2 GlobalMousePosition = GetGlobalMousePosition();
        Vector2 GlobalPosition = GetGlobalTransform().Origin;
        double distance = Math.Sqrt(Math.Pow(GlobalMousePosition.X-GlobalPosition.X, 2) + Math.Pow(GlobalMousePosition.Y-GlobalPosition.Y, 2)); //Pythagorean theorem
        Vector2 offset = new((float)Math.Abs(distance * offsetmultiplier),0);
        if (distance * offsetmultiplier >= distancemax) //Add max value to prevent spear from moving too far away from player
        {
            offset = new Vector2((float)distancemax, 0);
        }
        Position = offset;

        LookAt(GlobalMousePosition);
    }
}
