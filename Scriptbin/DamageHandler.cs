using Godot;
using System;

public class DamageHandler
{
	int hp;
	Area2D hitbox;
    bool successfull_hit = false;
	Area2D dmg_box;
	int proposed_dmg;


    public DamageHandler(int hp, ref bool has_taken_dmg, ref bool has_died) { 
		this.hp = hp;		
	}

	public void SendHitRequest(Area2D dmg_box, int proposed_dmg) { // call this method at start of attack
		this.dmg_box = dmg_box;
		this.proposed_dmg = proposed_dmg;

		hitbox.AreaEntered += (entered_box) => HitMethod(entered_box, dmg_box, proposed_dmg);
	}	
	public void EndHitRequest() { // call this method when attack has ended
        hitbox.AreaEntered -= (entered_box) => HitMethod(entered_box, dmg_box, proposed_dmg);
    }

	private void HitMethod(Area2D x, Area2D y, int dmg) {
        if (x != y) return;
        hp -= dmg;
        successfull_hit = true;
    }

	public void UpdateHitbox(Area2D hitbox) {
		this.hitbox = hitbox;
	}
	public void UpdateDmgBox(Area2D dmg_box) {
		this.dmg_box = dmg_box;
	}
}
