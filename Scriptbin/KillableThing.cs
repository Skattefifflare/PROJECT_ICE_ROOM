using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin {
    public partial class KillableThing : CharacterBody2D {

        protected GameManager gm;
        protected bool taken_dmg_flag = false;
        protected bool death_flag = false;
        protected int hp;
        public DamageHandler dmgh;
        protected CollisionShape2D feet;
        protected AnimatedSprite2D sprite_player;
        protected Area2D hitbox; // the damagehandler hitbox automatically updates when this hitbox changes.

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
