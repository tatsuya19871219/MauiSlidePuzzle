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
        
        // Set the last panel as the blank
        _blank = _panels.Last();
    }

    internal IEnumerable<SlidePanel> Shuffle(int counts = 10)
    {
        int i = 0;

        while (i < counts)
        {
            var neighbors = GetNeighbors(_blank);

            var k = Random.Shared.Next(neighbors.Count);
            var panel = neighbors[k];

            if (TryMove(panel))
            {
                i++;
                yield return panel;
            }
            else continue;
        }
    }

    internal bool TryMove(SlidePanel panel)
    {
        if (panel == _blank) throw new Exception("Blank panel can not move.");

        if (GetNeighbors(_blank).Contains(panel))
        {
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

    internal bool IsCompleted()
    {
        return _panels.All(p => p.IsInitialLocation());
    }
}
