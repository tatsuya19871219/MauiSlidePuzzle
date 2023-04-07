using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiSlidePuzzle.CustomViews;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.Controllers;

internal class SlidePuzzleController //: ObservableObject
{
    // [ObservableProperty] bool _isReady = false;
    // [ObservableProperty] bool _isCompleted = false;

    readonly SlidePuzzleView _view;
    readonly SlidePuzzle _model;

    internal Action Ready;
    internal Action Completed;

    internal SlidePuzzleController(SlidePuzzleView view, SlidePuzzle model)
    {
        _view = view;
        _model = model;

        //Initialize();
    }

    internal void Initialize()
    {
        // Initialize view
        _view.Initialize(_model);
    
        // Set callback events of the view
        if (_view.PanelViews is null) throw new Exception("PanelViews is null");
        // foreach (var panelView in _view.PanelViews)
        // {
        //     panelView.SetTappedNotifier(PanelTapped);
        // }

        // Make slide puzzle view to show "START" text
        // var tapRecognizer = new TapGestureRecognizer();
        // tapRecognizer.Tapped += (s,e) =>
        // {
        //     ViewTapped();
        // };
        // _view.StartImage.GestureRecognizers.Add(tapRecognizer);

        // _view.ShowStartImage();

        //IsReady = true;
        Ready?.Invoke();
    }

    void EnablePanelTap()
    {
        foreach (var panelView in _view.PanelViews) 
            panelView.Tapped += PanelTapped;
        // {
        //     panelView.SetTappedNotifier(PanelTapped);
        // }
    }

    void DisablePanelTap()
    {
        foreach (var panelView in _view.PanelViews)
            panelView.Tapped -= PanelTapped;
    }

    async internal Task ShuffleAsync(int counts = 10)
    {
        //IsReady = false;
        //IsCompleted = false;

        //var list = _model.Shuffle();

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

    // async void ViewTapped()
    // {
    //     // _view.HideStartImage();
    //     // _view.HideCompletedImage();

    //     await ShuffleAsync();
    // }

    async void PanelTapped(SlidePanelView panelView)
    {
        if (panelView.IsMoving) return;

        int id = panelView.ID;

        SlidePanel panel = _model.Panels[id];

        if ( _model.TryMove(panel) )
        {
            //int idBlank = _model.BlankPanel.ID;

            ImagePanelView imagePanelView = panelView as ImagePanelView;
            BlankPanelView blankPanelView = _view.BlankPanelView;

            uint dt = 200;

            await SwapPanelTranslationAsync(imagePanelView, blankPanelView, dt);
        }
        else await panelView.Shake(20, 200);
        // {
        //     View view = panelView as View;

        //     uint dt = 100;
        //     await view.RotateTo(20, dt);
        //     await view.RotateTo(-20, dt);
        //     await view.RotateTo(0, dt);
        // }

        if (_model.IsCompleted()) 
        {
            DisablePanelTap();
            Completed?.Invoke();
        }
        // {
        //     //IsCompleted = true;
        //     //IsReady = true;
        // }
        // // {
        //     // Show "CLEAR" message
        //     // _view.ShowCompletedImage();
        //     // _view.ShowStartImage();
        // }
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
