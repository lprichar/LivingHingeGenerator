// See https://aka.ms/new-console-template for more information
using LivingHingeGenerator.Lib;

var svgMaker = new SvgMaker(4.3M, 1M);
svgMaker.WriteToDisk("out.svg");