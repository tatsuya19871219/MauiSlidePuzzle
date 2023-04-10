using SkiaSharp;

namespace MauiSlidePuzzle.CustomViews;

internal class ImagePanelView : SlidePanelView
{
	internal ImagePanelView(SKImage skImage, RectF clipRect, int id) : base(skImage, clipRect, id) { }

	internal override void DrawPanelFrame(ICanvas canvas, RectF clipRect)
	{
		canvas.StrokeColor = Colors.Black;
		canvas.StrokeSize = 5;
		
		canvas.Alpha = 96;
		canvas.DrawRectangle(clipRect);
	}

	async internal override Task MoveTo(Point point, uint length)
    {
		_isMoving = true;
		await this.TranslateTo(point.X, point.Y, length);
		_isMoving = false;
    }

}