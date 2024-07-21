using CustomEngine.Rendering.Shaders;

namespace CustomEngine.Rendering.Primitives;

public interface IDrawable
{
    void Draw(Shader shader);
    void Delete();
}