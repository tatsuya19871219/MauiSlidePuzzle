namespace MauiSlidePuzzle.Models;

internal class SlidePanel
{

    public int ID {get; init;}
    public Point Location {get; set;}
    
    readonly Point _initialLocation;


    internal SlidePanel(Point initialLocation, int id)
    {
        ID = id;
        Location = _initialLocation = initialLocation;
    }

    internal void SwapLocation(SlidePanel targetPanel)
    {
        // var oldLocation = this.Location;
        // var newLocation = targetPanel.Location;

        // this.Location = newLocation;
        // targetPanel.Location = oldLocation;

        // Swapping by using tupple achieves the same operation with above
        (targetPanel.Location, this.Location) = (this.Location, targetPanel.Location);
    }

    internal bool IsInitialLocation() => Location.Equals(_initialLocation);

}
