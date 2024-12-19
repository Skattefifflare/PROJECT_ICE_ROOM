using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using static Godot.TextServer;

namespace Project_Ice_Room.Scriptbin;
public partial class Creature : CharacterBody2D {

    [Export]
    protected int HP = 1;
    [Export]
    protected int SPEED = 1;

    protected AnimatedSprite2D SPRITE_PLAYER;
    protected Weapon WHAP;
    protected Area2D FEET;
    protected Area2D HITBOX;
    protected Action CURRENT_ACTION;
    protected Vector2 DIRECTION;
    protected StateHandler SH;

    public override void _Ready() {
        base._Ready();

        SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");
        WHAP = (Weapon)FindChild("weapon");
        FEET = (Area2D)FindChild("feet");
        HITBOX = (Area2D)FindChild("hitbox");
        SH = new(SPRITE_PLAYER);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        MoveAndSlide();
    }


    public virtual void Idle() {
        CallAnimation("idle");
    }
    public virtual void Walk() {
        CallAnimation("walk");
    }
    public virtual void Die() {
        CallAnimation("die");
    }
    public virtual void AttackMoving() {
        CallAnimation("attack_moving");
    }
    public virtual void Attack() {
        CallAnimation("attack");
    }
    public virtual void TakeDamage() {
        CallAnimation("take_damage");
    }
}
public class StateHandler {
    private List<(bool, Action, string, bool)> STATE_LIST;
    private List<(bool, Action, string, bool)> ACTIVE_STATES;
    AnimatedSprite2D SPRITE_PLAYER;
    public StateHandler(AnimatedSprite2D SPRITE_PLAYER) {
        STATE_LIST = new();
        ACTIVE_STATES = new();
        this.SPRITE_PLAYER = SPRITE_PLAYER;
    }
    public void AddStates(List<(bool, Action, string, bool)> states) {
        STATE_LIST.Concat(states);
    }

    public void DecideState() {
        foreach (var state in STATE_LIST) {
            if (!state.Item1) continue;
            if (state.Item4) STATE_LIST.Clear();
            ACTIVE_STATES.Add(state);
            break;
        }
    }
    public void CallState() {
        foreach (var state in ACTIVE_STATES) {
            state.Item2();
        }
        SPRITE_PLAYER.Play(ACTIVE_STATES.Last().Item3);
    }
}