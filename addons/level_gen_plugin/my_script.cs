#if TOOLS
using Godot;
using System;

/// <summary>
///  this is the script responsible for creating the custom node.
/// </summary>
[Tool]
public partial class my_script : EditorPlugin
{
    
    public override void _EnterTree()
	{       
        // Add the new type with a name, a parent type, a script and an icon.
        var script = GD.Load<Script>("addons/level_gen_plugin/base_script.cs"); // make relative
        var texture = GD.Load<Texture2D>("addons/level_gen_plugin/icon.png");   // make relative
        AddCustomType("MyNode", "Node2D", script, texture);
    }

	public override void _ExitTree()
	{
        // Clean-up of the plugin goes here.
        // Always remember to remove it from the engine when deactivated.
        RemoveCustomType("MyButton");
    }
}
#endif
