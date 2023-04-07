namespace MauiSlidePuzzle.CustomViews;

internal class BlankPanelView : SlidePanelView
{
	internal BlankPanelView(Microsoft.Maui.Graphics.IImage image, RectF clipRect, int id) : base(image, clipRect, id)
	{
	}

	internal override void DrawPanelFrame(ICanvas canvas, RectF clipRect)
    {
        canvas.FillColor = Colors.LightGray;

		canvas.FillRectangle(clipRect);
    }

    async internal override Task MoveTo(Point point, uint length)
    {
        _isMoving = true;
        this.IsVisible = false;

        await this.TranslateTo(point.X, point.Y, 0);

        this.IsVisible = true;
        _isMoving = false;
    }

}