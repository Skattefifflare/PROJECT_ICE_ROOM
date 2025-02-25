using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ice_Room;
public partial class PlayerWeapon : Weapon {


    Marker2D left_marker;
    Marker2D right_marker;
    Bone2D left_bone;
    Bone2D right_bone;
    Node2D left_hand;
    Node2D right_hand;
    Node2D parent;
    AnimationPlayer animation;
    Node2D root;

    float arm_len = 12f;


    public override void _Ready() {
        base._Ready();
        left_marker = (Marker2D)FindChild("left_marker");
        right_marker = (Marker2D)FindChild("right_marker");
        left_bone = (Bone2D)GetNode("%upper_left");
        right_bone = (Bone2D)GetNode("%upper_right");
        left_hand = (Node2D)GetNode("%left_hand");
        right_hand = (Node2D)GetNode("%right_hand");
        parent = (Node2D)GetParent();
        animation = (AnimationPlayer)FindChild("animation");
        root = (Node2D)parent.GetParent();
        GD.Print(GetTreeStringPretty());
        //UpdateOffset();
    }
    public override void _Process(double delta) {
        base._Process(delta);
        Rotate();
        //if (Input.IsActionJustPressed("left_click")) {
        //    animation.Play("attack");
        //}
        //CorrectPosition();
        //Flip();
    }
    private void CorrectPosition() {
        if (animation.IsPlaying()) return;
        float left_dist = (left_marker.GlobalPosition - left_bone.GlobalPosition).Length();
        float right_dist = (right_marker.GlobalPosition - right_bone.GlobalPosition).Length();

        bool far_left = left_dist > 12f ? true : false;
        bool far_right = right_dist > 12f ? true : false;

        if (far_left && far_right) {
            Vector2 hand_diff = right_hand.GlobalPosition - left_hand.GlobalPosition;

            float hand_angle = Mathf.Atan2(hand_diff.Y, hand_diff.X);
            this.Rotation += hand_angle;

            Vector2 left_hand_dist = left_marker.GlobalPosition - left_hand.GlobalPosition;
            Vector2 right_hand_dist = right_marker.GlobalPosition - right_hand.GlobalPosition;
            this.GlobalPosition -= left_hand_dist.Length() > right_hand_dist.Length() ? left_hand_dist : right_hand_dist;
        }
        else if (far_left) {

        }
        else if (far_right) {

        }
    }
    private void Flip() {
        if (root.Scale.X == 1) {
            if (parent.RotationDegrees > 90 || parent.RotationDegrees < -90) {
                root.Scale = root.Scale * new Vector2(-1, 1);
            }
        }
    }
    private void Rotate() {
        if (root.Scale.X == 1)
            parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - parent.GlobalPosition.Y, GetGlobalMousePosition().X - parent.GlobalPosition.X);
        else
            parent.Rotation = Mathf.Atan2(GetGlobalMousePosition().Y - parent.GlobalPosition.Y, GetGlobalMousePosition().X - parent.GlobalPosition.X);
    }
    private void UpdateOffset() {
        //this.Offset = new Vector2((left_marker.Position.X + right_marker.Position.X) / 2, 0);
    }
}
