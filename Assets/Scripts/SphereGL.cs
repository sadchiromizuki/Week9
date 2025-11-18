using UnityEngine;

public class SphereGL : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    [Range(6, 64)] public int segments = 12;

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

        DrawSphere();

        GL.End();
        GL.PopMatrix();
    }

    void DrawSphere()
    {
        for (int lat = 0; lat < segments; lat++)
        {
            float theta1 = Mathf.PI * lat / segments;
            float theta2 = Mathf.PI * (lat + 1) / segments;

            for (int lon = 0; lon < segments; lon++)
            {
                float phi1 = 2f * Mathf.PI * lon / segments;
                float phi2 = 2f * Mathf.PI * (lon + 1) / segments;

                Vector3 p1 = GetSpherePoint(theta1, phi1);
                Vector3 p2 = GetSpherePoint(theta1, phi2);
                Vector3 p3 = GetSpherePoint(theta2, phi1);

                p1 = ApplyRotation(p1);
                p2 = ApplyRotation(p2);
                p3 = ApplyRotation(p3);

                p1 += position;
                p2 += position;
                p3 += position;

                DrawLine(p1, p2);
                DrawLine(p1, p3);
            }
        }
    }

    Vector3 GetSpherePoint(float theta, float phi)
    {
        return new Vector3(
            radius * Mathf.Sin(theta) * Mathf.Cos(phi),
            radius * Mathf.Cos(theta),
            radius * Mathf.Sin(theta) * Mathf.Sin(phi)
        );
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
