using MauiSlidePuzzle.Controllers;
using MauiSlidePuzzle.Models;
using System.Text.Json;

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

    //IList<VisualStateGroup> _vsgList;

    public MainPage()
    {

        InitializeComponent();

        BindingContext = this;

        //_vsgList = VisualStateManager.GetVisualStateGroups(this);
        //_vsgList = VisualStateManager.GetVisualStateGroups(CompletedMessage);

        GoToAppState("Initialized");
        GoToElementState(CurrentStageImage, "Default");
        GoToElementState(CompletedMessage, "Default");

        // Load stages
        int k = 1;
        while (TryLoadStageInfo($"Stage{k++}.json", out var stageInfo))
            _stageList.Add(stageInfo);

        // _stageList.Add( new("Stage 1", "myshapes.png", 3, 3, 10) );
        // _stageList.Add( new("Stage 2", "sky_and_stars.png", 3, 3, 25) );
        // _stageList.Add( new("Stage 3", "amin.png", 4, 4, 25) );
        // _stageList.Add( new("Stage 4", "guruguru.png", 4, 4, 30) );
        // _stageList.Add( new("Stage 5", "puzzle.png", 6, 6, 50) );

        CurrentStageInfo = _stageList[_indexCurrentStage = 0];
        PrevStageButton.IsEnabled = false;

        PrepareNewStage();

    }

    void PrepareNewStage()
    {
        if (CurrentStageInfo is null) throw new Exception("Stage info is not set yet");

        _cancellationTokenSource?.Cancel();

        GoToAppState("OnGamePreparing");
        GoToElementState(CurrentStageImage, "OnGamePreparing");


        // Prepare a puzzle according to the CurrentStageInfo
        CurrentStageImage.Source = CurrentStageInfo.ImageFilename;

        MyPuzzleView.SetImageSource(CurrentStageInfo.EmbeddedImagePath);

        var puzzle = new SlidePuzzle(CurrentStageInfo.Rows, CurrentStageInfo.Columns);

        // Prepare the puzzle controller for new puzzle
        _controller = new SlidePuzzleController(MyPuzzleView, puzzle);

        _controller.OnReady += () => GoToAppState("OnGameReady");

        _controller.OnCompleted += async () =>
        {
            _cancellationTokenSource?.Cancel();

            // GoToAppState("OnGameCompleted");
            GoToElementState(CurrentStageImage, "OnGameCompleted");
            GoToElementState(CompletedMessage, "OnGameCompleted");

            await CompletedMessage.ScaleTo(1.2);
            await CompletedMessage.ScaleTo(1.0, 100);

            // Restart 
            _controller.OnReady?.Invoke();
        };

        _controller.Initialize();

    }

    // Visual state transiton helpers
    void GoToAppState(string state) => GoToElementState(this, state);
    void GoToElementState(VisualElement element, string state)
        => VisualStateManager.GoToState(element, state);

    // Stage loading method
    bool TryLoadStageInfo(string filename, out StageInfo stageInfo)
    {
        stageInfo = null;

        var helper = PuzzleResourceHelper.Instance;

        if (!helper.TryGetEmbededResourcePath(filename, out string path)) return false;

        using var stream = helper.GetEmbededResourceStream(path);
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();
        stageInfo = JsonSerializer.Deserialize<StageInfo>(contents);

        return true;
    }

    // UI callbacks
    async void StartClicked(object sender, EventArgs e)
    {
        GoToAppState("OnGamePlay");
        GoToElementState(CurrentStageImage, "Default");
        GoToElementState(CompletedMessage, "Default");

        await _controller.ShuffleAsync(CurrentStageInfo.ShuffleCounts);

        await ShowResetButtonAsync();
    }

    async void ResetClicked(object sender, EventArgs e)
    {
        GoToAppState("OnGameReset");

        await _controller.ResetAsync();

        GoToAppState("OnGamePlay");

        await ShowResetButtonAsync();
    }

    void PrevStageClicked(object sender, EventArgs e)
    {
        if (_indexCurrentStage == 0) return;
        else NextStageButton.IsEnabled = true;

        CurrentStageInfo = _stageList[--_indexCurrentStage];

        if (_indexCurrentStage == 0) PrevStageButton.IsEnabled = false;

        GoToElementState(CompletedMessage, "Default");

        PrepareNewStage();
    }

    void NextStageClicked(object sender, EventArgs e)
    {
        if (_indexCurrentStage == _stageList.Count - 1) return;
        else PrevStageButton.IsEnabled = true;

        CurrentStageInfo = _stageList[++_indexCurrentStage];

        if (_indexCurrentStage == _stageList.Count - 1) NextStageButton.IsEnabled = false;

        GoToElementState(CompletedMessage, "Default");

        PrepareNewStage();
    }

    void ReloadStageClicked(object sender, EventArgs e)
    {
        PrepareNewStage();
    }

    // Reset button 
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
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        ResetButton.IsVisible = true;
    }

}
