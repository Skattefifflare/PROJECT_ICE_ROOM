using Godot;
using Project_Ice_Room.Scriptbin;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class CreatureClass : CharacterBody2D {

    protected int hp = 1;
    protected CollisionShape2D feet;
    protected AnimatedSprite2D sprite_player;
    protected Area2D hitbox;
    protected Weapon weapon_hurt_from;
    protected Area2D dmgbox;

    private Dictionary<string, Action> state_dict;
    protected Action current_state;
    protected bool is_busy = false;

    public override void _Ready() {
        base._Ready();
        feet = (CollisionShape2D)FindChild("feet");
        sprite_player = (AnimatedSprite2D)FindChild("sprite_player");
        hitbox = (Area2D)FindChild("hitbox");
        hitbox.AreaEntered += (entered_node) => {
            if (entered_node.GetParent().GetType() != typeof(Weapon)) return;
            weapon_hurt_from = (Weapon)entered_node.GetParent();
            CallState("take_damage");           
        };
        dmgbox = (Area2D)FindChild("dmgbox");
        dmgbox.Monitorable = false;        
        state_dict = new Dictionary<string, Action>() {
            {"idle", Idle },
            {"attack", Attack },
            {"take_damage", TakeDamage },
        };
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        StateMachine();
        MoveAndSlide();
    }

    protected void AddStates(Dictionary<string, Action> added_states) {
        state_dict = (Dictionary<string, Action>)state_dict.Concat(added_states).ToDictionary(s => s.Key, s => s.Value);
    }
    protected void CallState(string state) {
        if (!state_dict.ContainsKey(state)) GD.Print("state '" + state + "' does not exist in the dictionary.");
        if (current_state == state_dict[state]) return;
        current_state = state_dict[state];
        current_state();
        is_busy = true;
    }
    protected virtual void StateMachine() {
        throw new NotImplementedException("This method must be overridden in a derived class.");
    }


    //_____________STATES_____________
    protected virtual void Idle() {
        is_busy = false; // since idle state can be overruled is_busy will be false.
        sprite_player.Play("idle");
    }
    protected virtual void Attack() {
        dmgbox.Monitorable = true;
        sprite_player.Play("attack");

        Action finish_attack = null;
        finish_attack = () => {
            sprite_player.AnimationFinished -= finish_attack;
            dmgbox.Monitorable = false;
        };
        sprite_player.AnimationFinished += finish_attack;
    }
    protected virtual void TakeDamage() {
        hp -= weapon_hurt_from.dmg;
    }
    protected virtual void Die() {
        sprite_player.Play("die");
    }
}
