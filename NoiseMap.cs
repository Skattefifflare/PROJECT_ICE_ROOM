using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

[Tool]
public partial class NoiseMap : GenSpline {
    private float height, width;
    private int cHeight, cWidth;
    private int patchSize = 5;
    private Vector2 center; //center of the polygon
    private Vector2[] uVS;

    private float noiseScale = 100.0f; //Large number equals bigger noise
    private Random seed = new Random();
    private Image noiseMap;
    private FastNoiseLite noiseGen;

    private float[] mapSize;//max and min vector values
    private float[] NoiseData;

    private List<List<Vector2>> texturePoses;

    public override void _Ready() {
        Update();
        
        noiseGen = new FastNoiseLite();
        //Perlin noise is more smooth while Simplex is more dotted and intense
        noiseGen.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
        noiseGen.Frequency = 1.0f / noiseScale;
        noiseGen.Seed = seed.Next(0, 100000);
        CreateNoiseMap();
        DrawNoiseMap(ImageTexture.CreateFromImage(noiseMap));
        NoiseHandler test = new(NoiseData, cHeight, cWidth, patchSize, 0.5f, new float[]{ mapSize[1], mapSize[3] });
        texturePoses = test.StructureTexturePoses();
        Stopwatch sw = new Stopwatch();
        sw.Start();
        InsideMap(texturePoses, (0, 0), true);
        sw.Stop();
        GD.Print("Time: " + sw.ElapsedMilliseconds);
        PlaceTexture(test);
    }
    public void CreateNoiseMap() {
        mapSize = Bounds();
        width = mapSize[0] - mapSize[1];
        height = mapSize[2] - mapSize[3];
        center = GetSplineCenter();
        //Needs to ceilinged to avoid index out of bounds
        cWidth = (int)Math.Ceiling(width);
        cHeight = (int)Math.Ceiling(height);
        noiseMap = GenerateNoiseMap();
    }
    public float[] Bounds() {
        if (splinePoints.Count == 0) {
            throw new InvalidOperationException("The splinePoints list is empty.");
        }

        //Initialize max and min with the first element
        float[] bounds = new float[4]
        {
        splinePoints[0].X, //max.X
        splinePoints[0].X, //min.X
        splinePoints[0].Y, //max.Y
        splinePoints[0].Y  //min.Y
        };

        foreach (Vector2 vec in splinePoints) {
            if (vec.X > bounds[0]) bounds[0] = vec.X; //max.X
            if (vec.X < bounds[1]) bounds[1] = vec.X; //min.X
            if (vec.Y > bounds[2]) bounds[2] = vec.Y; //max.Y
            if (vec.Y < bounds[3]) bounds[3] = vec.Y; //min.Y
        }

        return bounds;
    }
    private Vector2 GetSplineCenter() {
        Vector2 newCenter = new Vector2();
        newCenter.X = (mapSize[0] + mapSize[1]) / 2;
        newCenter.Y = (mapSize[2] + mapSize[3]) / 2;
        return newCenter;
    }
    //Needs UV-coords so the texture renders correctly on the map
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
    //Converts the noisemap to an image
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
        SetNoiseData(pixelData);
        return img;
    }
    private void DrawNoiseMap(Texture2D texture) {
        splinePoly.UV = uVS;
        splinePoly.Texture = texture;
        AddChild(splinePoly);
    }
    private void DrawRawNoiseMap() { 
    }
    //Loops through the noisemap and saves the float values
    private void SetNoiseData(byte[] data) {
        NoiseData = new float[data.Length];
        for (int i = 0; i < data.Length; i++) {
            NoiseData[i] = (float)data[i] / 255;
        }
    }
    private void InsideMap(List<List<Vector2>> textposes, (int x, int y) start, bool direction) {
        if (start.y < 0 || start.y >= textposes.Count || start.x < 0 || start.x >= textposes[start.y].Count) {
            GD.Print("Out of Bounds: (" + start.x + ", " + start.y + ")");
            return;
        }

        for (int i = 0; i < textposes.Count; i++) {
            for (int j = 0; j < textposes[i].Count; j++) {
                if (Geometry2D.IsPointInPolygon(textposes[i][j], splinePoints.ToArray())) {
                    continue;
                }
                else {
                    textposes[i][j] = new Vector2(-1, -1);
                }
            }
        }

        //if (direction) {
        //    // Forward direction
        //    for (int i = start.x; i < textposes[start.y].Count; i++) {
        //        if (start.x == textposes[start.y].Count) {
        //            //GD.Print("End of Row");
        //            InsideMap(textposes, (0, start.y + 1), !direction); // Move to the next row
        //            return;
        //        }
        //        if (Geometry2D.IsPointInPolygon(textposes[start.y][i], splinePoints.ToArray())) {
        //            //GD.Print("I Have Arrived In A Polygon (Forward)");
        //            InsideMap(textposes, (textposes[start.y].Count - 1, start.y), !direction); //Sends backward
        //            return;
        //        }
        //        else {
        //            //GD.Print("I Have Not Arrived In A Polygon (Forward)");
        //            textposes[start.y][i] = new Vector2(-1, -1); // Mark as invalid
        //        }
        //    }
        //}
        //else {
        //    // Backward direction
        //    for (int i = start.x; i >= 0; i--) {
        //        if(Geometry2D.IsPointInPolygon(textposes[start.y][start.x], splinePoints.ToArray())) {
        //            //GD.Print("test");
        //            InsideMap(textposes, (0, start.y + 1), !direction); // Move to the next row
        //            return;
        //        }
        //        else if (Geometry2D.IsPointInPolygon(textposes[start.y][i], splinePoints.ToArray())) {
        //            //GD.Print("I Have Arrived In A Polygon (Backward)");
        //            if (start.y == textposes.Count - 1) {
        //                CutTexturePoses(textposes);
        //                return;
        //            }
        //            InsideMap(textposes, (0, start.y + 1), !direction); //Sends forward
        //            return;
        //        }
        //        else {
        //            //GD.Print("I Have Not Arrived In A Polygon (Backward)");
        //            textposes[start.y][i] = new Vector2(-1, -1);
        //        }
        //    }
        //}
    }
    private void CutTexturePoses(List<List<Vector2>> textposes) {
        foreach (List<Vector2> poses in textposes) {
            for (int i = 0; i < poses.Count; i++) {
                if (poses[i] == new Vector2(-1, -1)) {
                    poses.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    private void PlaceTexture(NoiseHandler handler) {
        //List<Vector2> positions = handler.textureposes;
        //foreach (Vector2 pos in positions) {
        //    Polygon2D obj = new Polygon2D() {
        //        Polygon = new Vector2[] {
        //        pos,
        //        new Vector2(pos.X + patchSize, pos.Y),
        //        new Vector2(pos.X + patchSize, pos.Y + patchSize),
        //        new Vector2(pos.X, pos.Y + patchSize)
        //        },
        //        Color = new Color(0, 1, 0, 0.3f)
        //    };
        //    AddChild(obj);
        //}
        //foreach (List<Vector2> poses in texturePoses) {
        //    foreach (Vector2 pos in poses) {
        //        Polygon2D obj = new Polygon2D() {
        //            Polygon = new Vector2[] {
        //            pos,
        //            new Vector2(pos.X + patchSize, pos.Y),
        //            new Vector2(pos.X + patchSize, pos.Y + patchSize),
        //            new Vector2(pos.X, pos.Y + patchSize)
        //            },
        //            Color = new Color(0, 1, 0, 1f)
        //        };
        //        AddChild(obj);
        //    }
        //}
    }

}
