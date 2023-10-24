using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateSmallHouse : MonoBehaviour
{
    [SerializeField] private CreateRoof roofBuilder;
    [SerializeField] private CreateWalls wallBuilder;
    [SerializeField] private CreateWalls bottomBuilder;
    [SerializeField] private CreateFloor floorBuilder;
    [SerializeField] private CreateFloor ceilingBuilder;
    [SerializeField] private CreateWalls stoneWall;
    private int wallSize = 3;

    [SerializeField] private float width, height, roofHeight, depth, offset;
    
    // Start is called before the first frame update
    void Start()
    {
        //Generate(width,height,depth,roofHeight,offset);
    }

    private void Update()
    {
        
    }

    public void setWallSize(int pWallSize)
    {
        wallSize = pWallSize;
    }

    public void Generate(float width, float height, float depth, float roofHeight, float offset, bool stoneWalls = false)
    {
        float pWidth = width * wallSize + offset * 2;
        float pHeight = height * wallSize;
        float pDepth = depth * wallSize + offset;
        float pRoofHeight = roofHeight * wallSize;
        
        roofBuilder.transform.localPosition = new Vector3(0, pHeight, 0);
        roofBuilder.Build(wallSize,pWidth, pRoofHeight, pDepth, offset, true);
        bottomBuilder.BuildBottom(wallSize,pWidth, pDepth, offset);
        floorBuilder.Build(wallSize,pWidth - offset * 2, pDepth - offset);
        ceilingBuilder.Build(wallSize,pWidth - offset * 2, pDepth - offset);
        ceilingBuilder.transform.localPosition = new Vector3(0, pHeight-wallSize, 0);

        if (stoneWalls)
        {
            float stoneWH = Random.Range(1,Mathf.Max(height-1,1)) * wallSize;
            stoneWall.Build(wallSize,pWidth,stoneWH, pDepth, offset,true);
            wallBuilder.Build(wallSize,pWidth, pHeight-stoneWH, pDepth, offset,false, false);
            wallBuilder.transform.localPosition = new Vector3(0, stoneWH, 0);
        }
        else
        {
            wallBuilder.Build(wallSize,pWidth, pHeight, pDepth, offset,true);
            wallBuilder.transform.localPosition = new Vector3(0, 0, 0);
        }
        
    }

    public void GenerateBig(float nWidth, float nHeight, float nDepth, float nRoofHeight, float nOffset,
        GameObject otherHouse, Transform pParent, bool stoneWalls = false)
    {
        float pWidth = nWidth * wallSize + offset * 2;
        float pHeight = nHeight * wallSize;
        float pDepth = nDepth * wallSize + nOffset;
        float pRoofHeight = nRoofHeight * wallSize;
        offset = nOffset;
        
        roofBuilder.transform.localPosition = new Vector3(0, pHeight, 0);
        roofBuilder.Build(wallSize,pWidth, pRoofHeight, pDepth, nOffset, true);
        bottomBuilder.BuildBottom(wallSize,pWidth, pDepth, nOffset);
        floorBuilder.Build(wallSize,pWidth - offset * 2, pDepth - nOffset);
        ceilingBuilder.Build(wallSize,pWidth - offset * 2, pDepth - nOffset);
        ceilingBuilder.transform.localPosition = new Vector3(0, pHeight-wallSize, 0);
        
        if (stoneWalls)
        {
            float stoneWH = Random.Range(1,Mathf.Max(height-1,1)) * wallSize;
            stoneWall.Build(wallSize,pWidth,stoneWH, pDepth, nOffset);
            wallBuilder.Build(wallSize,pWidth, pHeight-stoneWH, pDepth, nOffset,false, false);
            wallBuilder.transform.localPosition = new Vector3(0, stoneWH, 0);
        }
        else
        {
            wallBuilder.Build(wallSize,pWidth, pHeight, pDepth, offset);
            wallBuilder.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (nHeight > 1)
        {
            float addNewBuildingChance = Random.value;
            if (addNewBuildingChance > (1/(nHeight*3)))
            {
                GameObject newBuilding = Instantiate(otherHouse, pParent);
                CreateSmallHouse nHouse = newBuilding.GetComponent<CreateSmallHouse>();
                int rW = Random.Range((int)nWidth + 1, (int) nWidth+3);
                int rD = Random.Range((int)nDepth + 1, (int) nDepth+2);
                int rH = Random.Range(1, (int) nHeight);
                float rRH = Random.Range(Mathf.Max(rH / 2, 1), nRoofHeight);
                nHouse.GenerateBig(rW, rH, rD, rRH, nOffset, otherHouse, pParent);

                CreateHouse pHouse = pParent.GetComponent<CreateHouse>();
                Vector3 oldSize = pHouse.size;
                Vector3 newSize = new Vector3(Mathf.Max(oldSize.x, rW), Mathf.Max(oldSize.y, rH), Mathf.Max(oldSize.z, rD));
                pHouse.size = newSize;
            }

            return;
        }
        
        if (nWidth > 2 && nDepth > 1)
        {
            float addNewBuildingChance = Random.value;
            if (addNewBuildingChance > (1/nWidth*2))
            {
                GameObject newBuilding = Instantiate(otherHouse, pParent);
                CreateSmallHouse nHouse = newBuilding.GetComponent<CreateSmallHouse>();
                int rW = Random.Range((int)nDepth + 1, (int) nWidth);
                int rD = Random.Range(1, rW);
                int rH = Random.Range(1, (int) nHeight);
                float rRH = Random.Range(Mathf.Max(rH / 2, 1), nRoofHeight);
                CreateHouse pHouse = pParent.GetComponent<CreateHouse>();
                Vector3 oldSize = pHouse.size;
                Vector3 newSize = new Vector3(oldSize.x, oldSize.y, rW);
                pHouse.size = newSize;
                nHouse.Generate(rW, rH, rD, rRH, nOffset, stoneWalls);
                if (stoneWalls)
                {
                    float stoneWH = Random.Range(1,Mathf.Max(height-1,1)) * wallSize;
                    stoneWall.Build(wallSize,pWidth,stoneWH, pDepth, nOffset, false, false);
                    wallBuilder.Build(wallSize,pWidth, pHeight-stoneWH, pDepth, nOffset, false, false);
                    wallBuilder.transform.localPosition = new Vector3(0, stoneWH, 0);
                }
                else
                {
                    wallBuilder.Build(wallSize,pWidth, pHeight, pDepth, offset, false, false);
                    wallBuilder.transform.localPosition = new Vector3(0, 0, 0);
                }
                newBuilding.transform.localRotation = Quaternion.Euler(0,90,0);
            }
            return;
        }
    }

    
}
