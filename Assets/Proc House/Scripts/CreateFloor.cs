using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CreateFloor : MonoBehaviour
{
    MeshBuilder builder;
    private int wallSize;
    
    void Awake()
    {
        builder = new MeshBuilder ();
    }
    
    public void Build(int pWallSize, float pWidth, float pDepth)
    {
        wallSize = pWallSize;
        builder = new MeshBuilder ();
        builder.Clear ();
        createFloor(pWidth, pDepth);
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
    }

    private void createFloor(float width, float depth)
    {
        Vector3 p = transform.localPosition;
        int v1 = builder.AddVertex(new Vector3(p.x - width / 2, p.y, p.z - depth / 2), new Vector2(0, 0));
        int v2 = builder.AddVertex(new Vector3(p.x + width / 2, p.y, p.z - depth / 2), new Vector2(width/wallSize, 0));
        int v3 = builder.AddVertex(new Vector3(p.x - width / 2, p.y, p.z + depth / 2), new Vector2(0, depth/wallSize));
        int v4 = builder.AddVertex(new Vector3(p.x + width / 2, p.y, p.z + depth / 2), new Vector2(width/wallSize, depth/wallSize));
        
        int v5 = builder.AddVertex(new Vector3(p.x - width / 2, p.y, p.z - depth / 2), new Vector2(0, 0));
        int v6 = builder.AddVertex(new Vector3(p.x + width / 2, p.y, p.z - depth / 2), new Vector2(width/wallSize, 0));
        int v7 = builder.AddVertex(new Vector3(p.x - width / 2, p.y, p.z + depth / 2), new Vector2(0, depth/wallSize));
        int v8 = builder.AddVertex(new Vector3(p.x + width / 2, p.y, p.z + depth / 2), new Vector2(width/wallSize, depth/wallSize));
        
        builder.AddTriangle(v1,v2,v3);
        builder.AddTriangle(v2,v4,v3);
        
        builder.AddTriangle(v6,v5,v7);
        builder.AddTriangle(v8,v6,v7);
    }

    

    

    

    

    

    
}
