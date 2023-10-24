using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateWalls : MonoBehaviour
{
    
    MeshBuilder builder;

    public float width, height, depth;

    public float offset;

    public float wallSize = 3;

    private CreatePillar pillarBuilder;
    private CreateDoor doorBuilder;
    // Start is called before the first frame update
    void Awake()
    {
        builder = new MeshBuilder ();
        pillarBuilder = GetComponentInChildren<CreatePillar>();
        doorBuilder = GetComponentInChildren<CreateDoor>();
        //GetComponentInChildren<RoofWalls>().CreateWalls(width,depth,height, offset);
    }
    
    public void Build(int pWallSize, float pWidth, float pHeight, float pDepth, float pOffset, bool addDoorOnDepth = false, bool addDoors = true)
    {
        wallSize = pWallSize;
        builder = new MeshBuilder ();
        pillarBuilder = GetComponentInChildren<CreatePillar>();
        doorBuilder = GetComponentInChildren<CreateDoor>();
        doorBuilder.ClearDoor();
        builder.Clear ();
        pillarBuilder.resetBuilder();
        offset = pOffset;
        width = pWidth;
        depth = pDepth;
        height = pHeight;
        int storyCount = Mathf.CeilToInt(pHeight / wallSize);
        float hLeft = pHeight;
        for (int i = 0; i < storyCount; i++)
        {
            float h = hLeft >= wallSize ? wallSize : hLeft;
            GenerateWalls(pWidth, pDepth,h , i==0 && !addDoorOnDepth && addDoors, i*wallSize, true, addDoorOnDepth);
            hLeft -= wallSize;
        }
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
        pillarBuilder.build();
    }

    public void BuildBottom(int pWallSize, float pWidth, float pDepth, float pOffset)
    {
        wallSize = pWallSize;
        builder = new MeshBuilder ();
        pillarBuilder = GetComponentInChildren<CreatePillar>();
        builder.Clear ();
        pillarBuilder.resetBuilder();
        offset = pOffset;
        width = pWidth;
        depth = pDepth;
        height = 6;
        GenerateWalls(pWidth, pDepth, 6, false, 0, true);
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
        pillarBuilder.build();
    }

    private void Update()
    {
        
    }

    private void GenerateWalls(float width, float depth, float height, bool addDoor = true, float startHeight = 0, bool pAddPillar = true, bool addDoorOnDepth = false)
    {
        bool addPillar = pAddPillar;
        
        Vector3 frontBL = new Vector3(-width / 2 + offset, startHeight, depth / 2 - offset / 2);
        Vector3 frontTR = new Vector3(width / 2 - offset, startHeight+height, depth / 2 - offset / 2);
        if (addDoor) createFrontWall(frontBL, frontTR, width - offset * 2, false);
        else createWall(frontBL, frontTR, width - offset * 2, false, -1, addPillar);

        Vector3 backBL = new Vector3(-width/2 + offset, startHeight, -depth/2 + offset / 2);
        Vector3 backTR = new Vector3(width/2 - offset, startHeight+height, -depth/2 + offset / 2);
        createWall(backBL, backTR, width - offset * 2, false, -1, addPillar);

        Vector3 rightBL = new Vector3(-width / 2 + offset, startHeight, -depth / 2 + offset / 2);
        Vector3 rightTR = new Vector3(-width / 2 + offset, startHeight+height, depth / 2 - offset / 2);
        if (addDoorOnDepth) createFrontWall(rightBL, rightTR, depth - offset, true);
        else createWall(rightBL, rightTR, depth - offset, true, -1, addPillar);

        Vector3 leftBL = new Vector3(width/2 - offset, startHeight, -depth/2 + offset / 2);
        Vector3 leftTR = new Vector3(width/2 - offset, startHeight+height, depth/2 - offset / 2);
        createWall(leftBL, leftTR, depth - offset, true, -1, addPillar);
        
    }

    private float createWallPiece(Vector3 bL, Vector3 tR, float wdLeft, bool depth)
    {
        Vector3 h = new Vector3(0, tR.y - bL.y, 0);
        Vector3 add = depth ? new Vector3(0, 0, wallSize) : new Vector3(wallSize, 0, 0);
        if (wdLeft < wallSize) add = add * (wdLeft / wallSize);
        float uv = wdLeft < wallSize ? wdLeft/wallSize : 1;
        int v1 = builder.AddVertex(bL, new Vector2(0, 0));
        int v2 = builder.AddVertex(bL + add, new Vector2(uv, 0));
        int v3 = builder.AddVertex(bL + h, new Vector2(0, h.y/wallSize));
        int v4 = builder.AddVertex(bL + add + h, new Vector2(uv, h.y/wallSize));
        
        int v5 = builder.AddVertex(bL, new Vector2(0, 0));
        int v6 = builder.AddVertex(bL + add, new Vector2(uv, 0));
        int v7 = builder.AddVertex(bL + h, new Vector2(0, h.y/wallSize));
        int v8 = builder.AddVertex(bL + add + h, new Vector2(uv, h.y/wallSize));
        
        builder.AddTriangle(v1,v2,v3);
        builder.AddTriangle(v2,v4,v3);
        builder.AddTriangle(v5,v7,v6);
        builder.AddTriangle(v6,v7,v8);

        return wdLeft - wallSize;
    }

    private void createDoorWall(Vector3 bL, Vector3 tR, float wdLeft, bool depth)
    {
        Vector3 h = new Vector3(0, tR.y - bL.y - offset/4, 0);
        Vector3 add = depth ? new Vector3(0, 0, wallSize) : new Vector3(wallSize, 0, 0);
        Vector3 d = depth ? new Vector3(0, 0, 0.2f) : new Vector3(0.2f, 0, 0);
        if (wdLeft < wallSize) add = add * (wdLeft / wallSize);
        float uv = wdLeft < wallSize ? wdLeft/wallSize : 1;
        
        int v1 = builder.AddVertex(bL, new Vector2(0, 0));
        int v2 = builder.AddVertex(bL + h, new Vector2(0, 1));
        
        int v3 = builder.AddVertex(bL + add/4, new Vector2(uv/3, 0));
        int v4 = builder.AddVertex(bL + add/4 + h * .66f, new Vector2(uv/3, 0.75f));
        int v5 = builder.AddVertex(bL + add*.75f, new Vector2(uv*.66f, 0));
        int v6 = builder.AddVertex(bL + add*.75f + h * .66f, new Vector2(uv*.75f, 0.75f));

        int v7 = builder.AddVertex(bL + add, new Vector2(1, 0));
        int v8 = builder.AddVertex(bL + h + add, new Vector2(1, 1));

        int v9 = builder.AddVertex(bL + add / 4 + h, new Vector2(uv/4, 1));
        int v10 = builder.AddVertex(bL + add *.75f + h, new Vector2(uv*.75f, 1));
        
        builder.AddTriangle(v1,v3,v2);
        builder.AddTriangle(v2,v3,v9);
        
        builder.AddTriangle(v4,v10,v9);
        builder.AddTriangle(v6,v10,v4);
        
        builder.AddTriangle(v5,v8,v10);
        builder.AddTriangle(v7,v8,v5);
        
        pillarBuilder.createRoundPillar(bL + add/4, bL + add / 4 + h * .75f, 0.2f);
        pillarBuilder.createRoundPillar(bL + add*.75f, bL + add *.75f + h * .75f, 0.2f);
        pillarBuilder.createRoundPillar(bL + h*.75f, bL + add + h * .75f, 0.2f);
        //pillarBuilder.createRoundPillar(bL, bL + add /3 + d, 0.2f);
        //pillarBuilder.createRoundPillar(bL + add * .66f - d, bL + add, 0.2f);
        Vector3 smallAdd = add / 8;
        pillarBuilder.createBlockPillar(bL-Vector3.up*.3f+smallAdd, bL-Vector3.up*.3f+add-smallAdd, 0.6f,0.15f);
        pillarBuilder.createBlockPillar(bL-Vector3.up*.6f+smallAdd, bL-Vector3.up*.6f+add-smallAdd, 1.2f,0.15f);
        pillarBuilder.createBlockPillar(bL-Vector3.up*.9f+smallAdd, bL-Vector3.up*.9f+add-smallAdd, 1.8f,0.15f);
        doorBuilder.Build(bL + add/2, wallSize/2, wallSize*.66f,depth);
    }
    
    private void createWindowWall(Vector3 bL, Vector3 tR, float wdLeft, bool depth)
    {
        Vector3 h = new Vector3(0, tR.y - bL.y - offset/4, 0);
        Vector3 add = depth ? new Vector3(0, 0, wallSize) : new Vector3(wallSize, 0, 0);
        //Vector3 d = depth ? new Vector3(0, 0, 0.2f) : new Vector3(0.2f, 0, 0);
        if (wdLeft < wallSize) add = add * (wdLeft / wallSize);
        float uv = wdLeft < wallSize ? wdLeft/wallSize : 1;
        
        int v1 = builder.AddVertex(bL, new Vector2(0, 0));
        int v2 = builder.AddVertex(bL + h, new Vector2(0, 1));
        
        int v3 = builder.AddVertex(bL + add/3, new Vector2(uv/3, 0));
        int v4 = builder.AddVertex(bL + add/3 + h * .66f, new Vector2(uv/3, 0.66f));
        
        int v5 = builder.AddVertex(bL + add*.66f, new Vector2(uv*.66f, 0));
        int v6 = builder.AddVertex(bL + add*.66f + h * .66f, new Vector2(uv*.66f, 0.66f));

        int v7 = builder.AddVertex(bL + add, new Vector2(1, 0));
        int v8 = builder.AddVertex(bL + h + add, new Vector2(1, 1));

        int v9 = builder.AddVertex(bL + add / 3 + h, new Vector2(uv/3, 1));
        int v10 = builder.AddVertex(bL + add *.66f + h, new Vector2(uv*.66f, 1));

        int v11 = builder.AddVertex(bL + add / 3 + h / 3, new Vector2(uv / 3, 1f / 3));
        int v12 = builder.AddVertex(bL + add *.66f + h / 3, new Vector2(uv *.66f, 1f / 3));
        
        builder.AddTriangle(v1,v3,v2);
        builder.AddTriangle(v2,v3,v9);
        
        builder.AddTriangle(v4,v10,v9);
        builder.AddTriangle(v6,v10,v4);
        
        builder.AddTriangle(v5,v8,v10);
        builder.AddTriangle(v7,v8,v5);
        
        builder.AddTriangle(v3,v12,v11);
        builder.AddTriangle(v5,v12,v3);
        
        //pillarBuilder.createRoundPillar(bL + add/3, bL + add / 3 + h * .66f, 0.2f);
        //pillarBuilder.createRoundPillar(bL + add*.66f, bL + add *.66f + h * .66f, 0.2f);
        //pillarBuilder.createRoundPillar(bL + h*.66f, bL + add + h * .66f, 0.2f);
        //pillarBuilder.createRoundPillar(bL + h/3, bL + add + h/3, 0.2f);
        //pillarBuilder.createRoundPillar(bL, bL + add /3 + d, 0.2f);
        //pillarBuilder.createRoundPillar(bL + add * .66f - d, bL + add, 0.2f);
    }

    private void createCrossWall(Vector3 bL, Vector3 tR, float wdLeft, bool depth)
    {
        Vector3 up = new Vector3(0, tR.y - bL.y, 0);
        Vector3 add = depth ? Vector3.forward : Vector3.right;
        if (wdLeft >= wallSize) add *= wallSize;
        else add *= wdLeft;
        createWallPiece(bL, tR, wdLeft, depth);
        pillarBuilder.createRoundPillar(bL, bL+add+up, 0.2f);
        pillarBuilder.createRoundPillar(bL+up, bL+add, 0.2f);
    }

    private void createFrontWall(Vector3 bottomLeft, Vector3 topRight, float widthLeft, bool depth)
    {
        int wallPieces = (int)(widthLeft / wallSize);
        int doorInt = Random.Range(0, wallPieces);
        createWall(bottomLeft,topRight,widthLeft,depth, wallPieces-doorInt);
    }

    private void createWall(Vector3 bottomLeft, Vector3 topRight, float widthLeft, bool depth, int doorInt = -1, bool addPillars = true)
    {
        Vector3 up = new Vector3(0, topRight.y - bottomLeft.y, 0);
        Vector3 o = new Vector3(0, 0.2f, 0);
        pillarBuilder.createRoundPillar(bottomLeft, bottomLeft + up, 0.2f);
        Vector3 add = depth ? new Vector3(0, 0, wallSize) : new Vector3(wallSize, 0, 0);
        if (widthLeft < wallSize)
        {
            Vector3 finalWallPost = (new Vector3(topRight.x, bottomLeft.y, topRight.z) - bottomLeft).normalized *widthLeft;
            if (addPillars && widthLeft != 0)
            {
                //pillarBuilder.createRoundPillar(bottomLeft, bottomLeft + finalWallPost, 0.2f);
                pillarBuilder.createRoundPillar(bottomLeft + up - o, bottomLeft + up - o + finalWallPost, 0.2f);
            }
            //pillarBuilder.createRoundPillar(topRight - up, topRight, 0.2f);
            createWallPiece(bottomLeft, topRight, widthLeft, depth);
            return;
        }
        if (doorInt != -1)
        {
            
            int thisWall = (int)(widthLeft / wallSize);
            if (thisWall == doorInt)
            {
                Vector3 topPost = (new Vector3(topRight.x, bottomLeft.y, topRight.z) - bottomLeft).normalized * wallSize;
                pillarBuilder.createRoundPillar(bottomLeft + up - o, bottomLeft + up + topPost - o, 0.2f);
                createDoorWall(bottomLeft, topRight, widthLeft, depth);
                createWall(bottomLeft + add, topRight, widthLeft - wallSize, depth, doorInt, addPillars);
                return;
            }
        }
        Vector3 newWallPost = (new Vector3(topRight.x, bottomLeft.y, topRight.z) - bottomLeft).normalized * wallSize;
        if (addPillars)
        {
            //pillarBuilder.createRoundPillar(bottomLeft, bottomLeft + newWallPost, 0.2f);
            pillarBuilder.createRoundPillar(bottomLeft + up - o,
                bottomLeft + up - o + newWallPost, 0.2f);
        }
        float rV = Random.value;
        float wallSizeLeft = widthLeft - wallSize;
        
        if (rV < 0.2f && up.magnitude >= wallSize) createCrossWall(bottomLeft, topRight, widthLeft, depth);
        else createWallPiece(bottomLeft, topRight, widthLeft, depth);
        
        
        createWall(bottomLeft + add, topRight, wallSizeLeft, depth, doorInt, addPillars);
    }

    

    
}
