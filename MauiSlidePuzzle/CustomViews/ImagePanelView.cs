using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace MauiSlidePuzzle.CustomViews;

internal class ImagePanelView : SlidePanelView
{

	internal ImagePanelView(Microsoft.Maui.Graphics.IImage image, RectF clipRect, int id) : base(image, clipRect, id)
	{
	}

	internal override void DrawPanelFrame(ICanvas canvas, RectF clipRect)
	{
		canvas.StrokeColor = Colors.Black;
		canvas.StrokeSize = 10;
		
		canvas.Alpha = 128;
		canvas.DrawRectangle(clipRect);
	}

	//public override void SetTappedNotifier(Action<SlidePanelView> tapped)
	//{
	//	AddTapRecognizer(tapped);
	//}
  
	async internal override Task MoveTo(Point point, uint length)
    {
		_isMoving = true;
		await this.TranslateTo(point.X, point.Y, length);
		_isMoving = false;
    }

}