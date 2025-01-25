using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (active_states.Contains(state)) continue;
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





public class State2 {
    private (bool, State2)[] pointers;
    private bool has_binded = false;

    Action start_method;
    Action running_method;
    Action end_method;
    private bool has_started = false;

    public State2(Action s, Action r, Action e) {
        start_method = s;
        running_method = r;
        end_method = e;
    }

    public void BindStates((bool, State2)[] _pointers) {
        if (has_binded) {
            GD.Print("already bound states");
            return;
        }
        pointers = _pointers;
        has_binded = true;
    }


    public void Call(State2 current_state) {
        if (!has_started) {
            start_method();
            has_started = true;
        }
        foreach ((bool, State2) pointer in pointers) {
            if (pointer.Item1) {
                end_method();
                current_state = pointer.Item2;
                current_state.start_method();
                break;
            }
        }
        running_method();
    }
}
