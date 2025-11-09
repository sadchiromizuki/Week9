using UnityEngine;

public class CylinderGL : MonoBehaviour
{
    public Material material;
    public float radius = 1f;
    public float height = 2f;
    [Range(6, 64)] public int segments = 10;
    public Vector3 position;

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
            top[i] = position + new Vector3(x, height / 2f, z);
            bottom[i] = position + new Vector3(x, -height / 2f, z);
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            DrawLine(top[i], top[next]);
            DrawLine(bottom[i], bottom[next]);
            DrawLine(top[i], bottom[i]);
        }
    }

    void DrawLine(Vector3 a, Vector3 b)
    {
        float ap = PerspectiveCamera.Instance.GetPerspective(a.z);
        float bp = PerspectiveCamera.Instance.GetPerspective(b.z);
        GL.Vertex(new Vector3(a.x * ap, a.y * ap, 0));
        GL.Vertex(new Vector3(b.x * bp, b.y * bp, 0));
    }
}
