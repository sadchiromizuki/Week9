using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 position;   
    public Vector3 size;       

    public bool IsColliding(LineGen cube)
    {
        CubeDimensions c = cube.GetWorldCubeDimensions();

        bool overlapX = c.maxX >= position.x - size.x && c.minX <= position.x + size.x;
        bool overlapY = c.minY <= position.y + size.y && c.maxY >= position.y - size.y;
        bool overlapZ = c.maxZ >= position.z - size.z && c.minZ <= position.z + size.z;

        return overlapX && overlapY && overlapZ;
    }
}
