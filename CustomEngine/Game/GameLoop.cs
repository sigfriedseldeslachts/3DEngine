using System.Numerics;
using Silk.NET.GLFW;
using Silk.NET.Input;
using Silk.NET.Windowing;

namespace CustomEngine.Core.Game;

public class GameLoop(IWindow window)
{
    private static IKeyboard primaryKeyboard;
    
    private Matrix4x4 _projection;
    private Matrix4x4 _view;
    private Matrix4x4 _model;
    
    public Vector3 _cameraPosition { get; private set; } = new Vector3(0, 0, 3);
    public Vector3 _cameraFront = new Vector3(0, 0, -1);
    public Vector3 _cameraUp = Vector3.UnitY;
    public Vector3 _cameraDirection = Vector3.Zero;
    
    
    public void OnLoad()
    {
        IInputContext input = window.CreateInput();
        primaryKeyboard = input.Keyboards.FirstOrDefault();
    }

    public void OnUpdate(double deltaTime)
    {
        var moveSpeed = 2.5f * (float) deltaTime;

        if (primaryKeyboard.IsKeyPressed(Key.W))
        {
            //Move forwards
            _cameraPosition += moveSpeed * _cameraFront;
        }
        if (primaryKeyboard.IsKeyPressed(Key.S))
        {
            //Move backwards
            _cameraPosition -= moveSpeed * _cameraFront;
        }
        if (primaryKeyboard.IsKeyPressed(Key.A))
        {
            //Move left
            _cameraPosition -= Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * moveSpeed;
        }
        if (primaryKeyboard.IsKeyPressed(Key.D))
        {
            //Move right
            _cameraPosition += Vector3.Normalize(Vector3.Cross(_cameraFront, _cameraUp)) * moveSpeed;
        }
    }
    
    public void OnKeyDown(Key key, int code, KeyModifiers mods)
    {
        
    }
}