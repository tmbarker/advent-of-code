// See https://aka.ms/new-console-template for more information

using Automation.SolutionRunner;

var year = 2022;
var day = 1;

if (args.Length >= 1 && int.TryParse(args[0], out var yearArg))
{
    year = yearArg;
}

if (args.Length >= 2 && int.TryParse(args[1], out var dayArg))
{
    day = dayArg;
}

SolutionRunner.Run(year, day);