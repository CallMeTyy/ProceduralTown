using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Splatmapper : MonoBehaviour
{
    public Terrain t;
    [SerializeField] private Texture2D splat;
    [SerializeField] private Texture2D town;
    [SerializeField] private Texture2D height;
    [SerializeField] private Texture2D water;
    [SerializeField] private Toggle _showBuildingToggle;
    
    private int snowHeight = 125;
    private float rockStrength = 4f;
    private bool showBuildingArea;

    public float treeDensity = 0.001f;

    void Start()
    {
        showBuildingArea = _showBuildingToggle.isOn;
        HeightMap();
        SplatMap();
    }

    public void DrawMap()
    {
        HeightMap();
        SplatMap();
        PlaceTrees();
    }

    public void ShowAreaToggle()
    {
        showBuildingArea = _showBuildingToggle.isOn;
    }
    
    void HeightMap()
    {
        float[,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight];
        for (int y = 0; y < t.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < t.terrainData.alphamapWidth; x++)
            {
                float r = height.GetPixel(x, y).r;
                float b = water.GetPixel(x, y).b;
                if (b > 0 && splat.GetPixel(x,y).r < 1)
                {
                    map[y, x] = 0.075f - (b/20);
                }
                else
                {
                    map[y, x] = 0.075f;
                    //map[y, x] = 0.025f + r;
                    //float newX = Mathf.Floor(x / 4);
                    //float newY = Mathf.Floor(y / 4);
                    //map[y, x] = newX / 256f + newY / 256f;
                }
                
            }
        }
        t.terrainData.SetHeights(0, 0, map);
    }

    void SplatMap()
    {
        float[,,] map = new float[t.terrainData.alphamapWidth, t.terrainData.alphamapHeight, 5];
        for (int y = 0; y < t.terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < t.terrainData.alphamapWidth; x++)
            {
                float normX = x * 1.0f / (t.terrainData.alphamapWidth - 1);
                float normY = y * 1.0f / (t.terrainData.alphamapHeight - 1);
                float r = splat.GetPixel(x, y).r;
                //float g = town.GetPixel(x, y).g;
                float b = town.GetPixel(x, y).b;
                float w = water.GetPixel(x, y).b;
                float dot = (1 - Vector3.Dot(t.terrainData.GetInterpolatedNormal(normX,normY), Vector3.up)) * rockStrength;
                float snowR = Random.Range(0.6f, 0.65f);
                float snowG = Random.Range(0.5f, 0.55f);
                if (w > 0 && r > 0)
                {
                    map[y, x, 3] = (dot + r) - snowR;
                }
                else
                {
                    map[y, x, 3] = (dot + r) - snowR;
                }
                map[y, x, 1] = (1 - dot - r) - snowG;
                map[y, x, 2] = (1- snowR) * r + (1-snowG) * (1-r);
                
                if (showBuildingArea) map[y, x, 3] = b;
                if (t.terrainData.GetInterpolatedHeight(normX, normY) > snowHeight)
                {
                    map[y, x, 2] = Mathf.Min((t.terrainData.GetInterpolatedHeight(normX, normY) - snowHeight) / 10, 1);
                }
            }
        }
        t.terrainData.SetAlphamaps(0, 0, map);
    }
    
    void PlaceTrees()
    {
        t.terrainData.treeInstances = new TreeInstance[0];
        float width = t.terrainData.alphamapWidth;
        float height = t.terrainData.alphamapHeight;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float r = splat.GetPixel(x, y).r;
                float g = town.GetPixel(x, y).g;
                float b = town.GetPixel(x, y).b;
                float w = water.GetPixel(x, y).b;
                float com = r + g + b + w;
                if (com == 0)
                {
                    if (Random.value > 1 - treeDensity)
                    {
                        TreeInstance tree = new TreeInstance();
                        tree.prototypeIndex = Random.Range(0, 3);
                        tree.widthScale = 1f;
                        tree.heightScale = 1f;
                        tree.rotation = Random.Range(0, 3f);
                        tree.color = Color.white;
                        tree.lightmapColor = Color.white;
                        tree.position = new Vector3(x/width, 0, y/height);
                        t.AddTreeInstance(tree);
                        print(x + ", " + y);
                    }
                    
                    if (Random.value > 0.93f)
                    {
                        TreeInstance tree = new TreeInstance();
                        tree.prototypeIndex = Random.Range(3, 5);
                        tree.widthScale = Random.Range(1f,2f);
                        tree.heightScale = Random.Range(1,2f);
                        tree.rotation = Random.Range(0, 3f);
                        tree.color = Color.white;
                        tree.lightmapColor = Color.white;
                        tree.position = new Vector3(x/width, 0, y/height);
                        t.AddTreeInstance(tree);
                        print(x + ", " + y);
                    }
                    
                    if (Random.value > 1-treeDensity/4)
                    {
                        TreeInstance tree = new TreeInstance();
                        tree.prototypeIndex = 5;
                        tree.widthScale = Random.Range(2f,10f);
                        tree.heightScale = Random.Range(2f,10f);
                        tree.rotation = Random.Range(0, 3f);
                        tree.color = Color.white;
                        tree.lightmapColor = Color.white;
                        tree.position = new Vector3(x/width, 0, y/height);
                        t.AddTreeInstance(tree);
                        print(x + ", " + y);
                    }
                }
            }
        }
        t.Flush();
    }
    
}
