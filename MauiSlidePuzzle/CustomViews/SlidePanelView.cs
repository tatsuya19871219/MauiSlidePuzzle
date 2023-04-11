using SkiaSharp;

namespace MauiSlidePuzzle.CustomViews;

abstract internal partial class SlidePanelView : ContentView
{
    public Point Translation
    {
        get => new Point(this.TranslationX, this.TranslationY);
        set => (this.TranslationX, this.TranslationY) = value;
    }

    internal bool IsMoving => _isMoving;
    protected bool _isMoving = false;

    readonly ClipImagePanel _imagePanel;
    readonly GraphicsView _gv;

    public int ID { get; init; }

    internal Action<SlidePanelView> Tapped;

    protected SlidePanelView(SKImage skImage, RectF clipRect, int id)
    {
        _gv = new GraphicsView();

        var drawable = new PanelFrameDrawable(clipRect) { DrawFrame = DrawPanelFrame };
        _gv.Drawable = drawable;

        _imagePanel = new ClipImagePanel() { Image = skImage, ClipRect = clipRect };

        ID = id;

        Content = new Grid
        {
            Children = { _imagePanel, _gv }
        };

        this.WidthRequest = clipRect.Width;
        this.HeightRequest = clipRect.Height;

        this.HorizontalOptions = LayoutOptions.Start;
        this.VerticalOptions = LayoutOptions.Start;

        this.Translation = new Point(clipRect.X, clipRect.Y);


        TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
        tapRecognizer.Tapped += (s, e) => Tapped?.Invoke(this);

        _gv.GestureRecognizers.Add(tapRecognizer);
    }


    abstract internal void DrawPanelFrame(ICanvas canvas, RectF clipRect);

    abstract internal Task MoveTo(Point point, uint length);

    async internal Task Shake(double amplitude, uint length)
    {
        _isMoving = true;

        uint len = length / 4;
        await this.RelRotateTo(amplitude, len);
        await this.RelRotateTo(-2 * amplitude, 2 * len);
        await this.RelRotateTo(amplitude, len);

        _isMoving = false;
    }
}
