using Godot;
using System;

[Tool]
public partial class Weapon : Sprite2D
{
    [Export]
    public int dmg = 1;

    Area2D dmg_box;
    public bool is_dangerous = false;
  
    Node2D left_hand_pos;    
    Node2D right_hand_pos;
    Bone2D left_hand;
    Bone2D right_hand;

    Vector2 mouse_pos;


    Vector2 prev_pos;
    float prev_rot;

    public override void _Ready() {
        base._Ready();
        try {
            left_hand_pos = (Node2D)FindChild("left_pos");
            right_hand_pos = (Node2D)FindChild("right_pos");
            GD.Print(left_hand_pos.GlobalPosition);
        }
        catch {
            GD.Print("left_hand_pos");
        }

        try {

            left_hand = (Bone2D)GetNode("%lh");
            right_hand = (Bone2D)GetNode("%rh");
            GD.Print(left_hand.GlobalPosition);
        }
        catch {
            GD.Print("left_hand");
        }
      

        prev_pos = this.GlobalPosition;
        prev_rot = this.Rotation;

        dmg_box = (Area2D)FindChild("dmg_box");
        is_dangerous = false;
    } 
    public void MakeDangerous() {
        is_dangerous = true;
    }
    public void MakeHarmless() {
        is_dangerous = false;
    }
    public override void _Process(double delta) {
        base._Process(delta);
        Update();
        prev_pos = this.GlobalPosition;
        prev_rot = this.Rotation;
    }

    private void Update() {
        mouse_pos = GetGlobalMousePosition();

        if (GlobalPosition != prev_pos || Rotation != prev_rot) {
            if ((left_hand_pos.GlobalPosition-left_hand.GlobalPosition).Length() > 1f) {
                SetBack();
            }
            if ((right_hand_pos.GlobalPosition - right_hand.GlobalPosition).Length() > 1f) {
                SetBack();
            }
        }
    }
    private void SetBack() {
        this.GlobalPosition = prev_pos;
        this.Rotation = prev_rot;
    }
}
