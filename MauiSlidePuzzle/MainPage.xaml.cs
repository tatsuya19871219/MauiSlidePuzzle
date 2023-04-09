using MauiSlidePuzzle.Controllers;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle;

public partial class MainPage : ContentPage
{
	public static BindableProperty ImageFilenameProperty = BindableProperty.Create(nameof(ImageFilename), typeof(string), typeof(MainPage), null);
	
	public string ImageFilename
	{
		get => (string)GetValue(ImageFilenameProperty);
		set => SetValue(ImageFilenameProperty, value);
	}

	readonly SlidePuzzleController _controller;

	CancellationTokenSource _cancellationTokenSource; // = new CancellationTokenSource();
	CancellationToken _cancellationToken => _cancellationTokenSource.Token;
	
	public MainPage()
	{
		InitializeComponent();

		var puzzle = new SlidePuzzle(3, 3);

		_controller = new SlidePuzzleController(MyPuzzleView, puzzle);

		_controller.OnReady += () =>
		{
			StartButton.IsVisible = true;
		};

		_controller.OnCompleted += () =>
		{
			CompletedMessage.IsVisible = true;
			ResetButton.IsVisible = false;

			_cancellationTokenSource?.Cancel();

			// Retart 
			_controller.OnReady?.Invoke();
		};

		_controller.Initialize();

		StartButton.Clicked += async (s, e) =>
		{
			CompletedMessage.IsVisible = false;
			StartButton.IsVisible = false;
			await _controller.ShuffleAsync();

			await ShowResetButtonAsync();
		};

		ResetButton.Clicked += async (s, e) =>
		{
			ResetButton.IsVisible = false;

			await _controller.ResetAsync();

			await ShowResetButtonAsync();
		};

	}

	async Task ShowResetButtonAsync(int milliseconds = 5000)
	{
		_cancellationTokenSource = new CancellationTokenSource();

		try
		{
			await Task.Delay(milliseconds, _cancellationToken);
		}
		catch (OperationCanceledException ex)
		{
			// Need not to show reset button
			return;
		}
		finally
		{
			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
		}

		ResetButton.IsVisible = true;
	}

}
