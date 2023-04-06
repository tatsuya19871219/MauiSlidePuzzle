using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiSlidePuzzle.CustomViews;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.Controllers;

internal class SlidePuzzleController
{
    readonly SlidePuzzleView _view;
    readonly SlidePuzzle _model;

    internal SlidePuzzleController(SlidePuzzleView view, SlidePuzzle model)
    {
        _view = view;
        _model = model;

        Initialize();
    }

    async void Initialize()
    {
        // Initialize view
        _view.Initialize(_model);
        //await _view.InitializeAsync(_model.Rows, _model.Columns);

        // Set callback events of the view
        if (_view.PanelViews is null) throw new Exception("PanelViews is null");
        foreach (var panelView in _view.PanelViews)
        {
            panelView.SetTappedNotifier(Tapped);
        }

        //var list = _model.Shuffle();

        foreach (var panel in _model.Shuffle(100))
        {
            var id = panel.ID;

            ImagePanelView imagePanelView = _view.PanelViews[id] as ImagePanelView;
            BlankPanelView blankPanelView = _view.BlankPanelView;

            uint dt = 100;

            await SwapPanelTranslationAsync(imagePanelView, blankPanelView, dt);
        }
    }

    async void Tapped(SlidePanelView panelView)
    {
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
        else
        {
            View view = panelView as View;

            uint dt = 100;
            await view.RotateTo(20, dt);
            await view.RotateTo(-20, dt);
            await view.RotateTo(0, dt);
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
