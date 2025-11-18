using UnityEngine;

public class RectangularColumnGL : MonoBehaviour
{
    public Material material;
    public Vector3 size = new Vector3(1, 2, 1);

    public Vector3 position;

    public float rotX;
    public float rotY;
    public float rotZ;

    private void OnPostRender()
    {
        if (material == null) return;

        GL.PushMatrix();
        material.SetPass(0);
        GL.Begin(GL.LINES);

        DrawColumn();

        GL.End();
        GL.PopMatrix();
    }

    void DrawColumn()
    {
        Vector3[] corners = new Vector3[8];
        for (int i = 0; i < 8; i++)
        {
            corners[i] = new Vector3(
                (i & 1) == 0 ? -size.x * 0.5f : size.x * 0.5f,
                (i & 2) == 0 ? -size.y * 0.5f : size.y * 0.5f,
                (i & 4) == 0 ? -size.z * 0.5f : size.z * 0.5f
            );

            corners[i] = ApplyRotation(corners[i]);

            corners[i] += position;
        }

        int[,] edges = new int[,]
        {
            {0,1},{1,3},{3,2},{2,0},
            {4,5},{5,7},{7,6},{6,4}, 
            {0,4},{1,5},{2,6},{3,7}  
        };


        for (int i = 0; i < edges.GetLength(0); i++)
            DrawLine(corners[edges[i, 0]], corners[edges[i, 1]]);
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

    void DrawLine(Vector3 a, Vector3 b)
    {
        float ap = PerspectiveCamera.Instance.GetPerspective(a.z);
        float bp = PerspectiveCamera.Instance.GetPerspective(b.z);

        GL.Vertex(new Vector3(a.x * ap, a.y * ap, 0));
        GL.Vertex(new Vector3(b.x * bp, b.y * bp, 0));
    }
}
