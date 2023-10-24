using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CreatePillar : MonoBehaviour
{
    MeshBuilder builder;

    // Start is called before the first frame update
    void Awake()
    {
        builder = new MeshBuilder ();
        builder.Clear ();
    }

    public void createPillar(Vector3 pos, float height, float d)
    {
        float c = 0.71f;
        float r = d / 2;
        
        int v1 = builder.AddVertex(new Vector3(pos.x - r, 0, pos.z), new Vector2(0,0));
        int v2 = builder.AddVertex(new Vector3(pos.x - r, height, pos.z), new Vector2(0,1));
        int v3 = builder.AddVertex(new Vector3(pos.x - r * c, 0, pos.z - r*c), new Vector2(0.125f,0));
        int v4 = builder.AddVertex(new Vector3(pos.x - r * c, height, pos.z - r*c), new Vector2(0.125f,1));
        int v5 = builder.AddVertex(new Vector3(pos.x, 0, pos.z - r), new Vector2(0.25f,0));
        int v6 = builder.AddVertex(new Vector3(pos.x, height, pos.z - r), new Vector2(0.25f,1));
        int v7 = builder.AddVertex(new Vector3(pos.x + r*c, 0, pos.z - r*c), new Vector2(0.375f,0));
        int v8 = builder.AddVertex(new Vector3(pos.x + r*c, height, pos.z - r*c), new Vector2(0.375f,1));
        int v9 = builder.AddVertex(new Vector3(pos.x + r, 0, pos.z), new Vector2(0.5f,0));
        int v10 = builder.AddVertex(new Vector3(pos.x + r, height, pos.z), new Vector2(0.5f,1));
        int v11 = builder.AddVertex(new Vector3(pos.x + r*c, 0, pos.z + r*c), new Vector2(0.625f,0));
        int v12 = builder.AddVertex(new Vector3(pos.x + r*c, height, pos.z + r*c), new Vector2(0.625f,1));
        int v13 = builder.AddVertex(new Vector3(pos.x, 0, pos.z + r), new Vector2(0.75f,0));
        int v14 = builder.AddVertex(new Vector3(pos.x, height, pos.z + r), new Vector2(0.75f,1));
        int v15 = builder.AddVertex(new Vector3(pos.x - r*c, 0, pos.z + r*c), new Vector2(0.875f,0));
        int v16 = builder.AddVertex(new Vector3(pos.x - r*c, height, pos.z + r*c), new Vector2(0.875f,1));
        int v17 = builder.AddVertex(new Vector3(pos.x - r, 0, pos.z), new Vector2(1,0));
        int v18 = builder.AddVertex(new Vector3(pos.x - r, height, pos.z), new Vector2(1,1));
        
        builder.AddTriangle(v1, v2, v3);
        builder.AddTriangle(v2, v4, v3);
        builder.AddTriangle(v3, v4, v5);
        builder.AddTriangle(v4, v6, v5);
        builder.AddTriangle(v5, v6, v7);
        builder.AddTriangle(v6, v8, v7);
        builder.AddTriangle(v7, v8, v9);
        builder.AddTriangle(v8, v10, v9);
        builder.AddTriangle(v9, v10, v11);
        builder.AddTriangle(v10, v12, v11);
        builder.AddTriangle(v11, v12, v13);
        builder.AddTriangle(v12, v14, v13);
        builder.AddTriangle(v13, v14, v15);
        builder.AddTriangle(v14, v16, v15);
        builder.AddTriangle(v15, v16, v17);
        builder.AddTriangle(v16, v18, v17);
    }

    public void createBlockPillar(Vector3 start, Vector3 end, float w, float h)
    {
        Vector3 line = end - start;
        Vector3 forward = (Quaternion.LookRotation(line, Vector3.up) * Vector3.down).normalized * h;
        Vector3 back = (Quaternion.LookRotation(line, Vector3.up) * Vector3.up).normalized * h;
        Vector3 left = (Quaternion.LookRotation(line, Vector3.up) * Vector3.left).normalized * w;
        Vector3 right = (Quaternion.LookRotation(line, Vector3.up) * Vector3.right).normalized * w;
        
        int v1 = builder.AddVertex(start + left + forward, new Vector2(0,0));
        int v2 = builder.AddVertex(end + left + forward, new Vector2(0,1));
        int v3 = builder.AddVertex(start + left + back, new Vector2(0.25f,0));
        int v4 = builder.AddVertex(end + left + back, new Vector2(0.25f,1));
        int v5 = builder.AddVertex(start + right + forward, new Vector2(0.5f,0));
        int v6 = builder.AddVertex(end + right + forward, new Vector2(0.5f,1));
        int v7 = builder.AddVertex(start + right + back, new Vector2(0.75f,0));
        int v8 = builder.AddVertex(end + right + back, new Vector2(0.75f,1));
        int v9 = builder.AddVertex(start + left + forward, new Vector2(1,0));
        int v10 = builder.AddVertex(end + left + forward, new Vector2(1,1));
        
        builder.AddTriangle(v1,v2,v3);
        builder.AddTriangle(v2,v4,v3);
        builder.AddTriangle(v3,v4,v7);
        builder.AddTriangle(v4,v8,v7);
        builder.AddTriangle(v7,v8,v5);
        builder.AddTriangle(v8,v6,v5);
        builder.AddTriangle(v5,v6,v9);
        builder.AddTriangle(v6,v10,v9);
        
        int v11 = builder.AddVertex(start + left + forward, new Vector2(0,0));
        int v12 = builder.AddVertex(start + right + forward, new Vector2(1,0));
        int v13 = builder.AddVertex(start + left + back, new Vector2(0,1));
        int v14 = builder.AddVertex(start + right + back, new Vector2(1,1));

        builder.AddTriangle(v11,v13,v12);
        builder.AddTriangle(v12,v13,v14);
        
        int v15 = builder.AddVertex(end + left + forward, new Vector2(0,0));
        int v16 = builder.AddVertex(end + right + forward, new Vector2(1,0));
        int v17 = builder.AddVertex(end + left + back, new Vector2(0,1));
        int v18 = builder.AddVertex(end + right + back, new Vector2(1,1));

        builder.AddTriangle(v15,v16,v17);
        builder.AddTriangle(v16,v18,v17);
    }

    public void createRoundPillar(Vector3 start, Vector3 end, float d)
    {
        Vector3 line = end - start;
        Vector3 forward = (Quaternion.LookRotation(line, Vector3.up) * Vector3.down).normalized * d;
        Vector3 back = (Quaternion.LookRotation(line, Vector3.up) * Vector3.up).normalized * d;
        Vector3 left = (Quaternion.LookRotation(line, Vector3.up) * Vector3.left).normalized * d;
        Vector3 right = (Quaternion.LookRotation(line, Vector3.up) * Vector3.right).normalized * d;
        
        
        
        int v1 = builder.AddVertex(start + left, new Vector2(0,0));
        int v2 = builder.AddVertex(end + left, new Vector2(0,1));
        int v3 = builder.AddVertex(start + Vector3.Normalize(left + back) * d, new Vector2(0.125f,0));
        int v4 = builder.AddVertex(end + Vector3.Normalize(left + back) * d, new Vector2(0.125f,1));
        int v5 = builder.AddVertex(start + back, new Vector2(0.25f,0));
        int v6 = builder.AddVertex(end + back, new Vector2(0.25f,1));
        int v7 = builder.AddVertex(start + Vector3.Normalize(right + back) * d, new Vector2(0.375f,0));
        int v8 = builder.AddVertex(end + Vector3.Normalize(right + back) * d, new Vector2(0.375f,1));
        int v9 = builder.AddVertex(start + right, new Vector2(0.5f,0));
        int v10 = builder.AddVertex(end + right, new Vector2(0.5f,1));
        int v11 = builder.AddVertex(start + Vector3.Normalize(right + forward) * d, new Vector2(0.625f,0));
        int v12 = builder.AddVertex(end + Vector3.Normalize(right + forward) * d, new Vector2(0.625f,1));
        int v13 = builder.AddVertex(start + forward, new Vector2(0.75f,0));
        int v14 = builder.AddVertex(end + forward, new Vector2(0.75f,1));
        int v15 = builder.AddVertex(start + Vector3.Normalize(left + forward) * d, new Vector2(0.875f,0));
        int v16 = builder.AddVertex(end + Vector3.Normalize(left + forward) * d, new Vector2(0.875f,1));
        int v17 = builder.AddVertex(start+left, new Vector2(1,0));
        int v18 = builder.AddVertex(end+left, new Vector2(1,1));
        
        builder.AddTriangle(v1, v2, v3);
        builder.AddTriangle(v2, v4, v3);
        builder.AddTriangle(v3, v4, v5);
        builder.AddTriangle(v4, v6, v5);
        builder.AddTriangle(v5, v6, v7);
        builder.AddTriangle(v6, v8, v7);
        builder.AddTriangle(v7, v8, v9);
        builder.AddTriangle(v8, v10, v9);
        builder.AddTriangle(v9, v10, v11);
        builder.AddTriangle(v10, v12, v11);
        builder.AddTriangle(v11, v12, v13);
        builder.AddTriangle(v12, v14, v13);
        builder.AddTriangle(v13, v14, v15);
        builder.AddTriangle(v14, v16, v15);
        builder.AddTriangle(v15, v16, v17);
        builder.AddTriangle(v16, v18, v17);
        
    }

    public void build()
    {
        GetComponent<MeshFilter> ().mesh = builder.CreateMesh (true);
    }

    public void resetBuilder()
    {
        builder = new MeshBuilder ();
        builder.Clear ();
    }
}
