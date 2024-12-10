using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin {
    public partial class KillableThing : CharacterBody2D {

        public bool taken_dmg_flag = false;
        public bool death_flag = false;
        public DamageHandler dmgh;

        public AnimatedSprite2D sprite_player;
        public Area2D hitbox;
        public CollisionShape2D feet;

        public override void _Ready() {
            base._Ready();
            sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
        }
    }
}
