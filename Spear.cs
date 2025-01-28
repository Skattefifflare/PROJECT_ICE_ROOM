using Godot;
using System;

[Tool]
public partial class Spear : Sprite2D
{

	Bone2D lh;
	Bone2D rh;

	Vector2 diff;
	Vector2 pos;
	float rot;
	public override void _Ready()
	{
		lh = (Bone2D)GetNode("../skeleton/ul/ll/lh");
        rh = (Bone2D)GetNode("../skeleton/ur/lr/rh");
    }

	public override void _Process(double delta)
	{
		diff = (lh.GlobalPosition - rh.GlobalPosition);
        pos = diff / 2 + rh.GlobalPosition;
		Position = pos;

		rot = Mathf.Atan2(diff.Y, diff.X);
		rot = Mathf.RadToDeg(rot);

		RotationDegrees = rot + 90;
	}
}
