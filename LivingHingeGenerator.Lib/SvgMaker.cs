using System.Drawing;
using Svg;

namespace LivingHingeGenerator.Lib;

public class SvgMaker(decimal heightInInches, decimal widthInInches)
{
    private static readonly SvgUnit DistanceBetweenDashes = new(SvgUnitType.Millimeter, 13);
    private static readonly SvgUnit SpaceBetweenDashes = new(SvgUnitType.Millimeter, 6);
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
        var linesGroup = new SvgGroup();
        svgDoc.Children.Add(linesGroup);
        int numberOfLines = (int)(Width / HorizontalSpaceBetweenLines) + 1;

        SvgUnit offset = DistanceBetweenDashes / 2;
        for (int i = 0; i < numberOfLines; i++)
        {
            SvgUnit startX = i * HorizontalSpaceBetweenLines;
            bool isOffset = i % 2 == 0;
            SvgUnit startY = isOffset ? -offset : 0;
            SvgUnit thisHeight = isOffset ? Height + offset : Height;
            MakeDashy(linesGroup, startX, startY, thisHeight);
        }

        var boxGroup = new SvgGroup();
        svgDoc.Children.Add(boxGroup);
        boxGroup.Children.Add(new SvgRectangle()
        {
            X = 0,
            Y = 0,
            Width = Width,
            Height = Height,
            Stroke = new SvgColourServer(Color.Blue),
            Fill = SvgPaintServer.None
        });
        return svgDoc;
    }

    private static void MakeDashy(SvgGroup group, SvgUnit startX, SvgUnit startY, SvgUnit height)
    {
        int numberOfLines = (int)(height / DistanceBetweenDashes) + 1;
        for (int i = 0; i < numberOfLines; i++)
        {
            group.Children.Add(new SvgLine
            {
                StartX = startX,
                StartY = startY + i * DistanceBetweenDashes,
                EndX = startX,
                EndY = startY + (((i + 1) * DistanceBetweenDashes) - SpaceBetweenDashes),
                Stroke = new SvgColourServer(Color.Red),
            });
        }
    }
}