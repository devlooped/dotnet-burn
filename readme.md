![Icon](https://raw.githubusercontent.com/devlooped/dotnet-burn/main/assets/img/32x32.png) dotnet-burn
============

[![Version](https://img.shields.io/nuget/v/dotnet-burn.svg?color=royalblue)](https://www.nuget.org/packages/dotnet-burn) 
[![Downloads](https://img.shields.io/nuget/dt/dotnet-burn.svg?color=green)](https://www.nuget.org/packages/dotnet-burn) 
[![License](https://img.shields.io/github/license/devlooped/chromium.svg?color=blue)](https://github.com/devlooped/dotnet-burn/blob/main/license.txt) 
[![Build](https://github.com/devlooped/dotnet-burn/workflows/build/badge.svg?branch=main)](https://github.com/devlooped/dotnet-burn/actions)

Subtitles burner for .NET 6 powered by HandBrake

```
dotnet tool install -g dotnet-burn
```

## Usage
<!-- #content -->

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


<!-- #sponsors -->
<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->