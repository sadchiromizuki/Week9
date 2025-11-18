using UnityEngine;

public class PyramidGL : MonoBehaviour
{
    public Material material;

    public float size = 1f;
    public Vector2 position;
    public float zPos;

    public float rotX;
    public float rotY;
    public float rotZ;

    private void OnPostRender()
    {
        if (material == null)
        {
            Debug.LogError("");
            return;
        }

        GL.PushMatrix();
        material.SetPass(0);
        GL.Begin(GL.LINES);

        DrawPyramid();

        GL.End();
        GL.PopMatrix();
    }

    void DrawPyramid()
    {
        float half = size * 0.5f;

    
        Vector3 apex = new Vector3(0, size, 0);
        Vector3[] baseVerts = new Vector3[]
        {
            new Vector3(-half, -half, -half),
            new Vector3(+half, -half, -half),
            new Vector3(+half, -half, +half),
            new Vector3(-half, -half, +half),
        };

  
        apex = ApplyRotation(apex);
        for (int i = 0; i < 4; i++)
            baseVerts[i] = ApplyRotation(baseVerts[i]);
 
        apex += new Vector3(position.x, position.y, zPos);
        for (int i = 0; i < 4; i++)
            baseVerts[i] += new Vector3(position.x, position.y, zPos);

        for (int i = 0; i < 4; i++)
        {
            DrawLine(baseVerts[i], baseVerts[(i + 1) % 4]);
        }

        for (int i = 0; i < 4; i++)
        {
            DrawLine(baseVerts[i], apex);
        }
    }

    Vector3 ApplyRotation(Vector3 p)
    {
        float rx = rotX * Mathf.Deg2Rad;
        float ry = rotY * Mathf.Deg2Rad;
        float rz = rotZ * Mathf.Deg2Rad;


        p = new Vector3(
            p.x,
            p.y * Mathf.Cos(rx) - p.z * Mathf.Sin(rx),
            p.y * Mathf.Sin(rx) + p.z * Mathf.Cos(rx)
        );

 
        p = new Vector3(
            p.x * Mathf.Cos(ry) + p.z * Mathf.Sin(ry),
            p.y,
            -p.x * Mathf.Sin(ry) + p.z * Mathf.Cos(ry)
        );


        p = new Vector3(
            p.x * Mathf.Cos(rz) - p.y * Mathf.Sin(rz),
            p.x * Mathf.Sin(rz) + p.y * Mathf.Cos(rz),
            p.z
        );

        return p;
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        float s = PerspectiveCamera.Instance.GetPerspective(start.z);
        float e = PerspectiveCamera.Instance.GetPerspective(end.z);

        GL.Vertex(new Vector3(start.x * s, start.y * s, 0));
        GL.Vertex(new Vector3(end.x * e, end.y * e, 0));
    }
}
