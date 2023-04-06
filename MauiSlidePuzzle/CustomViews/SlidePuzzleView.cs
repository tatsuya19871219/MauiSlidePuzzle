#if WINDOWS
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics.Skia.Views;
using SkiaSharp;
#else
using Microsoft.Maui.Graphics.Platform;
#endif

using System.Reflection;
using System.Runtime.CompilerServices;

using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.CustomViews;

public class SlidePuzzleView : ContentView
{
	public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(FileImageSource), typeof(SlidePuzzleView), null);
	public FileImageSource Source
	{
		get => (FileImageSource)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	internal List<SlidePanelView> PanelViews => _panelViews;

	Microsoft.Maui.Graphics.IImage _image;
	//readonly List<ImagePanel> _panels;

	readonly Grid _grid;
	readonly List<SlidePanelView> _panelViews = new();

	//Dictionary<Point, Point> _indexToPositionDict = new();

	public SlidePuzzleView()
	{
		Content = _grid = new Grid();

		_grid.BackgroundColor = Colors.WhiteSmoke;
	}

	internal void Initialize(SlidePuzzle model)
	{
		if (Source is null) throw new Exception("Source is not set yet.");
		//if (rows <= 0 || columns <= 0) throw new ArgumentException("rows and columns should be positive numbers");
		
		_grid.Clear();

		int rows = model.Rows;
		int columns = model.Columns;

		double width = _image.Width;
		double height = _image.Height;

		double panelWidth = width / columns;
		double panelHeight = height / rows;

		foreach (var panel in model.Panels)
		{
			int id = panel.ID;
			Point loc = panel.Location;
			RectF clipRect = new Rect(loc.X*panelWidth, loc.Y*panelHeight, panelWidth, panelHeight);

			SlidePanelView panelView;

			if (panel == model.BlankPanel)
				panelView = new BlankPanelView(_image, clipRect, id);
			else
				panelView = new ImagePanelView(_image, clipRect, id);

			// panelView.HorizontalOptions = LayoutOptions.Start;
			// panelView.VerticalOptions = LayoutOptions.Start;

			// panelView.TranslationX = clipRect.X;
			// panelView.TranslationY = clipRect.Y;

			_grid.Add( panelView as IView );

			_panelViews.Add( panelView );
		}
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

		// Fit grid size to the view
		if (width > 0 && height > 0)
		{
			_grid.AnchorX = 0;
			_grid.AnchorY = 0;

			_grid.ScaleX = width / _image.Width;
			_grid.ScaleY = height / _image.Height;
		}
    }

    // async public Task ShuffleAsync(int count = 10)
	// {

	// } 

	protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

		switch (propertyName)
		{
			case nameof(Source):

				SetImageFromFileImageSource(Source);

				break;

			//default:

			//	throw new Exception("Unknown property name is given");
			
        }
	}

	internal void SetImageFromFileImageSource(FileImageSource source)
	{
		using ( Stream stream = GetStreamFromFileImageSource(source) ) 
		{
#if WINDOWS
			_image = SkiaImage.FromStream( stream );
#else
			_image = PlatformImage.FromStream( stream );
#endif
		}
	}

	Stream GetStreamFromFileImageSource(FileImageSource source)
	{
		var file = source.File;

		Assembly assembly = GetType().GetTypeInfo().Assembly;

		return assembly.GetManifestResourceStream($"MauiSlidePuzzle.Resources.Images.{file}");
	}


}