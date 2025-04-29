using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*TODO:
 * Scale down polygon when checking if textures are inside
 * Add clouds around the map
 * Merging
 * 
 * Notes: Order
 * trees
 * flower
 * grass
 * rocks
 */

[Tool]
public partial class NoiseMap : Node2D {
    private float height, width;
    private int cHeight, cWidth;
    private int patchSize = 10;

    private float noiseScale = 100.0f; //Large number equals bigger noise
    private Random seed = new Random();

    public GenSpline map;
    private List<Vector2> splinePoints;
    public Polygon2D splinePoly;

    private float[] mapSize;//max and min vector values
    private float[] NoiseData;

    private List<Vector2> texturePoses;
    public List<Polygon2D> texturePolys;

    public List<NoiseLayer> noiseLayers = new();
    private List<Texture2D> textures = new();

    public struct NoiseLayer {
        public FastNoiseLite noise;
        public int patchSize;
        public List<float> noiseData;
        public float threshold;
        public List<Vector2> poses;
        public Texture2D texture;
        public List<Texture2D> textures;
        public NoiseLayer(float scale, FastNoiseLite.NoiseTypeEnum noiseType, int seed, int patchSize, float threshold, Texture2D texture) {
            noise = new FastNoiseLite();
            noise.NoiseType = noiseType;
            noise.Frequency = 1.0f / scale;
            noise.Seed = seed;
            this.patchSize = patchSize;
            this.threshold = threshold;
            noiseData = new();
            poses = new();
            this.texture = texture;
            textures = new List<Texture2D>();
        }
        public NoiseLayer(float scale, FastNoiseLite.NoiseTypeEnum noiseType, int seed, int patchSize, float threshold, List<Texture2D> textures) {
            noise = new FastNoiseLite();
            noise.NoiseType = noiseType;
            noise.Frequency = 1.0f / scale;
            noise.Seed = seed;
            this.patchSize = patchSize;
            this.threshold = threshold;
            noiseData = new();
            poses = new();
            texture = new();
            this.textures = textures;
        }
    }

    public void Begin(Vector2[] shape) {
        Spline(shape);
        NoiseStart();
        for (int i = 0; i < noiseLayers.Count; i++) {
            noiseLayers[i] = TextureHandling(noiseLayers[i], i);
        }
        Finish();
    }
    private void Spline(Vector2[] shape) {
        map = new GenSpline();
        map.Update(shape);
        splinePoints = map.splinePoints;
        splinePoly = map.splinePoly;
    }
    //Perlin noise is more smooth while Simplex is more dotted and intense
    private void NoiseStart() {
        LoadTextures();
        noiseLayers.Add(new NoiseLayer(noiseScale, FastNoiseLite.NoiseTypeEnum.Simplex, seed.Next(0, 100000), 44, 0.3f, textures[0]));
        noiseLayers.Add(new NoiseLayer(noiseScale, FastNoiseLite.NoiseTypeEnum.Value, seed.Next(0, 100000), 16, 0.3f, textures[1]));
        noiseLayers.Add(new NoiseLayer(noiseScale, FastNoiseLite.NoiseTypeEnum.SimplexSmooth, seed.Next(0, 100000), 20, 0.4f, textures[2]));
        noiseLayers.Add(new NoiseLayer(noiseScale, FastNoiseLite.NoiseTypeEnum.Perlin, seed.Next(0, 100000), 18, 0.35f, textures[3]));

        for(int i = 0; i < noiseLayers.Count; i++) {
            noiseLayers[i] = CreateNoiseMap(noiseLayers[i]);
        }
    }
    private void LoadTextures() {
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/tree.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/flower.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/grass.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/rocks.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/cloud1.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/cloud2.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/cloud3.png"));
        textures.Add(GD.Load<Texture2D>("res://LVL_GENERATOR/assets/cloud4.png"));
    }
    private NoiseLayer TextureHandling(NoiseLayer noise, int layer) {
        NoiseHandler handler = new();
        texturePolys = new List<Polygon2D>();
        handler.Constructer(noise.noiseData.ToArray(), cHeight, cWidth, noise.patchSize, noise.threshold, new float[] { mapSize[1], mapSize[3] });
        noise.poses = handler.textureposes;
        if(layer < 2) {
            InsideMap(ref noise.poses, 0.95f, true);
            noise.poses = handler.ListProximity(noise.poses, 3, false);
            InsideMap(ref noise.poses, 0.95f, true);
        }
        else if (layer == 4) {
            InsideMap(ref noise.poses, 1f, false);
            noise.poses = NaturalEnvironment(noise.poses, 2f);
            InsideMap(ref noise.poses, 1f, false);
        }
        else {
            InsideMap(ref noise.poses, 0.95f, true);
            noise.poses = NaturalEnvironment(noise.poses, 1f);
            InsideMap(ref noise.poses, 0.95f, true);
        }
        return noise;
    }
    public NoiseLayer CreateNoiseMap(NoiseLayer noise) {
        mapSize = Bounds();
        width = mapSize[0] - mapSize[1];
        height = mapSize[2] - mapSize[3];
        //Needs to ceilinged to avoid index out of bounds
        cWidth = (int)Math.Ceiling(width);
        cHeight = (int)Math.Ceiling(height);
        noise.noiseData = GenerateNoiseMap(ref noise.noise).ToList();
        return noise;
    }
    public float[] Bounds() {
        if (splinePoints.Count == 0) {
            throw new InvalidOperationException("The splinePoints list is empty.");
        }

        //Initialize max and min with the first element
        float[] bounds = new float[4]
        {
        splinePoints[0].X,
        splinePoints[0].X, 
        splinePoints[0].Y, 
        splinePoints[0].Y  
        };

        foreach (Vector2 vec in splinePoints) {
            if (vec.X > bounds[0]) bounds[0] = vec.X; //max.X
            if (vec.X < bounds[1]) bounds[1] = vec.X; //min.X
            if (vec.Y > bounds[2]) bounds[2] = vec.Y; //max.Y
            if (vec.Y < bounds[3]) bounds[3] = vec.Y; //min.Y
        }

        return bounds;
    }
    //Needs UV-coords so the texture renders correctly on the map
    //private Vector2[] GenerateUV(int rHeight, int rWidth) {
    //    Vector2[] tempPoints = splinePoints.ToArray();
    //    Vector2[] uvs = new Vector2[splinePoints.Count];
    //    Rect2 boundingBox = new Rect2(
    //        new Vector2(mapSize[1], mapSize[3]),
    //        new Vector2(mapSize[0] - mapSize[1], mapSize[2] - mapSize[3])
    //    );

