using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin {
    public partial class Creature : KillableThing {

        public Dictionary<string, Action> state_dict;
        public bool state_is_busy = false;
        public Action current_state;
        public Area2D dmg_box;
        int dmg;

        public override void _Ready() {
            base._Ready();
            state_dict = new Dictionary<string, Action>() {
                {"idle", Idle },
                {"death", Death },
                {"walk", Walk },
                {"damage_reaction", DamageReaction },
                {"attack", Attack }

            };
            dmg_box = new Area2D();
            dmg_box.Renamed += () => dmgh.UpdateDmgBox(dmg_box);
        }
        public override void _PhysicsProcess(double delta) {
            base._PhysicsProcess(delta);
            StateMachine();
        }

        public virtual void StateMachine() {
            GD.PrintErr("DecideState has to be implemented in a child script.");
        }
        public void CallState(string state) {
            if (state_is_busy) return;
            current_state = state_dict[state];
            try {
                current_state();
                state_is_busy = true;
            }
            catch {
                GD.PrintErr("state '" + state + "' could not be started. Make sure animations and nodes needed exist and are properly named");
            }
        }


        public virtual void Idle() {
        }
        public virtual void Death() {
        }
        public virtual void Walk() {
        }
        public virtual void DamageReaction(){
        }
        public virtual void Attack() {         
            try {
                gm.SendAttack(dmg_box, dmg);
                sprite_player.Play("attack");
                Action stop_action = null;
                stop_action = () =>
                {
                    gm.PlayerStopsAttack();
                    sprite_player.AnimationFinished -= stop_action;
                };
                sprite_player.AnimationFinished += stop_action;
            }
            catch {
                GD.PrintErr("probably missing the animation 'attack'");
            }          
        }
    }
}
