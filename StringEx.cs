using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;
public static class StringEx
{
    private const string Pattern = @"([^;:]*)\:?([^;:]*)\:?([^;:]*)\:?([^;:]*)\:?([^;:]*)\;";
    private static readonly Dictionary<string, string> styles = new Dictionary<string, string>()
        {
            {"b","b"},
            {"B","b"},
            {"BOLD","b"},
            {$"{FontStyle.Bold}","b"},

            {"i","i"} ,
            {"I","i"} ,
            {"ITALIC","i"},
            {$"{FontStyle.Italic}","i"},

            {"bi",$"{FontStyle.BoldAndItalic}"},
            {"ib",$"{FontStyle.BoldAndItalic}"},
            {"Bi",$"{FontStyle.BoldAndItalic}"},
            {"Ib",$"{FontStyle.BoldAndItalic}"},
            {"BI",$"{FontStyle.BoldAndItalic}"},
            {"IB",$"{FontStyle.BoldAndItalic}"},
            {$"{FontStyle.BoldAndItalic}",$"{FontStyle.BoldAndItalic}"},
            {"BoldItalic",$"{FontStyle.BoldAndItalic}"},

        };
    private static readonly Dictionary<string, string> variant = new Dictionary<string, string>()
    {
        {"U","ToUpper"},
        {"UP","ToUpper"},
        {"UPPER","ToUpper"},

        {"u","ToTitleCase"},
        {"Upper","ToTitleCase"},
        {"Up","ToTitleCase"},


        {"L","ToLower"},
        {"LO","ToLower"},
        {"LOWER","ToLower"},
        {"l","ToLower"},
        {"Lower","ToLower"},
        {"Lo","ToLower"},


    };
    private static readonly Dictionary<string, string> sizeList = Enumerable.Range(0, 999).ToList().ConvertAll(o => o.ToString()).ToDictionary(x => x, x => x);
    private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color>()
    {
        { "Red" ,Color.red},
        { "RED" ,Color.red},
        { "red" ,Color.red},
        { "r" ,Color.red},
        { "R" ,Color.red},
        {$"{Color.red}",Color.red},

        {"Yellow",Color.yellow},
        {"YEllOW",Color.yellow},
        {"yellow",Color.yellow},
        {"y",Color.yellow},
        {"Y",Color.yellow},
        {$"{Color.yellow}",Color.yellow},

        {"Green",Color.green},
        {"GREEN",Color.green},
        {"green",Color.green},
        {"g",Color.green},
        {"G",Color.green},
        {$"{Color.green}",Color.green},

        {"Magenta",Color.magenta},
        {"MAGENTA",Color.magenta},
        {"magenta",Color.magenta},
        {"m",Color.magenta},
        {"M",Color.magenta},
        {$"{Color.magenta}",Color.magenta},

        {"White",Color.white},
        {"WHITE",Color.white},
        {"white",Color.white},
        {"w",Color.white},
        {"W",Color.white},
        {$"{Color.white}",Color.white},

        {"Black",Color.black},
        {"BLACK",Color.black},
        {"black",Color.black},
        {"bla",Color.black},
        {"Bla",Color.black},
        {$"{Color.black}",Color.black},

        {"Blue",Color.blue},
        {"blue",Color.blue},
        {"bl",Color.blue},
        {"Bl",Color.blue},
        {"BL",Color.blue},
        {$"{Color.blue}",Color.blue},

        {"Gray",Color.gray},
        {"GRAY",Color.gray},
        {"gray",Color.gray},
        {"gr",Color.gray},
        {"GR",Color.gray},
        {$"{Color.gray}",Color.gray},
        {"Grey",Color.grey},
        {"GREY",Color.grey},
        {"grey",Color.grey},

        {"Cyan",Color.cyan},
        {"CYAN",Color.cyan},
        {"cyan",Color.cyan},
        {"c",Color.cyan},
        {"C",Color.cyan},
        {$"{Color.cyan}",Color.cyan},

        {"violet",new Color(128,0,128)},
        {"Violet",new Color(128,0,128)},
        {"VIOLET",new Color(128,0,128)},
        {"V",new Color(128,0,128)},
        {"v",new Color(128,0,128)},

        {"Orange",new Color(255,165,0)},
        {"orange",new Color(255,165,0)},
        {"ORANGE",new Color(255,165,0)},
        {"O",new Color(255,165,0)},
        {"o",new Color(255,165,0)},

        {"olive",new Color(186,184,108)},
        {"Olive",new Color(186,184,108)},
        {"OLIVE",new Color(186,184,108)},
        {"OL",new Color(186,184,108)},
        {"ol",new Color(186,184,108)},

        {"purple",new Color(128,0,128)},
        {"Purple",new Color(128,0,128)},

        {"Darkred",new Color(139,0,0)},
        {"DarkRed",new Color(139,0,0)},
        {"darkred",new Color(139,0,0)},
        {"dr",new Color(139,0,0)},
        {"DR",new Color(139,0,0)},

        {"darkorange",new Color(255,140,0)},
        {"Darkorange",new Color(255,140,0)},
        {"DarkOrange",new Color(255,140,0)},
        {"DARKORANGE",new Color(255,140,0)},
        {"DO",new Color(255,140,0)},
        {"do",new Color(255,140,0)},


        {"darkgreen",new Color(34,139,34)},
        {"Darkgreen",new Color(34,139,34)},
        {"DARKGREEN",new Color(34,139,34)},
        {"DarkGreen",new Color(34,139,34)},
        {"DG",new Color(34,139,34)},
        {"dg",new Color(34,139,34)},



        {"Gold",new Color(255,215,0)},
        {"gold",new Color(255,215,0)},
        {"GOLD",new Color(255,215,0)},
        {"GO",new Color(255,215,0)},
        {"go",new Color(255,215,0)},

        {"Lightblue",new Color(173,216,230)},
        {"lightblue",new Color(173,216,230)},
        {"LightBlue",new Color(173,216,230)},
        {"LB",new Color(173,216,230)},
        {"lb",new Color(173,216,230)},

    };
    private const int MaxSize = 50, MinSize = 5;
    private static string GetValue(string text, GroupCollection groups, Dictionary<string, Color> list) => GetValue(text, groups, list, out var value) ? value : text;
    private static string GetValue(string text, GroupCollection groups, Dictionary<string, string> list) => GetValue(text, groups, list, out var value) ? value : text;
    public static bool GetValue(string text, GroupCollection groups, Dictionary<string, Color> list, out string value) =>
        (value = string.IsNullOrEmpty(Find(groups, out var key, list)) ? text : Convert(key, text, list[key])) != string.Empty;
    public static bool GetValue(string text, GroupCollection groups, Dictionary<string, string> list, out string value) =>
        (value = string.IsNullOrEmpty(Find(groups, out var key, list)) ? text : Convert(key, text, Color.clear)) != string.Empty;
    private static string Find(IEnumerable groups, out string value, IReadOnlyDictionary<string, Color> list) => value = groups.Cast<Group>().ToList().Find(o => list.ContainsKey(o.Value))?.Value;
    private static string Find(IEnumerable groups, out string value, IReadOnlyDictionary<string, string> list) => value = groups.Cast<Group>().ToList().Find(o => list.ContainsKey(o.Value))?.Value;
    private static string Convert(string str, string text, Color color) =>
        int.TryParse(str, out var value) ? $"<size={Mathf.Clamp(value, MinSize, MaxSize)}>{text}</size>" :
        colors.ContainsKey(str) ? $"<color=#{ColorUtility.ToHtmlStringRGB(colors[str])}> {text} </color>" :
        styles.ContainsKey(str) ? StyleConvert(str, text) :
        variant.ContainsKey(str) ? VariantConvert(str, text) :
        $"<{str}>" + text + $"</{str}>";
    private static string StyleConvert(string str, string text) =>
        styles[str] == $"{FontStyle.BoldAndItalic}" ? $"<b><i>" + text + $"</i></b>" : $"<{styles[str]}>" + text + $"</{styles[str]}>";

