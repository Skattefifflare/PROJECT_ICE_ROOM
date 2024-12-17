using Godot;
using System;

[Tool]
public partial class NoiseMap : GenSpline {
    private float height, width;
    private int cHeight, cWidth;
    private Vector2 center; //center of the polygon
    private Vector2[] uVS;

    private float noiseScale = 100.0f; //Large number equals bigger noise
    private Random seed = new Random();
    private Image noiseMap;
    private FastNoiseLite noiseGen;

    private float[] mapSize;//max and min vector values

    public override void _Ready() {
        Update();
        
        noiseGen = new FastNoiseLite();
        //Perlin noise is more smooth while Simplex is more dotted and intense
        noiseGen.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        noiseGen.Frequency = 1.0f / noiseScale;
        noiseGen.Seed = seed.Next(0, 100000);
        CreateNoiseMap();
        DrawNoiseMap(ImageTexture.CreateFromImage(noiseMap));
        NoiseHandler test = new(GetNoiseData(), cHeight, cWidth, 10, 0.5f);
    }
    public void CreateNoiseMap() {
        mapSize = GetMaxAndMin();
        width = mapSize[0] - mapSize[1];
        height = mapSize[2] - mapSize[3];
        center = GetSplineCenter();
        cWidth = (int)Math.Ceiling(width);
        cHeight = (int)Math.Ceiling(height);
        noiseMap = GenerateNoiseMap();
    }
    public float[] GetMaxAndMin() {
        if (splinePoints.Count == 0) {
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

        foreach (Vector2 vec in splinePoints) {
            if (vec.X > maxAndMin[0]) maxAndMin[0] = vec.X; // max.X
            if (vec.X < maxAndMin[1]) maxAndMin[1] = vec.X; // min.X
            if (vec.Y > maxAndMin[2]) maxAndMin[2] = vec.Y; // max.Y
            if (vec.Y < maxAndMin[3]) maxAndMin[3] = vec.Y; // min.Y
        }

        return maxAndMin;
    }
    private Vector2 GetSplineCenter() {
        Vector2 newCenter = new Vector2();
        newCenter.X = (mapSize[0] + mapSize[1]) / 2;
        newCenter.Y = (mapSize[2] + mapSize[3]) / 2;
        return newCenter;
    }
    private Vector2[] GenerateUV(int rHeight, int rWidth) {
        Vector2[] tempPoints = splinePoints.ToArray();
        Vector2[] uvs = new Vector2[splinePoints.Count];
        Rect2 boundingBox = new Rect2(
            new Vector2(mapSize[1], mapSize[3]),
            new Vector2(mapSize[0] - mapSize[1], mapSize[2] - mapSize[3])
        );

        for (int i = 0; i < tempPoints.Length; i++) {
            uvs[i] = new Vector2(
                tempPoints[i].X - boundingBox.Position.X,
                tempPoints[i].Y - boundingBox.Position.Y
            );
        }
        return uvs;
    }
    private Image GenerateNoiseMap() {
        uVS = GenerateUV(cHeight, cWidth);

        Image img = new Image();
        byte[] pixelData = new byte[cWidth * cHeight];

        for (int x = 0; x < cWidth; x++) {
            for (int y = 0; y < cHeight; y++) {
                float noiseValue = noiseGen.GetNoise2D(x, y);
                byte grayScaleValue = (byte)Mathf.Lerp(0, 255, (noiseValue + 1) / 2);
                pixelData[y * cWidth + x] = grayScaleValue;
            }
        }
        img.SetData(cWidth, cHeight, false, Image.Format.L8, pixelData);
        return img;
    }
    private void DrawNoiseMap(Texture2D texture) {
        splinePoly.UV = uVS;
        splinePoly.Texture = texture;
        AddChild(splinePoly);
    }
    private float[] GetNoiseData() {
        float[] noiseValues = new float[cWidth * cHeight];
        for (int x = 0; x < cWidth; x++) {
            for (int y = 0; y < cHeight; y++) {
                noiseValues[y * cWidth + x] = noiseGen.GetNoise2D(x, y);
            }
        }
        return noiseValues;
    }
}
