namespace Mermaider.Core.Abstractions
{
    public interface IMermaidRenderer
    {
        MermaidRenderResult RenderAsSvg(string inputText);
        MermaidRenderResult RenderAsImage(string inputText);
    }
}
