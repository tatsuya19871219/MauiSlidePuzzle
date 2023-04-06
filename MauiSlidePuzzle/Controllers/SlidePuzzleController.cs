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

    void Initialize()
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
    }

    async void Tapped(SlidePanelView panelView)
    {
        int id = panelView.ID;

        SlidePanel panel = _model.Panels[id];

        if ( _model.TryMove(panel) )
        {
            int idBlank = _model.BlankPanel.ID;

            SlidePanelView blankPanelView = _view.PanelViews[idBlank];

            uint dt = 200;

            Point trans0 = panelView.Translation;
            Point trans1 = blankPanelView.Translation;

            View view = panelView as View;
            View viewBlank = blankPanelView as View;

            viewBlank.ZIndex = view.ZIndex - 1;
            viewBlank.TranslationX = trans0.X;
            viewBlank.TranslationY = trans0.Y;

            viewBlank.IsVisible = false;

            await view.TranslateTo(trans1.X, trans1.Y, dt);

            viewBlank.IsVisible = true;

            panelView.Translation = trans1;
            blankPanelView.Translation = trans0;
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
}
