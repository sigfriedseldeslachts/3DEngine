using System.Numerics;
using CustomEngine.Core.Game;
using CustomEngine.Core.Rendering;
using CustomEngine.Core.Rendering.Shaders;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Shader = CustomEngine.Core.Rendering.Shader;

namespace CustomEngine.Rendering;

public class RenderLoop(IWindow window, GameLoop gameLoop)
{
    private GL _gl = null!;
    private readonly List<IDrawable> _drawables = [];
    private Shader _shader;

    public unsafe void OnLoad()
    {
        _gl = GL.GetApi(window);
        _gl.Enable(GLEnum.DepthTest);
        
        //_gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Line);

        _shader = new DefaultShader(window, "/home/sigfried/Documents/Projects/GameDev/CustomEngine/CustomEngine/Resources/Shaders/default_shader.vert", "/home/sigfried/Documents/Projects/GameDev/CustomEngine/CustomEngine/Resources/Shaders/default_shader.frag");
        
        // Load first model
        var model = new Model("/home/sigfried/Documents/Projects/GameDev/CustomEngine/CustomEngine/Resources/Models/Fox.glb", _gl);
        AddDrawable(model);
    }

    public unsafe void OnRender(double obj)
    {
        //Clear the color channel.
        _gl.Clear((uint) ClearBufferMask.ColorBufferBit | (uint) ClearBufferMask.DepthBufferBit);
        _gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        
        var difference = (float) (window.Time * 100) * MathF.PI / 180;
        

        // Model matrix of 0.1f scale
        var model = Matrix4x4.CreateScale(0.05f);
        model.Translation = new Vector3(0, -2, 0);
        //var model = Matrix4x4.Identity;
        model = Matrix4x4.CreateRotationY(difference) * model;
        var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 4, 1280f / 720f, 0.1f, 100f);
        var view = Matrix4x4.CreateLookAt(gameLoop._cameraPosition, gameLoop._cameraPosition + gameLoop._cameraFront, gameLoop._cameraUp);
        
        _shader.SetUniformMat4("projection", projection);
        _shader.SetUniformMat4("view", view);
        _shader.SetUniformMat4("model", model);
        
        foreach (var drawable in _drawables)
        {
            drawable.Draw(_shader);
        }
    }
    
    public void AddDrawable(IDrawable drawable)
    {
        _drawables.Add(drawable);
    }
    
    public void OnClose()
    {
        foreach (var mesh in _drawables)
        {
            mesh.Delete();
        }
        
        _gl.DeleteProgram(_shader.ShaderProgId);
    }
    
    public void OnResize(Vector2D<int> size)
    {
        _gl.Viewport(size);
    }
}