#if TOOLS
using Godot;
using System;

[Tool]
public partial class NodeAdder : EditorPlugin
{
	public override void _EnterTree()
	{
        var script = GD.Load<Script>("addons/EnemyAIFolder/AI.cs");
        var texture = GD.Load<Texture2D>("addons/EnemyAIFolder/AI.png");
        AddCustomType("EnemyAI", "CharacterBody2D", script, texture);
    }

	public override void _ExitTree()
	{
		RemoveCustomType("EnemyAI");
    }
}
#endif
