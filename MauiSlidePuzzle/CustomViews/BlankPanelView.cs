using SkiaSharp;

namespace MauiSlidePuzzle.CustomViews;

internal class BlankPanelView : SlidePanelView
{
    internal BlankPanelView(SKImage skImage, RectF clipRect, int id) : base(skImage, clipRect, id) { }

	internal override void DrawPanelFrame(ICanvas canvas, RectF clipRect)
    {
        canvas.FillColor = this.BackgroundColor; //Colors.LightGray;

		canvas.FillRectangle(clipRect);
    }

    async internal override Task MoveTo(Point point, uint length)
    {
        _isMoving = true;
        //this.IsVisible = false;

        this.TranslateTo(point.X, point.Y, 0);

        this.Opacity = 0;

        await this.FadeTo(1, length);

        //this.IsVisible = true;
        _isMoving = false;
    }

}