using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSlidePuzzle.Models;

internal class SlidePuzzle
{
    public int Rows {get; init;}
    public int Columns {get; init;}
    public List<SlidePanel> Panels => _panels;
    public SlidePanel BlankPanel => _blank;
    readonly List<Point> _initialLocations = new();
    readonly List<SlidePanel> _panels = new();
    readonly SlidePanel _blank;

    internal SlidePuzzle(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0) throw new ArgumentException("rows and columns should be positive numbers");

        Rows = rows;
        Columns = columns;

        // Prepare locations
        for (int iy = 0; iy < rows; iy++)
            for (int ix = 0; ix < columns; ix++)
                _initialLocations.Add( new Point(ix, iy) );

        // Prepare panels for each location
        for (int k = 0; k < _initialLocations.Count; k++)
            _panels.Add( new SlidePanel(_initialLocations[k], k));
        // {
        //     if (k == _initialLocations.Count-1)
        //         _panels.Add( _blank = new BlankPanel(_initialLocations[k], k) );
        //      else
        //         _panels.Add( new ImagePanel(_initialLocations[k], k) );
        // }

        _blank = _panels.Last();

        // // Assign geometric relation (i.e., neighbors) for each panel
        // foreach (var panel in _panels)
        // {
        //     // var neighbors = _panels.Where( p => p.Location.Distance(panel.Location) == 1).ToList();
        //     // panel.Neighbors = neighbors;

        //     panel.Neighbors = GetNeighbors(panel);
        // }

    }

    internal bool TryMove(SlidePanel panel)
    {
        if (GetNeighbors(_blank).Contains(panel))
        {
            //panel.SwapNeighbors(_blank);
            panel.SwapLocation(_blank);

            return true;
        }
        else
        {
            return false;
        }
    }

    List<SlidePanel> GetNeighbors(SlidePanel panel)
    {
        return _panels.Where( p => p.Location.Distance(panel.Location) == 1 ).ToList();
    }


}
