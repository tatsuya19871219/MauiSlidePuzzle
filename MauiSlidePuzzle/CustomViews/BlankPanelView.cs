namespace MauiSlidePuzzle.CustomViews;

internal class BlankPanelView : SlidePanelView
{
	internal BlankPanelView(Microsoft.Maui.Graphics.IImage image, RectF clipRect, int id) : base(image, clipRect, id)
	{
	}

	internal override void DrawPanelFrame(ICanvas canvas, RectF clipRect)
    {
        canvas.FillColor = Colors.LightGray;
		// canvas.StrokeColor = Colors.Black;
		// canvas.StrokeSize = 10;

		canvas.FillRectangle(clipRect);
    }

    public override void SetTappedNotifier(Action<SlidePanelView> tapped)
    {
        // just ignore the tap
    }

}