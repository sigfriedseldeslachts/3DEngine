using System.Numerics;
using System.Runtime.InteropServices;

namespace CustomEngine.Rendering.Primitives;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Vertex
{
    public readonly Vector3 Position;
    public readonly Vector3 Normal;
    public readonly Vector2 TexCoords;
    
    public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords)
    {
        Position = position;
        Normal = normal;
        TexCoords = texCoords;
    }
}