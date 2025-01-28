using Godot;
using System;

[Tool]
public partial class WeaponPos : Node2D
{
	Bone2D right_hand;
	Bone2D left_hand;


	Vector2 midpoint;
	float angle;
	Vector2 diff;
	public override void _Ready()
	{
		try {
            right_hand = (Bone2D)FindChild("rh");
            left_hand = (Bone2D)FindChild("lh");
			GD.Print(right_hand.Position);
        }
		catch {
			GD.Print("cant find hands");
		}
    }

	
	public override void _Process(double delta)
	{
		diff = (right_hand.GlobalPosition - left_hand.GlobalPosition);

		midpoint = left_hand.GlobalPosition + (diff / 2);
		angle = Mathf.Atan2(diff.Y, diff.X);
		angle = Mathf.RadToDeg(angle);

		Rotation = angle;

	}
}
