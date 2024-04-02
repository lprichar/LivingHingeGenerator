using LivingHingeGenerator.Lib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LivingHingeGenerator.Blazor.Pages;

public partial class Home
{
    private bool Loading { get; set; }

    private string? SvgAsText { get; set; }

    [Inject] 
    private IJSRuntime JsRuntime { get; set; } = null!;

    public decimal HeightInInches { get; set; } = 4.57M;
    public decimal DiameterInInches { get; set; } = .723M;

    private async Task Generate()
    {
        Loading = true;
        try
        {
            // calculate width from diameter aka C=PiD;
            var widthInInches = DiameterInInches * (decimal)Math.PI;

            var svgMaker = new SvgMaker(HeightInInches, widthInInches);
            var svAsString = svgMaker.GetSvAsString();
            SvgAsText = svAsString;
            object[] args = ["out.svg", svAsString];
            await JsRuntime.InvokeAsync<string>("downloadFile", args);
            await Task.Yield();
        }
        finally
        {
            Loading = false;
        }
    }
}

