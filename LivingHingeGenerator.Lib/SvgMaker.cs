using System.Drawing;
using Svg;

namespace LivingHingeGenerator.Lib;

public class SvgMaker(decimal heightInInches, decimal widthInInches)
{
    const int DistanceBetweenDashes = 20;
    const int SpaceBetweenDashes = 5;
    const int Height = 115;
    const int Width = 392;

    public void WriteToDisk()
    {
        SvgDocument svgDoc = GetDashyRectangularSvgDocument();
        var currentDirectory = Directory.GetCurrentDirectory();
        var outFile = Path.Combine(currentDirectory, "out.svg");
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

    private static SvgDocument GetDashyRectangularSvgDocument()
    {
        var svgDoc = new SvgDocument
        {
            Width = Width,
            Height = Height,
            ViewBox = new SvgViewBox(0, 0, Width, Height),
        };
        var linesGroup = new SvgGroup();
        svgDoc.Children.Add(linesGroup);
        const int horizontalSpaceBetweenLines = 5;
        int numberOfLines = (Width / horizontalSpaceBetweenLines) + 1;

        const int offset = DistanceBetweenDashes / 2;
        for (int i = 0; i < numberOfLines; i++)
        {
            int startX = i * horizontalSpaceBetweenLines;
            bool isOffset = i % 2 == 0;
            int startY = isOffset ? -offset : 0;
            int thisHeight = isOffset ? Height + offset : Height;
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

    private static void MakeDashy(SvgGroup group, int startX, int startY, int height)
    {
        int numberOfLines = (height / DistanceBetweenDashes) + 1;
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