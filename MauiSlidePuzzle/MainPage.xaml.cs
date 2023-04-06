using MauiSlidePuzzle.Controllers;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle;

public partial class MainPage : ContentPage
{

	readonly SlidePuzzleController _controller;
	
	public MainPage()
	{
		InitializeComponent();

		//InitializePuzzle();

		var puzzle = new SlidePuzzle(3, 3);

		_controller = new SlidePuzzleController(MyPuzzleView, puzzle);
	}

	// async void InitializePuzzle()
	// {
	// 	var puzzle = new SlidePuzzle(3, 3);

	// 	//await MyPuzzle.InitializeAsync(3, 3);

	// 	//MyPuzzle.ShuffleAsyns();
	// }

}

