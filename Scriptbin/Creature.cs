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


    protected Weapon WHAP;
    protected Area2D FEET;
    protected AnimatedSprite2D SPRITE_PLAYER;
    protected Area2D HITBOX;
    protected Action CURRENT_ACTION;
    protected Vector2 DIRECTION;
        

    public override void _Ready() {
        base._Ready();

        WHAP = (Weapon)FindChild("weapon");          
        FEET = (Area2D)FindChild("feet");
        SPRITE_PLAYER = (AnimatedSprite2D)FindChild("sprite_player");
        HITBOX = (Area2D)FindChild("hitbox");
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        MoveAndSlide();
    }

    public void StateMachine() {
        DIRECTION = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");


        if (HP <= 0) {
            CallState(Die);
            return;
        }
        else {
            
        }
        

    }
    protected void CallState(Action state) {

    }

    
    public virtual void Idle() {
        SPRITE_PLAYER.Play("idle");
    }
    public virtual void Walk() {
        SPRITE_PLAYER.Play("walk");
    }
    public virtual void Die() {
        SPRITE_PLAYER.Play("die");
    }
    public virtual void Attack() {
        SPRITE_PLAYER.Play("attack");
    }
    public virtual void TakeDamage() {
        SPRITE_PLAYER.Play("take_damage");
    }
}
public struct State {
    Action STATE_ACTION;
    
    


}

