﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiSlidePuzzle.Models;

public partial class StageInfo : ObservableObject
{
    [ObservableProperty] string _stageName;
    [ObservableProperty] string _imageFilename;
    [ObservableProperty] int _rows;
    [ObservableProperty] int _columns;
    [ObservableProperty] string _embeddedImagePath;
    [ObservableProperty] int _shuffleCounts;

    internal StageInfo(string stageName, string imageFilename, int rows, int columns, int counts = 10)
    {
        if (rows < 1 || columns < 1) throw new ArgumentException("Puzzle rows/columns should be positive");

        var helper = PuzzleResourceHelper.Instance;

        if (!helper.Exists(imageFilename)) throw new ArgumentException($"Given file {imageFilename} doesn't exist");

        StageName = stageName;
        ImageFilename = imageFilename;
        Rows = rows;
        Columns = columns;

        helper.TryGetEmbededResourcePath(imageFilename, out var embeddPath);

        EmbeddedImagePath = embeddPath;

        ShuffleCounts = counts;
    }
}
