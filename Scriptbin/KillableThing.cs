using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin {
    public partial class KillableThing : CharacterBody2D {

        public GameManager gm;

        public bool taken_dmg_flag = false;
        public bool death_flag = false;
        public int hp;
        public DamageHandler dmgh;

        // And the Gamedev said, "All things which can die must have feet."
        // And so it was that all things which perish must have feet because edge cases are annoying.
        public CollisionShape2D feet;
        // all of these are dubious to have in the base class.
        public AnimatedSprite2D sprite_player;
        public Area2D hitbox;

        public override void _Ready() {
            base._Ready();
            gm = (GameManager)FindParent("gm");
            sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
            feet = (CollisionShape2D)FindChild("feet");
            hitbox = (Area2D)FindChild("hitbox");
            dmgh = new DamageHandler(hitbox, hp, ref taken_dmg_flag, ref death_flag);
            hitbox.Renamed += () => dmgh.UpdateHitbox(hitbox); // dont know if this works
        }
    }
}
