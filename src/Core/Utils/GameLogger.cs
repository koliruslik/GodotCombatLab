using Godot;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CombatLab.Core.Utils;

[Flags]
public enum LogCategory
{
    Movement = 1,
    Combat   = 2,
    AI       = 4,
    UI       = 8,
    Init     = 16,
    State    = 32,
    CombatDetailed = 64,
    Etc      = 128,
    All = Movement | Combat | AI | UI | Init | State | Etc
}

public static class GameLogger
{
    // Базові кольори для рівнів
    private const string ColorInfo = "cyan";
    private const string ColorSuccess = "green";
    private const string ColorWarn = "yellow";
    private const string ColorError = "red";
    private const string ColorDebug = "gray";
    
    private const string ColorTime = "darkgray";
    private const string ColorClass = "khaki"; 
    private const string ColorCaller = "pink";

    public static LogCategory EnabledCategories = LogCategory.All;
    
    private static string GetTime() => 
        $"[color={ColorTime}]" +
        $"[{DateTime.Now:HH:mm:ss.fff}]" +
        $"[/color]";
    
    private static string GetClassInfo(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return "";
        
        string className = Path.GetFileNameWithoutExtension(filePath);
        return $"[color={ColorClass}]" +
               $"[{className}]" +
               $"[/color]";
    }
    
    private static string GetCallerInfo(string callerName) => 
        string.IsNullOrEmpty(callerName) ? 
            "" :
            $"[color={ColorCaller}]" +
            $"[{callerName}]" +
            $"[/color]";

    private static string GetCategoryColor(LogCategory category)
    {
        return category switch
        {
            LogCategory.Movement       => "lightblue",
            LogCategory.Combat         => "orange",
            LogCategory.AI             => "magenta",
            LogCategory.UI             => "yellowgreen",
            LogCategory.Init           => "white",
            LogCategory.State          => "plum",
            LogCategory.CombatDetailed => "coral",
            LogCategory.Etc            => "gray",
            _                          => "white"
        };
    }

    public static void Info(string msg, [CallerFilePath] string file = "", [CallerMemberName] string caller = "") => 
        GD.PrintRich($"{GetTime()}" +
                     $" [b][color={ColorInfo}]" +
                     $"[INFO][/color]" +
                     $"[/b] " +
                     $"{GetClassInfo(file)}" +
                     $"{GetCallerInfo(caller)} " +
                     $"{msg}");
    
    public static void Success(string msg, [CallerFilePath] string file = "", [CallerMemberName] string caller = "") => 
        GD.PrintRich($"{GetTime()}" +
                     $" [b][color={ColorSuccess}]" +
                     $"[ OK ]" +
                     $"[/color][/b] " +
                     $"{GetClassInfo(file)}" +
                     $"{GetCallerInfo(caller)}" +
                     $" {msg}");
    
    public static void Warn(string msg, [CallerFilePath] string file = "", [CallerMemberName] string caller = "")
    {
        GD.PrintRich($"{GetTime()}" +
                     $" [b][color={ColorWarn}]" +
                     $"[WARN]" +
                     $"[/color][/b] " +
                     $"{GetClassInfo(file)}" +
                     $"{GetCallerInfo(caller)}" +
                     $" [color={ColorWarn}]" +
                     $"{msg}" +
                     $"[/color]");
                     
        string className = Path.GetFileNameWithoutExtension(file);
        GD.PushWarning($"[{className}:{caller}] {msg}");
    }

    public static void Error(string msg, [CallerFilePath] string file = "", [CallerMemberName] string caller = "")
    {
        GD.PrintRich($"{GetTime()}" +
                     $" [b][color={ColorError}]" +
                     $"[ERR!]" +
                     $"[/color][/b] " +
                     $"{GetClassInfo(file)}" +
                     $"{GetCallerInfo(caller)} " +
                     $"[color={ColorError}]" +
                     $"{msg}" +
                     $"[/color]");
                     
        string className = Path.GetFileNameWithoutExtension(file);
        GD.PushError($"[{className}:{caller}] {msg}");
    }
    
    public static void Debug(string msg, LogCategory category, [CallerFilePath] string file = "", [CallerMemberName] string caller = "")
    {
    #if DEBUG
        if ((EnabledCategories & category) == 0) return;
        
        string catColor = GetCategoryColor(category);
        GD.PrintRich($"{GetTime()}" +
                     $" [b][color={ColorDebug}]" +
                     $"[DEBUG]" +
                     $"[/color][/b] " +
                     $"[b][color={catColor}]" +
                     $"[{category}]" +
                     $"[/color][/b] " +
                     $"{GetClassInfo(file)}" +
                     $"{GetCallerInfo(caller)}" +
                     $" {msg}");
    #endif
    }
}