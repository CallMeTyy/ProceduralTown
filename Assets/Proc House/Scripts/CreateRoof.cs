using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoof : MonoBehaviour
{
    
    MeshBuilder builder;

    public float width, height, depth;

    public float offset;

    private int wallSize;

    
    // Start is called before the first frame update
    void Awake()
    {
        builder = new MeshBuilder ();
    }

    public void Build(int pWallSize, float pWidth, float pHeight, float pDepth, float pOffset, bool addSideWalls = false)
    {
        wallSize = pWallSize;
        builder = new MeshBuilder ();
        builder.Clear();
        width = pWidth;
        height = pHeight;
        depth = pDepth;
        offset = pOffset;
        createRoof(pWidth,pDepth,pHeight, pOffset);
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
        if (addSideWalls) GetComponentInChildren<RoofWalls>().CreateWalls(pWidth,pDepth,pHeight, pOffset);
    }

    private void Update()
    {
        
    }

    private void createRoof(float width, float depth, float height, float offset)
    {
        
            //Left Side  ===================================
            int v1 = builder.AddVertex(new Vector3(width / 2, 0, -depth / 2), new Vector2(width/3,0));
            int v2 = builder.AddVertex(new Vector3(-width / 2, 0, -depth / 2), new Vector2(0,0));
            int v3 = builder.AddVertex(new Vector3(width / 2, height, 0), new Vector2(width/3,1));
            int v4 = builder.AddVertex(new Vector3(-width / 2, height, 0), new Vector2(0,1));
            
            builder.AddTriangle(v1, v4, v3);
            builder.AddTriangle(v2, v4,v1);
            
            //UnderSide
            int u1 = builder.AddVertex(new Vector3(width / 2, 0, -depth / 2 + offset/2), new Vector2(1,0));
            int u2 = builder.AddVertex(new Vector3(-width / 2, 0, -depth / 2 + offset/2), new Vector2(0,0));
            int u3 = builder.AddVertex(new Vector3(width / 2, height - offset/2, 0), new Vector2(1,1));
            int u4 = builder.AddVertex(new Vector3(-width / 2, height - offset/2, 0), new Vector2(0,1));
            
            builder.AddTriangle(u4, u1, u3);
            builder.AddTriangle(u4, u2,u1);
            
            //UnderRidge
            int r1 = builder.AddVertex(new Vector3(-width / 2, 0, -depth / 2), new Vector2(0,0));
            int r2 = builder.AddVertex(new Vector3(-width / 2, 0, -depth / 2  + offset/2), new Vector2(0,0.2f));
            int r3 = builder.AddVertex(new Vector3(width / 2, 0, -depth / 2),new Vector2(1,0));
            int r4 = builder.AddVertex(new Vector3(width / 2, 0, -depth / 2   + offset/2), new Vector2(1,0.2f));
            
            builder.AddTriangle(r3, r4, r1);
            builder.AddTriangle(r2, r1, r4);

            //Right Side ===================================
            int v5 = builder.AddVertex(new Vector3(-width / 2, 0, depth / 2), new Vector2(0,0));
            int v6 = builder.AddVertex(new Vector3(width / 2, 0, depth / 2), new Vector2(width/3,0));
            int v7 = builder.AddVertex(new Vector3(width / 2, height, 0),new Vector2(width/3,1));
            int v8 = builder.AddVertex(new Vector3(-width / 2, height, 0), new Vector2(0,1));
            
            builder.AddTriangle(v5, v7, v8);
            builder.AddTriangle(v5, v6,v7);
            
            //UnderSide
            int u5 = builder.AddVertex(new Vector3(-width / 2, 0, depth / 2 - offset/2), new Vector2(0,0));
            int u6 = builder.AddVertex(new Vector3(width / 2, 0, depth / 2 - offset/2), new Vector2(1,0));
            int u7 = builder.AddVertex(new Vector3(width / 2, height - offset/2, 0),new Vector2(1,1));
            int u8 = builder.AddVertex(new Vector3(-width / 2, height - offset/2, 0), new Vector2(0,1));
            
            builder.AddTriangle(u5, u7, u6);
            builder.AddTriangle(u5, u8,u7);
            
            //UnderRidge
            int r5 = builder.AddVertex(new Vector3(-width / 2, 0, depth / 2), new Vector2(0,0));
            int r6 = builder.AddVertex(new Vector3(-width / 2, 0, depth / 2  - offset/2), new Vector2(0,0.2f));
            int r7 = builder.AddVertex(new Vector3(width / 2, 0, depth / 2),new Vector2(1,0));
            int r8 = builder.AddVertex(new Vector3(width / 2, 0, depth / 2   - offset/2), new Vector2(1,0.2f));
            
            builder.AddTriangle(r7, r5, r8);
            builder.AddTriangle(r6, r8, r5);
            


            CreateSideWidth(width/2, depth, height, offset/2, false);
            CreateSideWidth(-width/2, depth, height, offset/2, true);
        
    }

    private void CreateSideWidth(float pos, float depth, float height, float offset, bool flipped)
    {
        int f = flipped ? 1 : -1;
        //Bottom Offset
        
        int ov1 = builder.AddVertex(new Vector3(pos, 0, depth/2), new Vector2(0,0));
        int ov2 = builder.AddVertex(new Vector3(pos, 0, depth/2 - offset), new Vector2(0.2f,0));
        int ov3 = builder.AddVertex(new Vector3(pos, height, 0), new Vector2(0,1));
        int ov4 = builder.AddVertex(new Vector3(pos, height - offset, 0), new Vector2(0.2f,1));
        
        
        
        int ov5 = builder.AddVertex(new Vector3(pos, 0, -depth/2), new Vector2(0,0));
        int ov6 = builder.AddVertex(new Vector3(pos, 0, -depth/2 + offset), new Vector2(0.2f,0));
        int ov7 = builder.AddVertex(new Vector3(pos, height, 0), new Vector2(0,1));
        int ov8 = builder.AddVertex(new Vector3(pos, height - offset, 0), new Vector2(0.2f,1));
        
        
        if (flipped)
        {
            builder.AddTriangle(ov4, ov1, ov3);
            builder.AddTriangle(ov4, ov2, ov1);
            
            
            builder.AddTriangle(ov5, ov8, ov7);
            builder.AddTriangle(ov6, ov4, ov5);
        }
        else
        {
            builder.AddTriangle(ov1, ov4, ov3);
            builder.AddTriangle(ov2, ov4, ov1);
            
            
            builder.AddTriangle(ov8, ov5, ov7);
            builder.AddTriangle(ov4, ov6, ov5);
        }
    }
}
