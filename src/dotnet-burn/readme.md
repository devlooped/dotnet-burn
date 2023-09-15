```
burn [OPTIONS]+ input subtitles output [-- [handbrake args]]
```

Run `burn --help` for more details:

```
Usage: burn [OPTIONS]+ input subtitles output [-- [handbrake args]]
Burns the subtitles over the input video into output.

Options:
  -?, -h, --help             Display this help
  -f, --font-name?=VALUE     Font name
  -s, --font-size?=VALUE     Font size
  -c, --font-color?=VALUE    Font color
  -b, --bold                 Bold font
  -i, --italic               Italic font
  -u, --underline            Underline font
```

Example: burn subtitles file using Helvetica 75pt yellow bold font:

```
burn -f "Helvetica" -s 75 -c #FFD300 -b input.mp4 input.srt output.mp4
```

<!-- #sponsors -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->