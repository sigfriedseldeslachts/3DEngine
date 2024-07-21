using CustomEngine.Game;
using CustomEngine.Rendering;
using Silk.NET.Maths;
using Silk.NET.Windowing;

class Game
{
    private static IWindow window;

    private static void Main(string[] args)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1280, 720);
        options.Title = "Custom Engine";
        options.API = new GraphicsAPI { API = ContextAPI.OpenGL, Profile = ContextProfile.Core, Version = new APIVersion(4, 6) };
        
        // Create the Silk.NET window
        window = Window.Create(options);
        
        // Create a game loop and render loop
        var gameLoop = new GameLoop(window);
        var renderLoop = new RenderLoop(window, gameLoop);
        
        // Assign events
        window.Load += gameLoop.OnLoad;
        window.Load += renderLoop.OnLoad;
        window.Update += gameLoop.OnUpdate;
        window.Render += renderLoop.OnRender;
        window.FramebufferResize += renderLoop.OnResize;
        window.Closing += renderLoop.OnClose;
        
        window.Run();
        window.Dispose();
    }
}