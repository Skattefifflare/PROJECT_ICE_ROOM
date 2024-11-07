#if TOOLS
using Godot;
using System;

[Tool]
public partial class lvlgen : EditorPlugin
{
	public override void _EnterTree()
	{
        var script = GD.Load<Script>("res://addons/LEVEL_GEN/lvlgen_script.cs");
        var icon = GD.Load<Texture2D>("res://addons/LEVEL_GEN/lvlgen_logo.png");
        AddCustomType("LvlGenNode", "Node2D", script, icon);
    }

	public override void _ExitTree()
	{
        RemoveCustomType("LvlGen");
    }
}
#endif
