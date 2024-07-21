using Silk.NET.OpenGL;
using Shader = CustomEngine.Rendering.Shaders.Shader;

namespace CustomEngine.Rendering.Primitives;

public class Mesh(Vertex[] vertices, uint[] indices, GL gl) : IDrawable
{
    private GL _gl = gl;
    private bool _hasIndices = indices.Length > 0;
    private uint _vao, _vbo, _ebo;
    
    

    public unsafe void SetupMesh()
    {
        // Creating a vertex array object
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);

        // Initializing a vertex buffer that holds the vertex data.
        _vbo = _gl.GenBuffer(); // Creating the buffer.
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo); // Binding the buffer.
        fixed (void* v = vertices)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * 32), v, BufferUsageARB.StaticDraw); //Setting buffer data.
        }

        // Initializing a element buffer that holds the index data.
        if (_hasIndices)
        {
            _ebo = _gl.GenBuffer(); // Creating the buffer.
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo); // Binding the buffer.
            fixed (void* i = indices)
            {
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (indices.Length * sizeof(uint)), i, BufferUsageARB.StaticDraw); //Setting buffer data.
            }
        }
        
        // ========= Setting the vertex attribute pointers.
        // Position attribute
        _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), null);
        _gl.EnableVertexAttribArray(0);
        // Normal attribute
        _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (3 * sizeof(float)));
        _gl.EnableVertexAttribArray(1);
        // Texture attribute
        _gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (6 * sizeof(float)));
        _gl.EnableVertexAttribArray(2);
    }

    public unsafe void Draw(Shader shader)
    {
        gl.BindVertexArray(_vao);
        shader.Use();
        if (_hasIndices)
        {
            gl.DrawElements(GLEnum.Triangles, (uint) indices.Length, DrawElementsType.UnsignedInt, null);
        }
        else
        {
            gl.DrawArrays(GLEnum.Triangles, 0, (uint) vertices.Length);
        }
        gl.BindVertexArray(0); // Unbind the VAO
    }
    
    public void Delete()
    {
        _gl.DeleteBuffer(_vbo);
        _gl.DeleteBuffer(_ebo);
        _gl.DeleteVertexArray(_vao);
    }
}