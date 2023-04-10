using MauiSlidePuzzle.Controllers;
using MauiSlidePuzzle.Models;
using System.Reflection;
using System.Reflection.Metadata;
//using Windows.Storage.Pickers.Provider;

namespace MauiSlidePuzzle;

public partial class MainPage : ContentPage
{
	public static readonly BindableProperty CurrentStageInfoProperty = BindableProperty.Create(nameof(CurrentStageInfo), typeof(StageInfo), typeof(MainPage), null);

	public StageInfo CurrentStageInfo
	{
		get => (StageInfo)GetValue(CurrentStageInfoProperty);
		set => SetValue(CurrentStageInfoProperty, value);
	}

	readonly SlidePuzzleController _controller;

	readonly List<StageInfo> _stageList = new();

	CancellationTokenSource _cancellationTokenSource; // = new CancellationTokenSource();
	CancellationToken _cancellationToken => _cancellationTokenSource.Token;


	public MainPage()
	{
		InitializeComponent();

		BindingContext = this;

		// Initialize stage info
		_stageList.Add( new("Stage 1", "myshapes.png", 3, 3) );
		_stageList.Add( new("Stage 2", "puzzle.png", 6, 6) );

		CurrentStageInfo = _stageList.First();


		//PuzzleTitleView.BindingContext = CurrentStageInfo;
		//Hoge.Text = CurrentStageInfo.StageName;
		//Hoge.BindingContext = CurrentStageInfo;

		//var resourceHelper = PuzzleResourceHelper.Instance;

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
