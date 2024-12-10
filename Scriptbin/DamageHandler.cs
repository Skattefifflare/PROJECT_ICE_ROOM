using Godot;
using System;

public class DamageHandler
{
	int hp;
	Area2D hitbox;
    bool successfull_hit = false;

	Area2D damage_box;
	int proposed_dmg;


    public DamageHandler(Area2D hitbox, int hp, ref bool has_taken_dmg, ref bool has_died) { 
		this.hp = hp;
		this.hitbox = hitbox;
		
	}

	public void SendHitRequest(Area2D damage_box, int proposed_dmg) { // call this method at start of attack

		this.damage_box = damage_box;
		this.proposed_dmg = proposed_dmg;

		hitbox.AreaEntered += (entered_box) => HitMethod(entered_box, damage_box, proposed_dmg);
	}	
	public void EndHitRequest() { // call this method when attack has ended
        hitbox.AreaEntered -= (entered_box) => HitMethod(entered_box, damage_box, proposed_dmg);
    }

	private void HitMethod(Area2D x, Area2D y, int dmg) {
        if (x != y) return;
        hp -= dmg;
        successfull_hit = true;
    }

	public void UpdateHitbox(Area2D hitbox) { 
		// remember that the creature needs to manually signal this when its hitbox is changed.
	}
}
