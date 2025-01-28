using Godot;
using System;


namespace Project_Ice_Room.Scriptbin;
public class State {
    private (Func<bool>, State)[] pointers;
    private bool has_binded = false;

    Action start_method;
    Action running_method;
    Action end_method;
    private bool has_started = false;

    public State(Action s = null, Action r = null, Action e = null) {
        start_method = s ?? (() => { });
        running_method = r ?? (() => { });
        end_method = e ?? (() => { });
    }

    public void BindStates((Func<bool>, State)[] _pointers) {
        if (has_binded) {
            GD.Print("already bound states");
            return;
        }
        pointers = _pointers;
        has_binded = true;
    }

    public void Call(ref State current_state) {
        if (!has_started) {
            start_method();
            has_started = true;
        }
        if (pointers == null) return;
        foreach ((Func<bool>, State) pointer in pointers) {
            if (pointer.Item1()) {
                end_method();
                current_state = pointer.Item2;
                current_state.start_method();
                has_started = false;
                break;
            }
        }
        running_method();
    }
}