    //    for (int i = 0; i < tempPoints.Length; i++) {
    //        uvs[i] = new Vector2(
    //            tempPoints[i].X - boundingBox.Position.X,
    //            tempPoints[i].Y - boundingBox.Position.Y
    //        );
    //    }
    //    return uvs;
    //}
    //Converts the noisemap to an image
    private float[] GenerateNoiseMap(ref FastNoiseLite noise) {
        //uVS = GenerateUV(cHeight, cWidth);

        byte[] pixelData = new byte[cWidth * cHeight];

        for (int x = 0; x < cWidth; x++) {
            for (int y = 0; y < cHeight; y++) {
                float noiseValue = noise.GetNoise2D(x, y);
                byte grayScaleValue = (byte)Mathf.Lerp(0, 255, (noiseValue + 1) / 2);
                pixelData[y * cWidth + x] = grayScaleValue;
            }
        }
        return SetNoiseData(pixelData);
    }
    //private void DrawNoiseMap(Texture2D texture) {
    //    splinePoly.UV = uVS;
    //    splinePoly.Texture = texture;
    //}
    //Loops through the noisemap and saves the float values
    private float[] SetNoiseData(byte[] data) {
        NoiseData = new float[data.Length];
        for (int i = 0; i < data.Length; i++) {
            NoiseData[i] = (float)data[i] / 255;
        }
        return NoiseData;
    }
    private void InsideMap(ref List<Vector2> textposes, float scale, bool inside) {
        textposes = ScalePolygon(textposes, scale);
        if (inside) {
            for (int i = 0; i < textposes.Count; i++) {
                if (Geometry2D.IsPointInPolygon(textposes[i], splinePoints.ToArray())) {
                    continue;
                }
                else {
                    textposes[i] = new Vector2();
                }
            }
        }
        else {
            for (int i = 0; i < textposes.Count; i++) {
                if (Geometry2D.IsPointInPolygon(textposes[i], splinePoints.ToArray())) {
                    textposes[i] = new Vector2();
                }
                else {
                    continue;
                }
            }
        }
        CutTexturePoses(ref textposes);
    }
    private List<Vector2> ScalePolygon(List<Vector2> polygon, float scale) {
        if (polygon == null || polygon.Count == 0) return new List<Vector2>();

        //Get center
        Vector2 centroid = Vector2.Zero;
        foreach (var point in polygon)
            centroid += point;
        centroid /= polygon.Count;

        //Scale polygon from center
        var scaledPolygon = new List<Vector2>();
        foreach (var point in polygon) {
            Vector2 direction = point - centroid;
            Vector2 scaledPoint = centroid + direction * scale;
            scaledPolygon.Add(scaledPoint);
        }

        return scaledPolygon;
    }

    private void CutTexturePoses(ref List<Vector2> textposes) {
        for (int i = 0; i < textposes.Count; i++) {
            if (textposes[i] == new Vector2()) {
                textposes.RemoveAt(i);
                i--;
            }
        }
    }
    private List<Vector2> NaturalEnvironment(List<Vector2> poses, float scale) {
        for (int i = 0; i < poses.Count; i++) {
            poses[i] = new Vector2(poses[i].X + (GD.Randf() - 1) * 10 * scale, poses[i].Y + (GD.Randf() - 1) * 10* scale);
        }

        return poses;
    }

    private void Finish() {
        List<Texture2D> cloudTextures = new List<Texture2D>() { 
            textures[4],
            textures[5],
            textures[6],
            textures[7]
        };
        noiseLayers.Add(new NoiseLayer(noiseScale, FastNoiseLite.NoiseTypeEnum.Cellular, seed.Next(0, 100000), 60, 0.2f, cloudTextures));
        noiseLayers[4] = CreateNoiseMap(noiseLayers[4]);
        noiseLayers[4] = TextureHandling(noiseLayers[4], 4);
    }
}
