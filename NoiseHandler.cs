using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


internal partial class NoiseHandler : Node2D {
    float[,] noiseValues;
    int height, width;
    int patchSize;
    float threshold;
    Vector2 minCoords;

    public List<Vector2> textureposes = new List<Vector2>();

    public NoiseHandler(float[] noiseValues, int height, int width, int patchSize, float threshold, float[] minCoords) {
        this.height = height;
        this.width = width;
        this.noiseValues = ConvertNoiseData(noiseValues); //values range from 0f -> 1f
        this.patchSize = patchSize;
        this.threshold = threshold;
        this.minCoords = new Vector2(-minCoords[0], -minCoords[1]);
        NoiseEvaluation();
    }
    //Converts our float array to a 2d float array 
    private float[,] ConvertNoiseData(float[] noiseData) {
        float[,] newValues = new float[width, height];
        for (int i = 0; i < noiseData.Length; i++) {
            try {
                int row = i % width;
                int col = i / width;

                newValues[row, col] = noiseData[i];
            }
            catch (Exception ex) { //Debugging
                GD.Print(ex + "Error at index: " + i);
            }
        }
        return newValues;
    }
    //Determines if a patch from the noisemap is above the threshold
    //we have given
    private void NoiseEvaluation() {
        for (int y = 0; y < height; y += patchSize) {
            for (int x = 0; x < width; x += patchSize) {
                try {
                    float value = GetNoisePatchMean(x, y);

                    if (value < threshold) {
                        //offset the position so it matches our map
                        Vector2 topLeftPosition = new Vector2(x, y);
                        Vector2 texturePosition = topLeftPosition - minCoords;
                        texturePosition += new Vector2(patchSize / 2, patchSize / 2); //center
                        textureposes.Add(texturePosition);
                    }
                }
                catch (Exception ex) {
                    GD.Print(ex + "Error at index: " + x, y);
                }
            }
        }
    }
    //Calculates the mean value of a patch based on given coordinates
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
    public void StructureList(bool type) {
        if(type) {
            StructureTo2DList();
        }
        else
        {
            StructureTo1DList();
        }
    }
    private List<List<Vector2>> StructureTo2DList() {
        List<List<Vector2>> newList = new List<List<Vector2>> {
        new List<Vector2>()
        };
        int row = 0;
        newList[row].Add(textureposes[0]);
        for (int i = 0; i < textureposes.Count - 1; i++) {


            if (textureposes[i].Y == textureposes[i + 1].Y) {
                newList[row].Add(textureposes[i + 1]);
            }
            else {
                row++;
                newList.Add(new List<Vector2>());
                newList[row].Add(textureposes[i + 1]);
            }
        }
        return newList;
    }
    private List<Vector2> StructureTo1DList() {
        List<Vector2> newList
    }
    public List<Vector2> CheckProximity(List<Vector2> textposes) {

    }
}

