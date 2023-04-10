﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiSlidePuzzle;

class PuzzleResourceHelper
{
    readonly Assembly _assembly;
    readonly List<string> _puzzleResourceList;

    static public PuzzleResourceHelper Instance => _instance ?? new PuzzleResourceHelper();
    static PuzzleResourceHelper _instance = null;

    private PuzzleResourceHelper()
    {
        _assembly = GetType().GetTypeInfo().Assembly;
        _puzzleResourceList = new();

        var files = _assembly.GetManifestResourceNames().ToList();

        _puzzleResourceList = files.Where(file => file.Contains("Puzzles")).ToList();
    }

    internal bool Exists(string name)
    {
        return _puzzleResourceList.Any(file => file.Contains(name));
    }

    internal bool TryGetEmbededResourcePath(string name, out string path)
    {
        bool exists = Exists(name);

        if (exists) path = _puzzleResourceList.Where(file => file.Contains(name)).FirstOrDefault();
        else path = null;
        
        return exists;
    }
}