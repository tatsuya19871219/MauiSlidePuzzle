using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSlidePuzzle.CustomViews;

internal class PanelDrawable : IDrawable
{
    readonly Microsoft.Maui.Graphics.IImage _image;
    readonly RectF _clipRect;

    public Action<ICanvas, RectF> DrawPanelFrame;

    internal PanelDrawable(Microsoft.Maui.Graphics.IImage image, RectF clipRect)
    {
        _image = image;
        _clipRect = clipRect;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {

        canvas.Translate((float)-_clipRect.Left, (float)-_clipRect.Top);

        PathF path = new PathF();
        path.AppendRectangle(_clipRect);
        canvas.ClipPath(path);
        // In windows, the clipping throws the following error message :
        // After calling CanvasDrawingSession.CreateLayer, you must close the resulting CanvasActiveLayer before ending the CanvasDrawingSession.

        if (_image is not null)
            canvas.DrawImage(_image, 0, 0, _image.Width, _image.Height);

        DrawPanelFrame?.Invoke(canvas, _clipRect);

    }
}
