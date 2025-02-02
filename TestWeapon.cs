using Godot;
using System;


[Tool]
public partial class TestWeapon : Sprite2D
{
	Marker2D left_marker;
	Marker2D right_marker;

	Bone2D left_bone;
	Bone2D right_bone;

	Node2D left_hand;
	Node2D right_hand;

	Node2D parent;

	AnimationPlayer animation;

	float left_dist;
	float right_dist;


	float arm_len = 12f;

	bool far_left;
	bool far_right;

	public override void _Ready()
	{
		left_marker = (Marker2D)FindChild("left_marker");
        right_marker = (Marker2D)FindChild("right_marker");

		left_bone = (Bone2D)GetNode("%upper_left");
        right_bone = (Bone2D)GetNode("%upper_right");

		left_hand = (Node2D)GetNode("%left_hand");
		right_hand = (Node2D)GetNode("%right_hand");

		parent = (Node2D)GetParent();
		animation = (AnimationPlayer)FindChild("animation");
    }

	public override void _Process(double delta)
	{
		parent.Rotation = Math.Clamp(Mathf.Atan2(GetGlobalMousePosition().Y, GetGlobalMousePosition().X), -Mathf.Pi/2, Mathf.Pi/2);
		if (Input.IsActionJustPressed("ui_left")) {
			animation.Play("attack");
		}



		left_dist = (left_marker.GlobalPosition - left_bone.GlobalPosition).Length();
		right_dist = (right_marker.GlobalPosition - right_bone.GlobalPosition).Length();

		far_left = left_dist > 12f ? true : false;
		far_right = right_dist > 12f ? true : false;

		if (far_left && far_right) {
			Vector2 hand_diff = right_hand.GlobalPosition - left_hand.GlobalPosition;

			float hand_angle = Mathf.Atan2(hand_diff.Y, hand_diff.X);
			this.Rotation += hand_angle;

			Vector2 left_hand_dist = left_marker.GlobalPosition - left_hand.GlobalPosition;
            Vector2 right_hand_dist = right_marker.GlobalPosition - right_hand.GlobalPosition;
			this.GlobalPosition -= left_hand_dist.Length() > right_hand_dist.Length() ? left_hand_dist : right_hand_dist;
		}
		else if (far_left) {

		}
		else if (far_right) {

		}
    }
}
