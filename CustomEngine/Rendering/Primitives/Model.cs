using System.Numerics;
using CustomEngine.Core.Rendering.Primitives;
using SharpGLTF.Schema2;
using Silk.NET.OpenGL;
using Mesh = CustomEngine.Core.Rendering.Primitives.Mesh;

namespace CustomEngine.Core.Rendering;

public class Model : IDrawable
{
    private readonly List<Mesh> _meshes = [];
    private readonly GL _gl;
    
    public Model(string path, GL gl)
    {
        _gl = gl;
        LoadModal(path);
    }
    
    private void LoadModal(string path)
    {
        var model = ModelRoot.Load(path);
        
        ProcessNodes(model.DefaultScene.VisualChildren);
    }

    private void ProcessNodes(IEnumerable<Node> nodes)
    {
        var enumerator = nodes.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            var node = enumerator.Current;
            
            if (node.Mesh != null)
            {
                var mesh = ProcessMesh(node.Mesh);
                mesh.SetupMesh();
                _meshes.Add(mesh);
            }
            ProcessNodes(node.VisualChildren);
        }
        
        enumerator.Dispose();
    }
    
    private Mesh ProcessMesh(SharpGLTF.Schema2.Mesh mesh)
    {
        var vertices = new List<Vertex>();
        var vertexBuffer = new List<float>();
        var indices = new List<uint>();
        
        foreach (var primitive in mesh.Primitives)
        {
            // Check if the POSITION attribute is present
            if (!primitive.VertexAccessors.ContainsKey("POSITION"))
            {
                throw new Exception("POSITION attribute is missing!");
            }
            var positions = primitive.GetVertexAccessor("POSITION").AsVector3Array();
            
            // Check if the NORMAL attribute is present
            IList<Vector3> normals = new List<Vector3>(); 
            if (primitive.VertexAccessors.ContainsKey("NORMAL"))
            {
                normals = primitive.GetVertexAccessor("NORMAL").AsVector3Array();
            }
            
            // Check if the TEXCOORD_0 attribute is present
            IList<Vector2> texCoords = new List<Vector2>();
            if (primitive.VertexAccessors.ContainsKey("TEXCOORD_0"))
            {
                texCoords = primitive.GetVertexAccessor("TEXCOORD_0").AsVector2Array();
            }

            for (var i = 0; i < positions.Count; i++)
            {
                var normal = normals.Count > 0 ? normals[i] : Vector3.Zero;
                var texCoord = texCoords.Count > 0 ? texCoords[i] : Vector2.Zero;
                var vertex = new Vertex(positions[i], normal, texCoord);
                vertices.Add(vertex);
            }
            
            // Check if we have indices, if so, add them
            if (primitive.IndexAccessor != null)
            {
                indices.AddRange(primitive.GetIndices());
            }
            
            // TODO: Textures
            // Note: if material is undefined the default material must be used.
        }
        
        return new Mesh(vertices.ToArray(), indices.ToArray(), _gl);
    }

    public void Draw(Shader shader)
    {
        foreach (var mesh in _meshes)
        {
            mesh.Draw(shader);
        }
    }
    
    public void Delete()
    {
        foreach (var mesh in _meshes)
        {
            mesh.Delete();
        }
    }
}