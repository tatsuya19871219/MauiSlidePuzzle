using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace MauiSlidePuzzle.CustomViews;

public partial class ClipImagePanel : ContentView
{
	public SKImage Image {get; init;}
	public RectF ClipRect {get; init;}

	//public Color BackgroundColor {get}

	readonly SKCanvasView _skiaView;

	public ClipImagePanel()
	{
		_skiaView = new SKCanvasView() { IgnorePixelScaling = true }; 
		_skiaView.PaintSurface += OnPaintSurface;

		Content = _skiaView;

		//this.BackgroundColor = Colors.White;
	}

	void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
	{
		var canvas = e.Surface.Canvas;

		//canvas.Clear(SKColors.Pink);
		//canvas.Clear(SKColors.Transparent);
		canvas.Clear();

		var sourceRect = new SKRect(ClipRect.Left, ClipRect.Top, ClipRect.Right, ClipRect.Bottom);
		var destRect = new SKRect(0, 0, ClipRect.Width, ClipRect.Height);

		canvas.DrawImage(Image, sourceRect, destRect);

		//canvas.DrawImage(Image, 0, 0);

	}

}