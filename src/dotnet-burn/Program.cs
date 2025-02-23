using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Mono.Options;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using Spectre.Console;

if (args.Any(x => x == "--debug"))
    Debugger.Launch();

if (HandBrake.Path == null)
{
    AnsiConsole.MarkupLineInterpolated($"[red]x[/] Current runtime {RuntimeInformation.RuntimeIdentifier} is not supported.");
    return -1;
}

#if DEBUG
AnsiConsole.MarkupLineInterpolated($"[green]✓[/] Located [lime][link={HandBrake.Path}]compatible HandBrake[/][/]");
#endif

var help = false;
var bold = false;
var italic = false;
var underline = false;

int? fs = null;
string? fn = null;
string? fc = null;

var options = new OptionSet
{
    { "?|h|help", "Display this help", x => help = x != null },
    { "f|font-name?=", "Font name", x => fn = x },
    { "s|font-size?=", "Font size", (int x) => fs = x },
    { "c|font-color?=", "Font color", x => fc = x },

    { "b|bold", "Bold font", x => bold = true },
    { "i|italic", "Italic font", x => italic = true },
    { "u|underline", "Underline font", x => underline = true },
};

var burnArgs = args.TakeWhile(x => x != "--").ToList();
var hbArgs = args.SkipWhile(x => x != "--").Skip(1).ToList();

List<string> extra;
try
{
    extra = options.Parse(burnArgs);
    if (extra.Count != 3 || help)
        return RenderHelp(options);

    var input = extra[0];
    var subtitles = extra[1];
    var output = extra[2];

    if (!File.Exists(input))
    {
        AnsiConsole.MarkupLineInterpolated($"[red]x[/] Input file {input} does not exist.");
        return -1;
    }
    if (!File.Exists(subtitles))
    {
        AnsiConsole.MarkupLineInterpolated($"[red]x[/] Subtitles file {subtitles} does not exist.");
        return -1;
    }

    if (Path.GetDirectoryName(output) is string dir && !string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        Directory.CreateDirectory(dir);

    if (bold || italic || underline || fs != null || fn != null || fc != null)
    {
        // Pre-process subs
        var subs = Subtitle.Parse(subtitles);
        subtitles = Path.GetTempFileName();

        var leading = new StringBuilder();
        var trailing = new StringBuilder();
        if (bold)
        {
            leading.Append("<b>");
            trailing.Append("</b>");
        }
        if (italic)
        {
            leading.Append("<i>");
            trailing.Insert(0, "</i>");
        }
        if (underline)
        {
            leading.Append("<u>");
            trailing.Insert(0, "</u>");
        }
        if (fs != null || fn != null || fc != null)
        {
            leading.Append("<font");
            if (fs != null)
                leading.Append($" size=\"{fs}\"");
            if (fn != null)
                leading.Append($" face=\"{fn}\"");
            if (fc != null)
                leading.Append($" color=\"{fc}\"");
            leading.Append('>');
            trailing.Insert(0, "</font>");
        }

        foreach (var para in subs.Paragraphs)
        {
            para.Text = leading + para.Text + trailing;
        }

        File.WriteAllText(subtitles, new SubRip().ToText(subs, Path.GetFileName(input)));
#if DEBUG
        //if (AnsiConsole.Confirm("Open formatted subtitles file?", false))
        //    Process.Start(new ProcessStartInfo("code", subtitles) { UseShellExecute = true });
#endif
    }

    hbArgs.Insert(0, "-i");
    hbArgs.Insert(1, input);
    hbArgs.Insert(2, "-o");
    hbArgs.Insert(3, output);
    hbArgs.Insert(4, "--srt-file");
    hbArgs.Insert(5, subtitles);
    hbArgs.Insert(6, "--srt-burn");

    var psi = new ProcessStartInfo(HandBrake.Path, string.Join(" ", hbArgs))
    {
        //RedirectStandardError = true,
        //RedirectStandardOutput = true,
        //UseShellExecute = false,
    };

    AnsiConsole.MarkupLineInterpolated($"[grey] => {psi.FileName} {psi.Arguments} [/]");
    Process.Start(psi)?.WaitForExit();
    if (AnsiConsole.Confirm("Open output video file?", true))
        Process.Start(new ProcessStartInfo(output) { UseShellExecute = true });
}
catch (OptionException e)
{
    AnsiConsole.Write("burn: ");
    AnsiConsole.WriteLine(e.Message);
    AnsiConsole.WriteLine("Try `greet --help' for more information.");
    return -1;
}

return 0;

int RenderHelp(OptionSet options)
{
    AnsiConsole.WriteLine("Usage: burn [OPTIONS]+ input subtitles output [-- [handbrake args]]");
    AnsiConsole.WriteLine("Burns the subtitles over the input video into output.");
    AnsiConsole.WriteLine();
    AnsiConsole.WriteLine("Options:");
    options.WriteOptionDescriptions(Console.Out);
    return 0;
}