    private static string VariantConvert(string str, string text) =>
        variant[str] == "ToUpper" ? text.ToUpper() : variant[str] == "ToTitleCase" ? text.ToTitleCase() : text.ToLower() ;

    public static string Interpolate(this string value) => string.Join("", new Regex(Pattern, RegexOptions.Multiline).Matches(value).OfType<Match>().ToList()
            .ConvertAll(o => GetValue(GetValue(GetValue(GetValue(o.Groups[1].Value, o.Groups, variant), o.Groups, styles), o.Groups, sizeList), o.Groups, colors)));

    public static string Apply(string value) => $":{value};";
    public static string Apply(Color color) => $":{color};";
    public static string Apply(FontStyle fontStyle) => $":{fontStyle};";
    public static string Apply(int i) => $":{i};";
    public static string Apply(string color, string fontStyle, int i) => $":{color}:{fontStyle}:{i};";
    public static string Apply(string color, string fontStyle) => $":{color}:{fontStyle};";
    public static string Apply(string value, int i) => $":{value}:{i};";
    public static string Apply(Color color, FontStyle fontStyle, int i) => $":{color}:{fontStyle}:{i};";
    public static string Apply(Color color, FontStyle fontStyle) => $":{color}:{fontStyle};";
    public static string Apply(Color color, int i) => $":{color}:{i};";
    public static string Apply(FontStyle fontStyle, int i) => $":{fontStyle}:{i};";

    public static void Log() => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, new string('_', 150));
    public static void Log(int i) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, new string('_', i));
    public static void Log(char c) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, new string(c, 150));
    public static void Log(char c, int i) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, new string(c, i));
    public static void Log(Color color) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, $"{new string('_', 150)}:{color};".Interpolate());
    public static void Log(Color color, char c) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, $"{new string(c, 150)}:{color};".Interpolate());
    public static void Log(Color color, char c, int i) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, $"{new string(c, i)}:{color};".Interpolate());
    public static void Log(string @string, Object o = null) => Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, o, @string.Interpolate());
    public static void LogWarning(string @string, Object o = null) => Debug.LogFormat(LogType.Warning, LogOption.NoStacktrace, o, @string.Interpolate());
    public static void LogError(string @string, Object o = null) => Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, o, @string.Interpolate());
    public static void LogAssert(string @string, Object o = null) => Debug.LogFormat(LogType.Assert, LogOption.NoStacktrace, o, @string.Interpolate());
    public static void LogT(string @string) => Debug.Log(@string.Interpolate());
    public static void LogWarningT(string @string) => Debug.LogWarningFormat(@string.Interpolate());
    public static void LogErrorT(string @string) => Debug.LogErrorFormat(@string.Interpolate());
    public static void LogAssertT(string @string) => Debug.LogAssertionFormat(@string.Interpolate());

    public static string ToTitleCase(this string @string) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(@string.ToLower());
}



