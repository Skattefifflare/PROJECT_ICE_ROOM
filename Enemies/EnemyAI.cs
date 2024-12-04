using Godot;
using System;
using System.Threading;

public partial class EnemyAI : Node
{
    public bool sees_player = false;

    Area2D field_of_view;
    int view_distance = 100;

    public EnemyAI(Node creature) {
        field_of_view = new Area2D();
        field_of_view.AddChild(new CollisionShape2D() {
            Shape = new CircleShape2D() {
                Radius = view_distance
            }
        });
        field_of_view.AreaEntered += (object_entered) => sees_player = true;
        
        creature.AddChild(field_of_view);
    }   
}
