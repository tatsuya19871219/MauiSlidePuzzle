using MauiSlidePuzzle.CustomViews;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.Controllers;

internal class SlidePuzzleController
{
    readonly SlidePuzzleView _view;
    readonly SlidePuzzle _model;

    internal Action OnReady;
    internal Action OnCompleted;

    internal SlidePuzzleController(SlidePuzzleView view, SlidePuzzle model)
    {
        _view = view;
        _model = model;
    }

    internal void Initialize()
    {
        // Initialize view
        _view.Initialize(_model);
    
        if (_view.PanelViews is null) throw new Exception("PanelViews is null");
        
        OnReady?.Invoke();
    }

    void EnablePanelTap()
    {
        foreach (var panelView in _view.PanelViews) 
            if (panelView != _view.BlankPanelView) panelView.Tapped += PanelTapped;
    }

    void DisablePanelTap()
    {
        foreach (var panelView in _view.PanelViews)
            if (panelView != _view.BlankPanelView) panelView.Tapped -= PanelTapped;
    }

    async internal Task ShuffleAsync(int counts = 10)
    {
        foreach (var panel in _model.Shuffle(counts))
        {
            var id = panel.ID;

            ImagePanelView imagePanelView = _view.PanelViews[id] as ImagePanelView;
            BlankPanelView blankPanelView = _view.BlankPanelView;

            uint dt = 100;

            await SwapPanelTranslationAsync(imagePanelView, blankPanelView, dt);
        }

        // If shuffling is failed to make random puzzle, try it again
        if (_model.IsCompleted()) await ShuffleAsync(counts); 
        else EnablePanelTap();
    }

    async void PanelTapped(SlidePanelView panelView)
    {
        if (panelView.IsMoving) return;

        int id = panelView.ID;

        SlidePanel panel = _model.Panels[id];

        if ( _model.TryMove(panel) )
        {
            ImagePanelView imagePanelView = panelView as ImagePanelView;
            BlankPanelView blankPanelView = _view.BlankPanelView;

            uint dt = 200;

            await SwapPanelTranslationAsync(imagePanelView, blankPanelView, dt);
        }
        else await panelView.Shake(20, 200);

        if (_model.IsCompleted()) 
        {
            DisablePanelTap();
            OnCompleted?.Invoke();
        }
    }

    async Task SwapPanelTranslationAsync(ImagePanelView imagePanelView, BlankPanelView blankPanelView, uint length)
    {
        Point trans0 = imagePanelView.Translation;
        Point trans1 = blankPanelView.Translation;

        blankPanelView.ZIndex = imagePanelView.ZIndex - 1;

        _ = blankPanelView.MoveTo(trans0, length);
        await imagePanelView.MoveTo(trans1, length);
    }
}
