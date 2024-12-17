using Godot;
using System;
using System.Linq;

[Tool]
internal partial class NoiseHandler : Node2D {
    float[,] noiseValues;
    int height, width;
    int patchSize;
    float threshold;

    public NoiseHandler(float[] noiseValues, int height, int width, int patchSize, float threshold) {
        this.height = height;
        this.width = width;
        this.noiseValues = ConvertNoiseData(noiseValues);
        this.patchSize = patchSize;
        NoiseEvaluation();
    }
    private float[,] ConvertNoiseData(float[] noiseData) {
        float[,] newValues = new float[width, height];
        for (int i = 0; i < noiseData.Length; i++) {
            try {
                int row = i / width;
                int col = i % height;

                newValues[row, col] = noiseData[i];
            }
            catch(Exception ex) {
                GD.Print(ex + "Error at index: " + i);
            }
        }
        return newValues;
    }
    private void NoiseEvaluation() {
        for(int y = 0; y < height; y += patchSize) {
            for (int x = 0; x < width; x += patchSize) {
                float value = GetNoisePatchMean(x, y);

                if(value > threshold) {
                    GD.Print($"Texture at ({x}, {y}): Mean={value}");
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
}

