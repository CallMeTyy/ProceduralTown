using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofWalls : MonoBehaviour
{
    MeshBuilder builder;
    private CreatePillar pillarBuilder;
    private void Awake()
    {
        builder = new MeshBuilder ();
        builder.Clear ();
    }

    public void CreateWalls(float width, float depth, float height, float offset)
    {
        builder = new MeshBuilder ();
        builder.Clear ();
        pillarBuilder = GetComponentInChildren<CreatePillar>();
        pillarBuilder.resetBuilder();
        
            float uv = (depth - offset)  / 3;
            int v1 = builder.AddVertex(new Vector3(width / 2 - offset, 0, -depth / 2 + offset /2), new Vector2(0,0));
            int v2 = builder.AddVertex(new Vector3(width / 2 - offset, 0, depth / 2 - offset/2), new Vector2(uv,0));
            int v3 = builder.AddVertex(new Vector3(width / 2 - offset, height, 0), new Vector2(uv/2,1));
            
            
            int v4 = builder.AddVertex(new Vector3(-width / 2 + offset, 0, -depth / 2 + offset/2), new Vector2(0,0));
            int v5 = builder.AddVertex(new Vector3(-width / 2 + offset, 0, depth / 2 - offset/2), new Vector2(uv,0));
            int v6 = builder.AddVertex(new Vector3(-width / 2 + offset, height, 0), new Vector2(uv/2,1));

				
            builder.AddTriangle(v2, v1, v3);
            builder.AddTriangle(v4, v5,v6);
            pillarBuilder.createRoundPillar(new Vector3(width / 2 - offset, 0, 0), new Vector3(width / 2 - offset, height - offset/2, 0),0.2f);
            pillarBuilder.createRoundPillar(new Vector3(-width / 2 + offset, 0, 0), new Vector3(-width / 2 + offset, height - offset/2, 0),0.2f);
        
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
        pillarBuilder.build();
        
    }
}
