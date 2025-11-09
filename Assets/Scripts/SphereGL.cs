using UnityEngine;

public class SphereGL : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    [Range(6, 64)] public int segments = 12;
    public Vector3 position;

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
            float theta1 = Mathf.PI * (float)lat / segments;
            float theta2 = Mathf.PI * (float)(lat + 1) / segments;

            for (int lon = 0; lon < segments; lon++)
            {
                float phi1 = 2f * Mathf.PI * (float)lon / segments;
                float phi2 = 2f * Mathf.PI * (float)(lon + 1) / segments;

                Vector3 p1 = GetSpherePoint(theta1, phi1);
                Vector3 p2 = GetSpherePoint(theta1, phi2);
                Vector3 p3 = GetSpherePoint(theta2, phi1);

                DrawLine(position + p1, position + p2);
                DrawLine(position + p1, position + p3);
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

    void DrawLine(Vector3 a, Vector3 b)
    {
        float ap = PerspectiveCamera.Instance.GetPerspective(a.z);
        float bp = PerspectiveCamera.Instance.GetPerspective(b.z);
        GL.Vertex(new Vector3(a.x * ap, a.y * ap, 0));
        GL.Vertex(new Vector3(b.x * bp, b.y * bp, 0));
    }
}
