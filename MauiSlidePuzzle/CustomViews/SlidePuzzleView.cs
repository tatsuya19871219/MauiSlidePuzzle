using SkiaSharp;

using System.Reflection;
using System.Runtime.CompilerServices;

using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.CustomViews;

public class SlidePuzzleView : ContentView
{
	//public static readonly BindableProperty EmbeddedImageSourceProperty = BindableProperty.Create(nameof(EmbeddedImageSource), typeof(string), typeof(SlidePuzzleView), null);
	//public string EmbeddedImageSource
	//{
	//	get => (string)GetValue(EmbeddedImageSourceProperty);
	//	set => SetValue(EmbeddedImageSourceProperty, value);
	//}

//	string _embeddedImageSource;

	internal List<SlidePanelView> PanelViews => _panelViews;
	internal BlankPanelView BlankPanelView {get; private set;}

	SKImage _skImage;

	double _panelWidth;
	double _panelHeight;

	readonly Grid _grid;
	readonly List<SlidePanelView> _panelViews = new();

	readonly double _width;
	readonly double _height;

	//public SlidePuzzleView()
	public SlidePuzzleView(double width, double height)
	{
		Content = _grid = new Grid();

		this.WidthRequest = _width = width;
		this.HeightRequest = _height = height;

	}

    internal void SetImageSource(string embeddedImageSource)
	{
		// Clear
		_skImage?.Dispose();
		_skImage = null;

		var helper = PuzzleResourceHelper.Instance;

		using (Stream stream = helper.GetEmbededResourceStream(embeddedImageSource))
		{
			SKBitmap bitmap = SKBitmap.Decode(stream);
			_skImage = SKImage.FromBitmap(bitmap.Resize(new SKImageInfo((int)_width, (int)_height), SKFilterQuality.Medium));
		}
    }

    // void ClearImageSource()
	// {
	// 	_skImage?.Dispose();
	// 	_skImage = null;

	// 	_panelViews.Clear(); // Need this!
	// }

	internal async void Initialize(SlidePuzzle model)
	{	
		while (true)
		{
			if (_skImage is not null) break;
			await Task.Delay(100);
		}

		_grid.Clear();
		_panelViews.Clear();

		int rows = model.Rows;
		int columns = model.Columns;

		double width = _skImage.Width;
		double height = _skImage.Height;

		_panelWidth = width / columns;
		_panelHeight = height / rows;

		foreach (var panel in model.Panels)
		{
			int id = panel.ID;
			Point loc = panel.Location;
			Point translation = ConvertLocToTrans(loc);			
			RectF clipRect = new Rect(translation, new Size(_panelWidth, _panelHeight));

			SlidePanelView panelView;

			if (panel == model.BlankPanel)
				panelView = BlankPanelView
							= new BlankPanelView(_skImage, clipRect, id);
			else
				panelView = new ImagePanelView(_skImage, clipRect, id);

			_grid.Add( panelView as IView );

			_panelViews.Add( panelView );

		}

	}

	internal Point ConvertLocToTrans(Point location)
		=> new Point(location.X*_panelWidth, location.Y*_panelHeight);
    
}