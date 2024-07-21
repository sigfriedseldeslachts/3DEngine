using System.Numerics;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace CustomEngine.Core.Rendering;

public abstract class Shader
{
    protected readonly GL Gl;
    public uint ShaderProgId { get; protected init; } = uint.MaxValue;

    protected Shader(IWindow window)
    {
        Gl = GL.GetApi(window);
    }

    public void Deconstruct()
    {
        Gl.DeleteProgram(ShaderProgId);
    }

    protected void CheckForShaderErrors()
    {
        Gl.GetProgram(ShaderProgId, GLEnum.LinkStatus, out var status);
        if (status != 0) return; // No error
        throw new Exception($"Error linking shader program\n{Gl.GetProgramInfoLog(ShaderProgId)}");
    }
    
    protected uint CreateShader(string data, ShaderType type)
    {
        var shader = Gl.CreateShader(type);
        Gl.ShaderSource(shader, data);
        Gl.CompileShader(shader);
        return shader;
    }
    
    public static string LoadShader(string path)
    {
        return File.ReadAllText(path);
    }
    
    public void Use()
    {
        Gl.UseProgram(ShaderProgId);
    }
    
    public unsafe void SetUniformMat3(string name, Matrix4x4 value)
    {
        var location = Gl.GetUniformLocation(ShaderProgId, name);
        if (location == -1) throw new Exception($"{name} uniform not found on shader.");
        Gl.UniformMatrix4(location, 1, false, (float*) &value);
    }
    
    public unsafe void SetUniformMat4(string name, Matrix4x4 value)
    {
        var location = Gl.GetUniformLocation(ShaderProgId, name);
        if (location == -1) throw new Exception($"{name} uniform not found on shader.");
        Gl.UniformMatrix4(location, 1, false, (float*) &value);
    }
}