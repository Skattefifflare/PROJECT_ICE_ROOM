using Godot;
using System;
using System.Linq;

[Tool]
internal partial class NoiseHandler : Node2D {
    float[,] noiseValues;
    int height, width;
    int patchSize;
    float threshold;
    Vector2 minCoords;

    public NoiseHandler(float[] noiseValues, int height, int width, int patchSize, float threshold, float[] minCoords) {
        this.height = height;
        this.width = width;
        this.noiseValues = ConvertNoiseData(noiseValues);
        this.patchSize = patchSize;
        this.threshold = threshold;
        this.minCoords = new Vector2(-minCoords[0], -minCoords[1]);
        NoiseEvaluation();
    }
    private float[,] ConvertNoiseData(float[] noiseData) {
        float[,] newValues = new float[width, height];
        for (int i = 0; i < noiseData.Length; i++) {
            try {
                int row = i % width;
                int col = i / width;

                newValues[row, col] = noiseData[i];
            }
            catch (Exception ex) {
                GD.Print(ex + "Error at index: " + i);
            }
        }
        return newValues;
    }
    private void NoiseEvaluation() {
        for (int y = 0; y < height; y += patchSize) {
            for (int x = 0; x < width; x += patchSize) {
                try {
                    float value = GetNoisePatchMean(x, y);

                    if (value > threshold) {
                        Vector2 topLeftPosition = new Vector2(x, y);
                        Vector2 texturePosition = topLeftPosition - minCoords;
                        PlaceTexture(texturePosition);
                        GD.Print($"Texture at ({texturePosition}): Mean={value}");
                    }
                }
                catch (Exception ex) {
                    GD.Print(ex + "Error at index: " + x, y);
                }
            }
        }
    }

    private float GetNoisePatchMean(int startX, int startY) {
        float total = 0;
        int count = 0;

        for(int y = startY; y < startY + patchSize && y < height; y++) {
            for (int x = startX; x < startX + patchSize && x < width; x++) { 
                total += noiseValues[x, y];
                count++;
            }
        }
        return total / count;
    }

    private void PlaceTexture(Vector2 pos) {
        Polygon2D obj = new Polygon2D() { 
            Polygon = new Vector2[] {
            pos, 
            new Vector2(pos.X + patchSize*10, pos.Y),
            new Vector2(pos.X + patchSize*10, pos.Y + patchSize*10),
            new Vector2(pos.X, pos.Y + patchSize*10)
            },
            Color = new Color(0, 1, 0, 1f)
        };
        AddChild(obj);
        GD.Print("test");
    }
}

