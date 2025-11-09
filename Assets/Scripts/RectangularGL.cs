using UnityEngine;

public class RectangularColumnGL : MonoBehaviour
{
    public Material material;
    public Vector3 size = new Vector3(1, 2, 1);
    public Vector3 position;

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
            corners[i] = position + new Vector3(
                (i & 1) == 0 ? -size.x * 0.5f : size.x * 0.5f,
                (i & 2) == 0 ? -size.y * 0.5f : size.y * 0.5f,
                (i & 4) == 0 ? -size.z * 0.5f : size.z * 0.5f
            );
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

    void DrawLine(Vector3 a, Vector3 b)
    {
        float ap = PerspectiveCamera.Instance.GetPerspective(a.z);
        float bp = PerspectiveCamera.Instance.GetPerspective(b.z);
        GL.Vertex(new Vector3(a.x * ap, a.y * ap, 0));
        GL.Vertex(new Vector3(b.x * bp, b.y * bp, 0));
    }
}
