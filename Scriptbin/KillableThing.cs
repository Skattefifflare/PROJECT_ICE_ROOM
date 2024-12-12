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
        public CollisionShape2D feet;
        public AnimatedSprite2D sprite_player;
        public Area2D hitbox; // the damagehandler hitbox automatically updates when this hitbox changes.

        public override void _Ready() {
            base._Ready();
            gm = (GameManager)FindParent("gamemaster");
            sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
            feet = (CollisionShape2D)FindChild("feet");
           
            dmgh = new DamageHandler(hp, ref taken_dmg_flag, ref death_flag);
            hitbox = (Area2D)FindChild("hitbox");
            hitbox.Renamed += () => dmgh.UpdateHitbox(hitbox); // dont know if this works
        }
    }
}
