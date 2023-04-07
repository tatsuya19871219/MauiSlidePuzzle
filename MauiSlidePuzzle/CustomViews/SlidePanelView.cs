using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiSlidePuzzle.Models;

namespace MauiSlidePuzzle.CustomViews;

abstract internal partial class SlidePanelView : ContentView
{
    public Point Translation
    {
        get => new Point(this.TranslationX, this.TranslationY);
        set
        {
            (this.TranslationX, this.TranslationY) = value;
        }
    }
    //Point _translation;

    internal bool IsMoving => _isMoving;
    protected bool _isMoving = false;

    readonly GraphicsView _gv;
    //readonly protected PanelDrawable Drawable;

    public int ID {get; init;}

    internal Action<SlidePanelView> Tapped;

    protected SlidePanelView(Microsoft.Maui.Graphics.IImage image, RectF clipRect, int id)
    {
        _gv = new GraphicsView();

        var drawable = new PanelDrawable(image, clipRect);
        _gv.Drawable = drawable;

        ID = id;

        Content = new Grid
        {
            Children = { _gv }
        };

        this.WidthRequest = clipRect.Width;
        this.HeightRequest = clipRect.Height;

        this.HorizontalOptions = LayoutOptions.Start;
        this.VerticalOptions = LayoutOptions.Start;

        this.Translation = new Point(clipRect.X, clipRect.Y);

        // this.TranslationX = _translation.X;
        // this.TranslationY = _translation.Y;

        drawable.DrawPanelFrame += DrawPanelFrame;

        TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
        tapRecognizer.Tapped += (s, e) => Tapped?.Invoke(this);

        _gv.GestureRecognizers.Add(tapRecognizer);
    }

    // protected void AddTapRecognizer(Action<SlidePanelView> tapped)
    // {
    //     TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
    //     tapRecognizer.Tapped += (s, e) =>
    //     {
    //         tapped?.Invoke(this);
    //     };

    //     _gv.GestureRecognizers.Add(tapRecognizer);
    // }

    abstract internal void DrawPanelFrame(ICanvas canvas, RectF clipRect);
    //abstract public void SetTappedNotifier(Action<SlidePanelView> tapped);

    abstract internal Task MoveTo(Point point, uint length);

    async internal Task Shake(double amplitude, uint length)
    {
        _isMoving = true;

        uint len = length/4;
        await this.RelRotateTo(amplitude, len);
        await this.RelRotateTo(-2*amplitude, 2*len);
        await this.RelRotateTo(amplitude, len);

        _isMoving = false;
    }
}
