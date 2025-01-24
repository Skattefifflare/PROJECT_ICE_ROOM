using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room.Scriptbin;


internal class State {
    public Func<bool> start_condition;
    public Func<bool> end_condition;

    public Action start_method;
    public Action running_method;
    public Action end_method;

    public string sprite;

    public bool state_started = false;
    public bool is_exclusive;

    public State(Func<bool> start_condition, Func<bool> end_condition, Action start_method, Action running_method, Action end_method, string sprite, bool is_exclusive) {
        this.start_condition = start_condition;
        this.end_condition = end_condition;
        this.start_method = start_method;
        this.running_method = running_method;
        this.end_method = end_method;
        this.sprite = sprite;
        this.is_exclusive = is_exclusive;
    }
}

public class StateHandler {
    private List<State> state_list;
    private List<State> active_states;
    private AnimatedSprite2D sprite_player;

    public StateHandler(AnimatedSprite2D SP) {
        sprite_player = SP;
        state_list = new();
        active_states = new();
    }
    internal void SetStates(List<State> states) {
        state_list = states;
    }


    public void Call() {
        foreach (State state in active_states.Where(s => s.end_condition())) {
            state.end_method();
        }
        active_states = active_states.Where(s => !s.end_condition()).ToList();

        foreach (State state in state_list.Where(s => s.start_condition())) {
            bool exclusive = false;
            if (state.is_exclusive) { 
                active_states.Clear();
                exclusive = true;
            }
            active_states.Add(state);
            state.start_method();
            if (exclusive) break;
        }
       
        foreach (State state in active_states) {
            state.running_method();
        }
        string sprite = (state_list.First(s => active_states.Contains(s)).sprite);
        if (sprite != "current") sprite_player.Play(sprite);
    }
}

//    public class StateHandler {
//    private List<State> state_list;
//    private List<State> active_states;
//    private AnimatedSprite2D sprite_player;

//    public StateHandler(AnimatedSprite2D SP) {
//        sprite_player = SP;
//        state_list = new();
//        active_states = new();
//    }
//    internal void SetStates(List<State> states) {
//        state_list = states;
//    }
    

//    public void CallStateHandler() {
//        var active_states_copy = new List<State>();
//        active_states_copy.AddRange(active_states);

//        // remove states that have ended      
//        foreach (var state in active_states) {
//            if (state.end_condition()) {
//                active_states_copy.Remove(state);
//                GD.Print("removed " + state.sprite);
//                state.end_method();
//                state_list.Find(s => s == state).state_started = false;
//            }
//        }
//        active_states = active_states_copy;

//        // add new states
//        foreach (var state in state_list) {
//            if (active_states.Contains(state)) break; // continue?
//            if (!state.start_condition()) continue;
//            if (state.is_exclusive) active_states.Clear();
//            active_states.Add(state);
//            GD.Print("added " + state.sprite);
//            break; // questionable
//        }

//        // start active states
//        foreach (var state in active_states) {
//            if (state.state_started) continue;
//            state.state_started = true;
//            state.start_method();
//            GD.Print(state.sprite);
//        }
//        sprite_player.Play(active_states.First().sprite);
//    }
//}


