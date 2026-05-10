using Godot;
using System;

namespace GodotCombatLab.Core.Utils;

public static class GameLogger
{
    private const string ColorInfo = "color=cyan";
    private const string ColorSuccess = "color=green";
    private const string ColorDebug = "color=gray";
    
    public static void Info(string msg) => 
        GD.PrintRich($"[{ColorInfo}][INFO][/color] {msg}");
    
    public static void Success(string msg) => 
        GD.PrintRich($"[{ColorSuccess}][OK][/color] {msg}");
    
    public static void Warn(string msg) => 
        GD.PushWarning(msg);

    public static void Error(string msg) =>
        GD.PushError(msg);
    
    public static void Debug(string msg) 
    {
    #if DEBUG
        GD.PrintRich($"[{ColorDebug}][DEBUG][/color] {msg}");
    #endif
    }
}
