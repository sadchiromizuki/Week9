using UnityEngine;

public class CylinderGL : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 2f;
    [Range(6, 64)] public int segments = 10;
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
        DrawCylinder();
        GL.End();
        GL.PopMatrix();
    }

    void DrawCylinder()
    {
        Vector3[] top = new Vector3[segments];
        Vector3[] bottom = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.PI * 2f * i / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            top[i] = new Vector3(x, height / 2f, z);
            bottom[i] = new Vector3(x, -height / 2f, z);

            top[i] = ApplyRotation(top[i]);
            bottom[i] = ApplyRotation(bottom[i]);

            top[i] += position;
            bottom[i] += position;
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            DrawLine(top[i], top[next]);
            DrawLine(bottom[i], bottom[next]);
            DrawLine(top[i], bottom[i]);
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

    void DrawLine(Vector3 a, Vector3 b)
    {
        float ap = PerspectiveCamera.Instance.GetPerspective(a.z);
        float bp = PerspectiveCamera.Instance.GetPerspective(b.z);
        GL.Vertex(new Vector3(a.x * ap, a.y * ap, 0));
        GL.Vertex(new Vector3(b.x * bp, b.y * bp, 0));
    }
}
