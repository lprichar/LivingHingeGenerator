using LivingHingeGenerator.Lib;

namespace LivingHingeGenerator.Blazor.Pages;

public partial class Home
{
    private string Message { get; set; } = Class1.GetMessage();

    private async Task Generate()
    {
        var svgMaker = new SvgMaker();
        svgMaker.WriteToDisk();
        await Task.Yield();
    }
}

