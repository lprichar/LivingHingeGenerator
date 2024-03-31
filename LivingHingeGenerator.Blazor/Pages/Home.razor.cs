using LivingHingeGenerator.Lib;

namespace LivingHingeGenerator.Blazor.Pages;

public partial class Home
{
    private bool Loading { get; set; }
    private string Message { get; set; } = Class1.GetMessage();
    public string SvgAsText { get; set; }

    private async Task Generate()
    {
        Loading = true;
        try
        {
            var svgMaker = new SvgMaker();
            var svAsString = svgMaker.GetSvAsString();
            SvgAsText = svAsString;
            await Task.Yield();
        }
        finally
        {
            Loading = false;
        }
    }
}

