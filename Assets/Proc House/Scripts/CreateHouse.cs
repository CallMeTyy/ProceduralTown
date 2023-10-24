using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHouse : MonoBehaviour
{
    [SerializeField] private GameObject smallHousePrefab;
    public int wallSize = 3;
    public Vector3 size;
    public Vector3 closestPath;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //GenerateRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateNew();
        }
    }

    public void GenerateNew()
    {
        DestroyChildren();
        Vector3 prevSize = size;
        GenerateRandom();
        transform.Translate(Vector3.forward * (prevSize.z - size.z));
    }

    public void GenerateRandom()
    {
        GameObject house = Instantiate(smallHousePrefab, transform);
        CreateSmallHouse sH = house.GetComponent<CreateSmallHouse>();
        sH.setWallSize(wallSize);
        int rW = Random.Range(2, 6);
        int rD = Random.Range(2, Mathf.Min(rW, 5));
        int rH = Random.Range(1, 4);
        float rRH = Random.Range(Mathf.Max(rH / 2, 1), 3);
        bool stoneHouse = Random.value > 0.5f && rH > 1;
        size = new Vector3(rW, rH, rD);
        sH.GenerateBig(rW,rH,rD,rRH,0.5f, smallHousePrefab, transform, stoneHouse);
    }

    public void GenerateWithSize(Vector3 size)
    {
        GameObject house = Instantiate(smallHousePrefab, transform);
        CreateSmallHouse sH = house.GetComponent<CreateSmallHouse>();
        int rW = (int)size.x;
        int rD = (int)size.z;
        int rH = (int)size.y;
        float rRH = Random.Range(Mathf.Max(rH / 2, 1), 3);
        bool stoneHouse = Random.value > 0.5f && rH > 1;
        size = new Vector3(rW, rH, rD);
        sH.GenerateBig(rW,rH,rD,rRH,0.5f, smallHousePrefab, transform, stoneHouse);
    }
    
    

    private void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)Destroy(child.gameObject);
            else DestroyImmediate(child.gameObject);
        }
    }
}
