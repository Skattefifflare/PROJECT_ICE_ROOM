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

	public void SendAttack(Area2D damage_box, int proposed_dmg, string target = null) {
		
        if (target == null) {
			List<DamageHandler> all_damagehandlers = GetChildren().OfType<KillableThing>().Select(x => x.dmgh).ToList();
            foreach (var damagehandler in all_damagehandlers) {                
                damagehandler.SendHitRequest(damage_box, proposed_dmg);
            }
        }
		else {
            DamageHandler enemy_dmgh;
			enemy_dmgh = GetChildren().OfType<KillableThing>().Where(x => x.Name.ToString() == target).Select(x => x.dmgh).ToList()[0];
		}

		
	}
	public void PlayerStopsAttack() {
        foreach (var enemy in enemies) {
            DamageHandler enemy_dmgh = enemy.GetChildren().OfType<KillableThing>().ToList()[0].dmgh;
            enemy_dmgh.EndHitRequest();
        }
    }
}

