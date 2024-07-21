namespace CustomEngine.Core.Rendering;

public interface IDrawable
{
    void Draw(Shader shader);
    void Delete();
}