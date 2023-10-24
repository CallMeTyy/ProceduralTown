using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PathDrawer : MonoBehaviour
{
    private RectTransform _rect;
    
    public bool isOver = false;

    private Vector2 defaultMin, defaultMax, bigMin, bigMax;

    [SerializeField] private Texture2D splatMap;
    [SerializeField] private Texture2D townMap;
    [SerializeField] private Texture2D waterMap;
    [SerializeField] private Image brushToggle;
    [SerializeField] private Text brushToggleText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Splatmapper mapper;
 
    [SerializeField] private GameObject Closebutton;
    

    private Color transparent;

    [SerializeField] private Slider brushSizeSlider;
    [SerializeField] private Slider buildingRadiusSlider;
    [SerializeField] private Slider buildingCountSlider;
    [SerializeField] private Slider buildingSizeSlider;
    [SerializeField] private Slider noBuildingRadiusSlider;
    [SerializeField] private Slider SpawnBuildingPercentSlider;
    [SerializeField] private Slider _treeDensitySlider;
    public int brushSize = 5;

    
    [Range(10,50)] public int buildingRadius = 15;

    
    [Range(1,25)] public int buildingCount = 5;

    
    [Range(3,10)] public int buildingSize = 5;

    
    [Range(25,150)] public int noBuildingRadius = 50;

    
    [Range(0, 0.95f)] public float SpawnBuildingPercent = 0.75f;

    [Range(0.0001f, 0.01f)] public float treeDensity = 0.0005f;

    public List<GameObject> HousePrefabs;

    private bool pathDrawing, waterDrawing, erase;

    private int stage = 0;

    private bool isLoading;

    private bool initializedValues;
    // Start is called before the first frame update
    void Start()
    {
        pathDrawing = true;
        _rect = GetComponent<RectTransform>();
        defaultMin = _rect.anchorMin;
        defaultMax = _rect.anchorMax;
        bigMin = new Vector2(0.2f, 0);
        bigMax = new Vector2(0.8f, 1);
        transparent = new Color(0, 0, 0, 0);
        brushSizeSlider.value = brushSize;
        buildingRadiusSlider.value = buildingRadius;
        buildingCountSlider.value = buildingCount;
        buildingSizeSlider.value = buildingSize;
        noBuildingRadiusSlider.value = noBuildingRadius;
        SpawnBuildingPercentSlider.value = SpawnBuildingPercent;
        _treeDensitySlider.value = treeDensity;
        initializedValues = true;
    }

    public void setBig()
    {
        _rect.anchorMin = bigMin;
        _rect.anchorMax = bigMax;
        Closebutton.SetActive(true);
    }

    public void SetSmall()
    {
        _rect.anchorMin = defaultMin;
        _rect.anchorMax = defaultMax; 
        Closebutton.SetActive(false);
    }

    public void ClearImage()
    {
        for (int x = 0; x < splatMap.width; x++)
        {
            for (int y = 0; y < splatMap.height; y++)
            {
                splatMap.SetPixel(x,y, transparent);
                townMap.SetPixel(x,y,transparent);
                waterMap.SetPixel(x,y,transparent);
            } 
        }
        waterMap.Apply();
        splatMap.Apply();
        townMap.Apply();
    }
    

    private void Update()
    {
        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;
        if (mouseX > bigMin.x && mouseX < bigMax.x && Closebutton.activeSelf && Input.GetMouseButton(0))
        {
            int x = (int) Map(mouseX, bigMin.x, bigMax.x, 0f, 511f);
            int y = (int) Map(mouseY, 0, 1, 0, 511);
            if (pathDrawing)
            {
                for (int i = x - brushSize; i < x + brushSize; i++)
                {
                    for (int j = y - brushSize; j < y + brushSize; j++)
                    {
                        if (i > 0 && j > 0 && i < 511 && j < 511)
                        {
                            splatMap.SetPixel(i,j,Color.red);
                        }
                    }
                }
            } else if (waterDrawing)
            {
                for (int i = x - brushSize * 5; i < x + brushSize * 5; i++)
                {
                    for (int j = y - brushSize * 5; j < y + brushSize *5 ; j++)
                    {
                        if (i > 0 && j > 0 && i < 511 && j < 511)
                        {
                            float d = 1 - Vector2.Distance(new Vector2(x, y), new Vector2(i, j)) / (brushSize * 5);
                            Color b = new Color(0, 0, d, d);
                            if (waterMap.GetPixel(i,j).b < b.b) waterMap.SetPixel(i,j,b);
                            
                        }
                    }
                }
            } else if (erase)
            {
                for (int i = x - brushSize * 5; i < x + brushSize * 5; i++)
                {
                    for (int j = y - brushSize; j < y + brushSize; j++)
                    {
                        if (i > 0 && j > 0 && i < 511 && j < 511)
                        {
                            waterMap.SetPixel(i,j,transparent);
                            splatMap.SetPixel(i,j, transparent);
                        }
                    }
                }
            }
            
            
            
            //ApplyBuilding(x,y);
            splatMap.Apply();
            waterMap.Apply();
        }
        
        if (isLoading) Load();
    }

    void Load()
    {
        int cases = 5;
        switch (stage)
        {
            case 0:
                ClearTownMap();
                progressBar.value += 1f/cases;
                break;
            case 1:
                ApplyBuilding();
                progressBar.value += 1f/cases;
                break;
            case 2:
                ClearFromPath();
                progressBar.value += 1f/cases;
                break;
            case 3:
                PlaceBuildings();
                progressBar.value += 1f/cases;
                break;
            case 4:
                mapper.DrawMap();
                progressBar.value += 1f/cases;
                break;
            default:
                isLoading = false;
                break;
        }

        stage++;
    }

    public void ToggleBrush()
    {
        if (pathDrawing)
        {
            pathDrawing = false;
            waterDrawing = true;
            erase = false;
            brushToggle.color = Color.blue;
            brushToggleText.text = "Brush: Water";
        }
        else if (waterDrawing)
        {
            pathDrawing = false;
            erase = true;
            waterDrawing = false;
            brushToggle.color = Color.clear;
            brushToggleText.text = "Brush: Erase";
        }
        else if (erase)
        {
            erase = false;
            waterDrawing = false;
            pathDrawing = true;
            brushToggle.color = Color.red;
            brushToggleText.text = "Brush: Path";
        }
    }

    public void ApplyBuilding()
    {
        for (int x = 0; x < townMap.width; x++)
        {
            for (int y = 0; y < townMap.height; y++)
            {
                if (splatMap.GetPixel(x, y).r > 0)
                {
                    for (int i = x - buildingRadius; i < x + buildingRadius; i++)
                    {
                        for (int j = y - buildingRadius; j < y + buildingRadius; j++)
                        {
                            if (i > 0 && j > 0 && i < 511 && j < 511)
                            {
                                float d = Vector2.Distance(new Vector2(x, y), new Vector2(i, j));
                                if (d < buildingRadius && !(waterMap.GetPixel(i,j).b > 0))
                                {
                                    townMap.SetPixel(i,j,Color.green);
                                }
                            }
                        }
                    }
                }
            }
        }
        townMap.Apply();
    }

    void ClearFromPath()
    {
        for (int x = 0; x < townMap.width; x++)
        {
            for (int y = 0; y < townMap.height; y++)
            {
                if (splatMap.GetPixel(x, y).r > 0)
                {
                    for (int i = x - buildingRadius; i < x + buildingRadius; i++)
                    {
                        for (int j = y - buildingRadius; j < y + buildingRadius; j++)
                        {
                            if (i > 0 && j > 0 && i < 511 && j < 511)
                            {
                                float d = Vector2.Distance(new Vector2(x, y), new Vector2(i, j));
                                if (d < buildingRadius - brushSize)
                                {
                                    townMap.SetPixel(i, j, transparent);
                                }
                            }
                        }
                    }
                }
            }
        }
        townMap.Apply();
    }

    void ClearTownMap()
    {
        GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
        for (int i = 0; i < houses.Length; i++)
        {
            DestroyImmediate(houses[i]);
        }
        for (int x = 0; x < splatMap.width; x++)
        {
            for (int y = 0; y < splatMap.height; y++)
            {
                townMap.SetPixel(x,y,transparent);
            } 
        }
        townMap.Apply();
    }

    public void SyncValues()
    {
        if (!initializedValues) return;
        brushSize = (int) brushSizeSlider.value;
        buildingRadius = (int) buildingRadiusSlider.value;
        buildingCount = (int) buildingCountSlider.value;
        buildingSize = (int) buildingSizeSlider.value;
        noBuildingRadius = (int) noBuildingRadiusSlider.value;
        SpawnBuildingPercent = SpawnBuildingPercentSlider.value;
        treeDensity = _treeDensitySlider.value;
    }

    public void StartBuilding()
    {
        isLoading = true;
        stage = 0;
        progressBar.value = 0;
        mapper.treeDensity = treeDensity;
    }

    public void StartBuildingEditor()
    {
        mapper.treeDensity = treeDensity;
        ClearTownMap();
        ApplyBuilding();
        ClearFromPath();
        PlaceBuildings();
        mapper.DrawMap();
    }

    public void PlaceBuildings()
    {
        int buildingsPlaced = 0;
        for (int x = 0; x < townMap.width; x++)
        {
            for (int y = 0; y < townMap.height; y++)
            {
                if (townMap.GetPixel(x, y).g > 0)
                {
                    if (Random.value > (99+SpawnBuildingPercent) / 100f && buildingsPlaced < buildingCount)
                    {
                        GameObject house = Instantiate(HousePrefabs[Random.Range(0, HousePrefabs.Count)]);
                        Vector2 closestPath = findClosestPath(x, y);
                        print(closestPath + " at " + x + "," + y);
                        house.transform.position = new Vector3(x, 15, y);
                        house.transform.LookAt(new Vector3(closestPath.x,15,closestPath.y));
                        //house.transform.localScale *= size;
                        house.tag = "House";
                        CreateHouse cH = house.GetComponent<CreateHouse>();
                        cH.GenerateRandom();
                        Vector3 size = cH.size * 5;
                        cH.closestPath = new Vector3(closestPath.x, 15, closestPath.y);
                        house.transform.Translate((Vector2.Distance(closestPath, new Vector2(x,y))-size.z/2) * Vector3.forward);
                        for (int j = x - (int)size.x/2; j < x + (int)size.x/2; j++)   
                        {
                            for (int k = y - (int)size.z; k < y + (int)size.z; k++)
                            {
                                //if (j > x-buildingSize && j < x + buildingSize && k > y - buildingSize && k < y + buildingSize) townMap.SetPixel(j,k,Color.magenta);
                                //else if (townMap.GetPixel(j,k).g > 0) townMap.SetPixel(j,k,transparent);
                                townMap.SetPixel(j,k,Color.magenta);
                            }
                        }

                        townMap.Apply();
                        buildingsPlaced++;
                    }
                }
            }
        }
    }

    Vector2 findClosestPath(int x, int y)
    {
        Vector2 closest = new Vector2(townMap.width/2, townMap.height/2);
        float distance = 10000;
        for (int i = x - buildingSize - 10; i < x + buildingSize + 10; i++)
        {
            for (int j = y - buildingSize - 10; j < y + buildingSize + 10; j++)
            {
                if (splatMap.GetPixel(i, j).r > 0)
                {
                    int dist = (i - x) *(i - x) + (j - y) * (j - y);
                    if (dist < distance)
                    {
                        distance = dist;
                        closest = new Vector2(i, j);
                    }
                }
            }
        }
        return closest;
    }
    
    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Math.Max(in_min, Math.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
