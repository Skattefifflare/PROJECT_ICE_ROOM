using Godot;
using System;
using System.Drawing;

[Tool]
public partial class NoiseMap : GenSpline
{
	private float height;
	private float width;
    private Vector2 center;

	private float noiseScale = 50.0f;
	private int noiseSeed = 21;
    private Image noiseMap;

    private float[] mapSize;

    private FastNoiseLite noiseGen;
	public override void _Ready()
	{
		Update();

		noiseGen = new FastNoiseLite();
		noiseGen.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		noiseGen.Frequency = 1.0f/noiseScale;
		noiseGen.Seed = noiseSeed;
		CreateNoiseMap();
	}
	public void CreateNoiseMap()
	{
		mapSize = GetMaxAndMin();
		width = mapSize[0] - mapSize[1];
		height = mapSize[2] - mapSize[3];
        center = GetSplineCenter();

		noiseMap = GenerateNoiseMap();
    }
    //Up for rewriting
    public float[] GetMaxAndMin()
    {
        if (splinePoints.Count == 0)
        {
            throw new InvalidOperationException("The splinePoints list is empty.");
        }

        // Initialize max and min with the first element
        float[] maxAndMin = new float[4]
        {
        splinePoints[0].X, // max.X
        splinePoints[0].X, // min.X
        splinePoints[0].Y, // max.Y
        splinePoints[0].Y  // min.Y
        };

        foreach (Vector2 vec in splinePoints)
        {
            if (vec.X > maxAndMin[0]) maxAndMin[0] = vec.X; // max.X
            if (vec.X < maxAndMin[1]) maxAndMin[1] = vec.X; // min.X
            if (vec.Y > maxAndMin[2]) maxAndMin[2] = vec.Y; // max.Y
            if (vec.Y < maxAndMin[3]) maxAndMin[3] = vec.Y; // min.Y
        }

        return maxAndMin;
    }
    private Vector2 GetSplineCenter()
    {
        Vector2 newCenter = new Vector2();
        newCenter.X = (mapSize[0] + mapSize[1]) / 2;
        newCenter.Y = (mapSize[2] + mapSize[3]) / 2;
        return newCenter;
    }
    private Image GenerateNoiseMap()
	{
		int rWidth = (int)Math.Ceiling(width);
		int rHeight = (int)Math.Ceiling(height);

        Image img = new Image();
        byte[] pixelData = new byte[rWidth * rHeight];
        
		for (int x = 0; x < rWidth; x++)
		{
			for (int y = 0; y < rHeight; y++)
			{
				float noiseValue = noiseGen.GetNoise2D(x, y);
				byte grayScaleValue = (byte)Mathf.Lerp(0, 255, (noiseValue + 1) / 2);
				pixelData[y * rWidth + x] = grayScaleValue;
			}
		}
		img.SetData(rWidth, rHeight, false, Image.Format.L8, pixelData);
		return img;
    }
	private void DrawNoiseMap(Texture2D texture)
	{
        Sprite2D sprite = new Sprite2D() { Texture = texture };
        sprite.Position = new Vector2(1000, 0);
        AddChild(sprite);
        splinePoly.Texture = texture;
        splinePoly.TextureRepeat = TextureRepeatEnum.Enabled;
		AddChild(splinePoly);
	}
    public override void _Draw()
    {
        DrawNoiseMap(ImageTexture.CreateFromImage(noiseMap));
    }
}
