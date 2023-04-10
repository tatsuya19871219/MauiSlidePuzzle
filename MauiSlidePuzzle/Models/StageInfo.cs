using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSlidePuzzle.Models;

class StageInfo
{
    readonly public string ImageFilename;
    readonly public int Rows;
    readonly public int Columns;

    internal StageInfo(string imageFilename, int rows, int columns)
    {
        if (rows < 1 || columns < 1) throw new ArgumentException("Puzzle rows/columns should be positive");

        
    }
}
