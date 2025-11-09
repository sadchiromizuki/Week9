using UnityEngine;

public class PyramidGL : MonoBehaviour
{
    public Material material;
    public float size = 1f;
    public Vector2 position;
    public float zPos;

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
        Vector3 apex = new Vector3(position.x, position.y + size, zPos);
        Vector3[] baseVerts = new Vector3[]
        {
            new Vector3(position.x - half, position.y - half, zPos - half),
            new Vector3(position.x + half, position.y - half, zPos - half),
            new Vector3(position.x + half, position.y - half, zPos + half),
            new Vector3(position.x - half, position.y - half, zPos + half)
        };

        for (int i = 0; i < 4; i++)
        {
            DrawLine(baseVerts[i], baseVerts[(i + 1) % 4]);
        }

        for (int i = 0; i < 4; i++)
        {
            DrawLine(baseVerts[i], apex);
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        float sPersp = PerspectiveCamera.Instance.GetPerspective(start.z);
        float ePersp = PerspectiveCamera.Instance.GetPerspective(end.z);
        GL.Vertex(new Vector3(start.x * sPersp, start.y * sPersp, 0));
        GL.Vertex(new Vector3(end.x * ePersp, end.y * ePersp, 0));
    }
}
