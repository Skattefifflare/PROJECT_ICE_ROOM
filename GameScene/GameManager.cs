using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node2D
{
	public Node player;

	public List<Node> enemies;
	public override void _Ready()
	{
		player = FindChild("player");

		enemies = new();
		foreach (var enemy in FindChild("enemies").GetChildren()) {
			enemies.Add(enemy);
		}
	}

	public override void _Process(double delta)
	{


	}

	public void PlayerAttacksEnemy(Area2D damage_box, int proposed_dmg) {
		foreach (var enemy in enemies) {
			DamageHandler enemy_dmgh = enemy.GetChildren().OfType<KillableThing>().ToList()[0].dmgh;
			enemy_dmgh.SendHitRequest(damage_box, proposed_dmg);
		}
	}
	public void PlayerStopsAttack() {
        foreach (var enemy in enemies) {
            DamageHandler enemy_dmgh = enemy.GetChildren().OfType<KillableThing>().ToList()[0].dmgh;
            enemy_dmgh.EndHitRequest();
        }
    }
}

