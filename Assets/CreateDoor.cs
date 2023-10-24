using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CreateDoor : MonoBehaviour
{
    MeshBuilder builder;
    
    
    void Awake()
    {
        builder = new MeshBuilder ();
    }
    
    public void Build(Vector3 pos, float pWidth, float pHeight, bool depth)
    {
        builder = new MeshBuilder ();
        builder.Clear ();
        createDoor(pos, pWidth, pHeight,depth);
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
    }

    public void ClearDoor()
    {
        builder = new MeshBuilder ();
        builder.Clear ();
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
    }

    private void createDoor(Vector3 pPos, float width, float height, bool depth = false)
    {
        Vector3 p = pPos;
        Vector3 add = depth ? new Vector3(0, 0, width/2) : new Vector3(width/2, 0, 0);
        Vector3 h = new Vector3(0, height, 0);
        
        int v1 = builder.AddVertex(p - add, new Vector2(0, 0));
        int v2 = builder.AddVertex(p + add, new Vector2(width/3, 0));
        int v3 = builder.AddVertex(p - add + h, new Vector2(0, height/3));
        int v4 = builder.AddVertex(p + add + h, new Vector2(width/3, height/3));
        
        builder.AddTriangle(v1,v2,v3);
        builder.AddTriangle(v2,v4,v3);
        
        int v5 = builder.AddVertex(p - add, new Vector2(0, 0));
        int v6 = builder.AddVertex(p + add, new Vector2(width/3, 0));
        int v7 = builder.AddVertex(p - add + h, new Vector2(0, height/3));
        int v8 = builder.AddVertex(p + add + h, new Vector2(width/3, height/3));
        
        builder.AddTriangle(v5,v7,v6);
        builder.AddTriangle(v6,v7,v8);
    }

    

    

    

    

    

    
}