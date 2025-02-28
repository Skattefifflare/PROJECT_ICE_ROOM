using Godot;
using System;


namespace Project_Ice_Room;

public partial class Armscript : Node2D
{

	Player.Player player;
	Skeleton2D sub_port;
	

	public override void _Ready()
	{
		player = (Player.Player)GetParent();
		sub_port = (Skeleton2D)FindChild("skeleton");
	}

	public override void _Process(double delta)
	{
		sub_port.GlobalPosition = player.GlobalPosition + new Vector2(-25, -25);
	}
}
