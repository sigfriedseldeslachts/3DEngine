using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Shader = CustomEngine.Rendering.Shaders.Shader;

namespace CustomEngine.Rendering.Shaders;

public class DefaultShader : Shader
{
    public DefaultShader(IWindow window, string vertexPath, string fragmentPath) : base(window)
    {
        // Load the vertex and fragment shaders
        var vertexShader = CreateShader(LoadShader(vertexPath), ShaderType.VertexShader);
        var fragmentShader = CreateShader(LoadShader(fragmentPath), ShaderType.FragmentShader);
        
        // Compile the shaders
        ShaderProgId = Gl.CreateProgram();
        Gl.AttachShader(ShaderProgId, vertexShader);
        Gl.AttachShader(ShaderProgId, fragmentShader);
        Gl.LinkProgram(ShaderProgId);
        
        // Delete the shader, we don't need it anymore
        Gl.DeleteShader(vertexShader);
        Gl.DeleteShader(fragmentShader);
        
        CheckForShaderErrors();
    }
}