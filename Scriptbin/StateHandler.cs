using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Project_Ice_Room.Scriptbin {
    public class StateHandler {
        private List<State> STATE_LIST;
        private List<State> ACTIVE_STATES;
        private AnimatedSprite2D SPRITE_PLAYER;

        public StateHandler(AnimatedSprite2D SPRITE_PLAYER) {
            STATE_LIST = new();
            ACTIVE_STATES = new();
            this.SPRITE_PLAYER = SPRITE_PLAYER;
        }
        public void SetStates(List<State> states) {
            STATE_LIST = states;
        }
        public void DecideState() {
            foreach (var state in STATE_LIST) {
                
                if (!ACTIVE_STATES.Contains(state)) {
                    if (!state.CONDITION()) continue;
                    if (state.IS_EXCLUSIVE)ACTIVE_STATES.Clear();
                    ACTIVE_STATES.Add(state);
                    break;
                }
                else {
                    if (!state.CONDITION()) {
                        ACTIVE_STATES.Remove(state);
                        continue;
                    }
                    else {
                        if (state.IS_EXCLUSIVE) break;
                    }
                }
            }
        }
        public void CallState() {
            foreach (var state in ACTIVE_STATES) {
                state.STATE_METHOD();
            }
        }
        public void PlaySprite() {
            if (ACTIVE_STATES.All(s => s.IS_EXCLUSIVE == false)) {
                CallSpritePlayer(ACTIVE_STATES.First().SPRITE);
            }
            else {
                foreach (var state in ACTIVE_STATES) {
                    if (state.IS_EXCLUSIVE) {
                        CallSpritePlayer(state.SPRITE);
                        break;
                    }
                }
            }
            void CallSpritePlayer(string sprite) {
                if (SPRITE_PLAYER.Animation != sprite) {
                    SPRITE_PLAYER.Play(sprite);
                }
            }
        }
    }

    public class State {
        public Func<bool> CONDITION;
        public Action STATE_METHOD;
        public bool IS_EXCLUSIVE;
        public string SPRITE;

        public State(Func<bool> CONDITION, Action STATE_METHOD, bool IS_EXCLUSIVE, string SPRITE) {
            this.CONDITION = CONDITION;
            this.STATE_METHOD = STATE_METHOD;
            this.IS_EXCLUSIVE = IS_EXCLUSIVE;
            this.SPRITE = SPRITE;
        }
    }
}
