using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class LvlGenNode : Node2D
{
	private NoiseMap spline;
	private Vector2 BASE_SIZE;
	[Export]
	public Vector2 _BASE_SIZE {
		get {
			return BASE_SIZE;
		}
		set {
			BASE_SIZE = value;
		}
	}

	public override void _Ready() {
		base._Ready();

		NewLvlShapeObject lso = new NewLvlShapeObject(700, 700, 5);
	   
		var generated_shape = lso.GetShape();

        spline = new NoiseMap();
		spline.Begin(generated_shape);
		
		OS.DelayMsec(5000);
        AddChild(spline.splinePoly);

        NavigationRegion2D navRegion = new NavigationRegion2D();
		NavigationPolygon navpoly = new NavigationPolygon();
		navpoly.AddOutline(spline.splinePoly.Polygon);
        navRegion.NavigationPolygon = navpoly;
		navRegion.BakeNavigationPolygon();
		AddChild(navRegion);

        foreach (var p in lso.GetSubShapes()) {
			//AddChild(p);
		}

		//foreach (var p in polygon.Polygon) {
		//	GD.Print(p);
		//}

	}
    public override void _Draw() {
        Random rand = new Random();
        for (int i = 3; i >= 0; i--) {
            
            foreach (var p in spline.noiseLayers[i].poses) {
				bool flipX = GD.Randf() > 0.5f;
                Vector2 scale = flipX ? new Vector2(-1, 1) : new Vector2(1, 1);

                //DrawTexture(spline.noiseLayers[i].texture, p);
				DrawTextureRect(spline.noiseLayers[i].texture, new Rect2(p, spline.noiseLayers[i].texture.GetSize() * scale), false);
            }
		}
		foreach (var p in spline.noiseLayers[4].poses) {
            bool flipX = GD.Randf() > 0.5f;
			int ranTexture = rand.Next(0, 4);
            Vector2 scale = flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
			//DrawTexture(spline.noiseLayers[i].texture, p);
			DrawTextureRect(spline.noiseLayers[4].textures[ranTexture], new Rect2(p, spline.noiseLayers[4].textures[ranTexture].GetSize() * scale), false);
        }
        base._Draw();
    }

    public override void _ExitTree() {
		base._ExitTree();

		foreach (var c in GetChildren()) {
			RemoveChild(c);
			c.QueueFree();
		}
		
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}
}
