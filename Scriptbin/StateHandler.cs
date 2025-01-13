using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin;
public class StateHandler {
    private List<State> STATE_LIST;
    private List<State> ACTIVE_STATES;
    private AnimatedSprite2D SPRITE_PLAYER;

    public StateHandler(AnimatedSprite2D SP) {
        SPRITE_PLAYER = SP;
        STATE_LIST = new();
        ACTIVE_STATES = new();
    }
    internal void SetStates(List<State> states) {
        STATE_LIST = states;
    }

    public void CallStateHandler() {
        var active_states_copy = new List<State>();
        active_states_copy.AddRange(ACTIVE_STATES);
        foreach (var state in ACTIVE_STATES) {
            if (state.END_CONDITION()) {
                active_states_copy.Remove(state);
            }
        }
        ACTIVE_STATES = active_states_copy;
        foreach (var state in STATE_LIST) {
            if (ACTIVE_STATES.Contains(state)) continue;
            if (!state.CONDITION()) continue;
            if (state.IS_EXCLUSIVE) ACTIVE_STATES.Clear();
            ACTIVE_STATES.Add(state);
            break;
        }
        foreach (var state in STATE_LIST) {
            if (ACTIVE_STATES.Contains(state)) {
                SPRITE_PLAYER.Play(state.SPRITE);
                break;
            }
        }
        foreach (var state in ACTIVE_STATES) {
            state.STATE_METHOD();
            GD.Print(state.SPRITE);
        }
    }
}


internal class State {
    public Func<bool> CONDITION;
    public Func<bool> END_CONDITION;
    public Action STATE_METHOD;
    public bool IS_EXCLUSIVE;
    public string SPRITE;

    public State(Func<bool> CONDITION, Func<bool> END_CONDITION, Action STATE_METHOD, bool IS_EXCLUSIVE, string SPRITE) {
        this.CONDITION = CONDITION;
        this.END_CONDITION = END_CONDITION;
        this.STATE_METHOD = STATE_METHOD;
        this.IS_EXCLUSIVE = IS_EXCLUSIVE;
        this.SPRITE = SPRITE;
    }
}



