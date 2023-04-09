namespace MauiSlidePuzzle.CustomViews;

internal class PanelFrameDrawable : IDrawable
{
    readonly RectF _clipRect;
    public Action<ICanvas, RectF> DrawFrame;

    internal PanelFrameDrawable(RectF clipRect)
    {
        _clipRect = clipRect;
    }
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.Translate((float) -_clipRect.Left, (float) -_clipRect.Top);

        DrawFrame?.Invoke(canvas, _clipRect);
    }
}
