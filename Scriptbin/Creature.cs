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

        SH.AddStates(new List<(Func<bool>, Action, string, bool)>() {
            (() => HP <= 0, Die, "die", true),
            (() => Input.IsActionJustPressed("attack") && DIRECTION != Vector2.Zero, AttackMoving, "walk", false)

        });
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        
        
        MoveAndSlide();
    }


    public virtual void Idle() {
        
    }
    public virtual void Walk() {
        Vector2 pos = Position + (DIRECTION * SPEED);
        Position = pos;
    }
    public virtual void Die() {
        SPRITE_PLAYER.AnimationFinished += () => QueueFree();
    }
    public virtual void AttackMoving() {
        WHAP.MakeDangerous();
        SPRITE_PLAYER.AnimationFinished += WHAP.MakeHarmLess;
    }
    public virtual void Attack() {
        
    }
    public virtual void TakeDamage() {
        
    }
}
public class StateHandler {
    private List<(Func<bool>, Action, string, bool)> STATE_LIST;
    private List<(Func<bool>, Action, string, bool)> ACTIVE_STATES;
    AnimatedSprite2D SPRITE_PLAYER;
    public StateHandler(AnimatedSprite2D SPRITE_PLAYER) {
        STATE_LIST = new();
        ACTIVE_STATES = new();
        this.SPRITE_PLAYER = SPRITE_PLAYER;
    }
    public void AddStates(List<(Func<bool>, Action, string, bool)> states) {
        STATE_LIST.Concat(states);
    }
    public void DecideState() {
        foreach (var state in STATE_LIST) {
            if (!state.Item1()) continue;
            if (state.Item4) STATE_LIST.Clear();
            ACTIVE_STATES.Add(state);
            break;
        }
    }
    public void CallState() {
        SPRITE_PLAYER.Play(ACTIVE_STATES.Last().Item3);
        foreach (var state in ACTIVE_STATES) {
            state.Item2();
        }      
    }
}

public class State {
    public Func<bool> CONDITION;
    public Action STATE_METHOD;
    public bool IS_EXCLUSIVE;

    public State(Func<bool> CONDITION, Action STATE_METHOD, bool IS_EXCLUSIVE) {
        this.CONDITION = CONDITION;
        this.STATE_METHOD = STATE_METHOD;
        this.IS_EXCLUSIVE = IS_EXCLUSIVE;
    }

    public  virtual string GetAnimation();

}