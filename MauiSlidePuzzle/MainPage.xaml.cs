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


	readonly List<StageInfo> _stageList = new();
	int _indexCurrentStage;

	SlidePuzzleController _controller;


	CancellationTokenSource _cancellationTokenSource; // = new CancellationTokenSource();
	CancellationToken _cancellationToken => _cancellationTokenSource.Token;


	public MainPage()
	{
		InitializeComponent();

		BindingContext = this;

		// Initialize stage info
		_stageList.Add( new("Stage 1", "myshapes.png", 3, 3) );
		_stageList.Add( new("Stage 2", "sky_and_stars.png", 3, 3, 25) );
		_stageList.Add( new("Stage 3", "guruguru.png", 4, 4, 30) );
		_stageList.Add( new("Stage 4", "puzzle.png", 6, 6, 50) );

		CurrentStageInfo = _stageList[_indexCurrentStage = 0];

		PrepareNewStage();

}


	void PrepareNewStage()
	{
		if (CurrentStageInfo is null) throw new Exception("Stage info is not set yet");

		_cancellationTokenSource?.Cancel();
		ResetButton.IsVisible = false;

		//MainThread.BeginInvokeOnMainThread(() =>
		//{
		//          CurrentStageImage.IsVisible = true;
		//	CurrentStageImage.Source = ImageSource.FromResource(CurrentStageInfo.EmbeddedImagePath);
		//});

		CurrentStageImage.IsVisible = true;
		CurrentStageImage.Source = CurrentStageInfo.ImageFilename;

		//var isMainThread = MainThread.IsMainThread;

        MyPuzzleView.ClearImageSource();
		MyPuzzleView.SetImageSource(CurrentStageInfo.EmbeddedImagePath);

		var puzzle = new SlidePuzzle(CurrentStageInfo.Rows, CurrentStageInfo.Columns);

		_controller = new SlidePuzzleController(MyPuzzleView, puzzle);

		_controller.OnReady += () =>
		{
			StartButton.IsVisible = true;

            //CurrentStageImage.IsVisible = true;
            //CurrentStageImage.Source = ImageSource.FromResource(CurrentStageInfo.EmbeddedImagePath); 
            // this throw the exception 
            // "Only the original thread that created a view hierarchy can touch its views." in android
        };

		_controller.OnCompleted += () =>
		{
			CompletedMessage.IsVisible = true;
			ResetButton.IsVisible = false;

			CurrentStageImage.IsVisible = true;

			_cancellationTokenSource?.Cancel();

			// Retart 
			_controller.OnReady?.Invoke();
		};

		_controller.Initialize();

	}

	// UI callbacks
	async void StartClicked(object sender, EventArgs e)
	{
		CurrentStageImage.IsVisible = false;
		CompletedMessage.IsVisible = false;
		StartButton.IsVisible = false;
		await _controller.ShuffleAsync(CurrentStageInfo.ShuffleCounts);

		await ShowResetButtonAsync();
	}

	async void ResetClicked(object sender, EventArgs e)
	{
		ResetButton.IsVisible = false;

		await _controller.ResetAsync();

		await ShowResetButtonAsync();
	}

	void PrevStageClicked(object sender, EventArgs e)
	{
		if (_indexCurrentStage == 0) return;

        //MyPuzzleView.Clear();

        CurrentStageInfo = _stageList[--_indexCurrentStage];

		PrepareNewStage();
	}

	void NextStageClicked(object sender, EventArgs e)
	{
		if (_indexCurrentStage == _stageList.Count-1) return;

        //MyPuzzleView.Clear();

        CurrentStageInfo = _stageList[++_indexCurrentStage];

		PrepareNewStage();
	}

	void ReloadStageClicked(object sender, EventArgs e)
	{
		PrepareNewStage();
	}

	// UI animations
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
			ResetCancellationToken();
			// _cancellationTokenSource?.Dispose();
			// _cancellationTokenSource = null;
		}

		ResetButton.IsVisible = true;
	}

	void ResetCancellationToken()
	{
		//_cancellationTokenSource?.Cancel();
		_cancellationTokenSource?.Dispose();
		_cancellationTokenSource = null;
		ResetButton.IsVisible = false;
	}

}
