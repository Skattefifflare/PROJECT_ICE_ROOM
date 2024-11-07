using Godot;
using System;

[Tool]
public partial class LvlGenNode : Node2D
{
    private Vector2 SIZE;
    [Export]
    public Vector2 _SIZE {
        get {
            return SIZE;
        }
        set {          
            SIZE = value;           
        }
    }

    public override void _Ready() {
        base._Ready();

        
    }

}
