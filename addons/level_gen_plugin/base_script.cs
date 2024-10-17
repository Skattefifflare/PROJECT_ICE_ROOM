using Godot;
using System;

/// <summary>
///  this is the actual script of our custom node.
/// </summary>
public partial class base_script : Node2D {

    // this is how to add custom parameters for the Node, works with regular nodes as well
    [ExportGroup("params")]
    [Export]
    public int MyInt { get; set; } = 0;


    [ExportGroup("params")]
    [Export]
    public bool MyBool { get; set; } = false;


    [Export(PropertyHint.Range, "-10,20,")]
    public float MyFloat { get; set; }



    public override void _Process(double delta) {
        base._Process(delta);


    }
}
