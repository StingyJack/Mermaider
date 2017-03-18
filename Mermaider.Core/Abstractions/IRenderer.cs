namespace Mermaider.Core.Abstractions
{
    public interface IRenderer
    {
        RenderResult RenderAsSvg(string fileName, string graphText);
        RenderResult RenderAsImage(string fileName, string graphText);
    }
}
