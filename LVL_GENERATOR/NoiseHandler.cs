﻿using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


internal partial class NoiseHandler : Node2D {
    float[,] noiseValues;
    int height, width;
    int patchSize;
    float threshold;
    Vector2 minCoords;

    public List<Vector2> textureposes = new List<Vector2>();

    public void Constructer(float[] noiseValues, int height, int width, int patchSize, float threshold, float[] minCoords) {
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

    //Legacy code
    //// 1D list --> 2D list
    //public List<List<Vector2>> StructureList(List<Vector2> textposes) {
    //    List<List<Vector2>> newList = new List<List<Vector2>> {
    //        new List<Vector2>()
    //    };
    //    int row = 0;
    //    newList[row].Add(textposes[0]);
    //    for (int i = 0; i < textposes.Count - 1; i++) {
    //        if (textposes[i].Y == textposes[i + 1].Y) {
    //            newList[row].Add(textposes[i + 1]);
    //        }
    //        else {
    //            row++;
    //            newList.Add(new List<Vector2>());
    //            newList[row].Add(textposes[i + 1]);
    //        }
    //    }
    //    return newList;
    //}
    //// 2D list --> 1D list
    //public List<Vector2> StructureList(List<List<Vector2>> textposes) {
    //    List<Vector2> newList = new List<Vector2>();
    //    for (int i = 0; i < textposes.Count; i++) {
    //        for (int j = 0; j < textposes[i].Count; j++) {
    //            newList.Add(textposes[i][j]);
    //        }
    //    }
    //    return newList;
    //}

    //Filter outs vectors close to eachother and replaces them with a mean vector
    public List<Vector2> ListProximity(List<Vector2> textposes, int proximityScale, bool change) {
        for (int i = 0; i < textposes.Count; i++) {
            TargetVector(textposes[i], textposes, proximityScale, ref change);
        }
        //Needs to iterate multiple times idk why
        if(change) {
            ListProximity(textposes, proximityScale, false);
        }
        return textposes;
    }

    private void TargetVector(Vector2 target, List<Vector2> textposes, int proximityScale, ref bool change) {
        int count = 0;
        Vector2 sum = Vector2.Zero;
        List<Vector2> toBeRemoved = new List<Vector2>();

        foreach (Vector2 pos in textposes) {
            if (pos.DistanceTo(target) <= patchSize * proximityScale) {
                sum += pos;
                count++;
                toBeRemoved.Add(pos);
            }
        }

        if (count > 1) {
            change = true;
            Vector2 meanVector = sum / count;
            textposes.Add(meanVector);
            foreach (Vector2 pos in toBeRemoved) {
                textposes.Remove(pos);
            }
        }
    }
}

