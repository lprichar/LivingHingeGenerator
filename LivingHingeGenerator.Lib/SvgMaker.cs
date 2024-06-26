﻿using System.Drawing;
using Svg;

namespace LivingHingeGenerator.Lib;

public class SvgMaker(decimal heightInInches, decimal widthInInches)
{
    /// <summary>
    /// The distance from the end of one dash to the start of the next dash
    /// </summary>
    private static readonly SvgUnit SpaceBetweenDashes = new(SvgUnitType.Millimeter, 6);
    
    /// <summary>
    /// Height of a dash
    /// </summary>
    private static readonly SvgUnit DashHeight = new(SvgUnitType.Millimeter, 11);

    /// <summary>
    /// The distance from the top of one dash to the top of the next dash
    /// </summary>
    private static readonly SvgUnit DistanceBetweenDashes = SpaceBetweenDashes + DashHeight;

    private static readonly SvgUnit HorizontalSpaceBetweenLines = new(SvgUnitType.Millimeter, 1);
    private SvgUnit Height { get; } = new(SvgUnitType.Inch, (float)heightInInches);
    private SvgUnit Width { get; } = new(SvgUnitType.Inch, (float)widthInInches);

    public void WriteToDisk(string fileName)
    {
        SvgDocument svgDoc = GetDashyRectangularSvgDocument();
        var currentDirectory = Directory.GetCurrentDirectory();
        var outFile = Path.Combine(currentDirectory, fileName);
        svgDoc.Write(outFile);
    }

    public string GetSvAsString()
    {
        SvgDocument svgDoc = GetDashyRectangularSvgDocument();
        using var stream = new MemoryStream();
        svgDoc.Write(stream);
        stream.Position = 0; // Reset stream position to read from the beginning
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private SvgDocument GetDashyRectangularSvgDocument()
    {
        var svgDoc = new SvgDocument
        {
            Width = Width,
            Height = Height,
            ViewBox = new SvgViewBox(0, 0, Width, Height),
        };

        var linesGroup = MakeLinesGroup();
        svgDoc.Children.Add(linesGroup);

        var outerBox = MakeOuterBox();
        svgDoc.Children.Add(outerBox);
        return svgDoc;
    }

    private SvgGroup MakeLinesGroup()
    {
        var linesGroup = new SvgGroup();
        int numberOfLines = (int)(Width / HorizontalSpaceBetweenLines);

        SvgUnit offset = DistanceBetweenDashes / 2;
        for (int i = 0; i < numberOfLines; i++)
        {
            SvgUnit startX = (i + 1) * HorizontalSpaceBetweenLines;
            bool isOffset = i % 2 == 1;
            SvgUnit startY = isOffset ? offset : 0;
            SvgUnit thisHeight = isOffset ? Height - offset : Height;
            MakeDashy(linesGroup, startX, startY, thisHeight);
        }

        return linesGroup;
    }

    private static void MakeDashy(SvgGroup group, SvgUnit x, SvgUnit startY, SvgUnit height)
    {
        int numberOfDashes = (int)(height / DistanceBetweenDashes) + 1;
        for (int i = 0; i < numberOfDashes; i++)
        {
            var dashStartY = startY + i * DistanceBetweenDashes;
            var dashEndY = startY + (((i + 1) * DistanceBetweenDashes) - SpaceBetweenDashes);
            var dash = new SvgLine
            {
                StartX = x,
                StartY = dashStartY,
                EndX = x,
                EndY = dashEndY,
                Stroke = new SvgColourServer(Color.Red),
            };
            group.Children.Add(dash);
        }
    }

    private SvgGroup MakeOuterBox()
    {
        var boxGroup = new SvgGroup();
        boxGroup.Children.Add(new SvgRectangle()
        {
            X = 0,
            Y = 0,
            Width = Width,
            Height = Height,
            Stroke = new SvgColourServer(Color.Blue),
            Fill = SvgPaintServer.None
        });
        return boxGroup;
    }
}