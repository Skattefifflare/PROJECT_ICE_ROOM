using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using Project_Ice_Room.Weapons;


namespace Project_Ice_Room.Player {
    public partial class Player : Creature {


        private PlayerWeapon player_whap;

        State Idle;
        private void IdleStart() {
            sprite_player.Play("idle");
            Velocity = Vector2.Zero;
        }
        State Walk;
        private void WalkStart() {
            sprite_player.Play("walk");
        }
        private void WalkRunning() {
            Velocity = direction * speed;
        }
        private void WalkEnd() {
            Velocity = Vector2.Zero;
        }   

        State Attack;


        public override void _Ready() {
            base._Ready();

            player_whap = (PlayerWeapon)FindChild("weapon");

            Idle = new(IdleStart);
            Walk = new(WalkStart, WalkRunning, WalkEnd);

            Idle.BindStates(new (Func<bool>, State)[] {
                (() => hp <= 0, Die),
                (() =>direction != Vector2.Zero, Walk),
            });
            Walk.BindStates(new (Func<bool>, State)[] {
                (() => hp <= 0, Die),
                (() => direction == Vector2.Zero, Idle)
            });

            current_state = Idle;
        }

        public override void _PhysicsProcess(double delta) {
            direction = Input.GetVector("left", "right", "up", "down");
            base._PhysicsProcess(delta);
        } 
    }
}
