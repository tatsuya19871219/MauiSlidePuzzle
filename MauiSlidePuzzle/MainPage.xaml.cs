using MauiSlidePuzzle.Controllers;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle;

public partial class MainPage : ContentPage
{
	readonly SlidePuzzleController _controller;
	
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

			// Retart 
			_controller.OnReady?.Invoke();
		};

		_controller.Initialize();

		StartButton.Clicked += async (s, e) =>
		{
			CompletedMessage.IsVisible = false;
			StartButton.IsVisible = false;
			await _controller.ShuffleAsync();
		};

	}

}
