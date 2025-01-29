using Godot;
using System;


[Tool]
public partial class PathControl : Node2D
{
	[Export (PropertyHint.Range, "0f, 1f")]
	public float progress = 0;

	

	PathFollow2D left_follow;
	PathFollow2D right_follow;
	public override void _Ready()
	{
		left_follow =  (PathFollow2D)FindChild("left_follow");
		right_follow = (PathFollow2D)FindChild("right_follow");
	}

	public override void _Process(double delta)
	{
		left_follow.ProgressRatio = progress;
		right_follow.ProgressRatio = progress;
	}
}
