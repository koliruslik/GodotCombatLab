using CombatLab.Core.Interfaces;
using CombatLab.Core.Services;
using CombatLab.Core.Utils;
using Godot;

namespace CombatLab.Core.Managers;

public partial class SceneManager : Node, ISceneManager
{

    public override void _EnterTree()
    {
        ServiceLocator.Register<ISceneManager>(this);
    }
    public override void _Ready()
    {
        GameLogger.Success("SceneManager Ready");
    }
    public void ChangeScene(string scenePath)
    {
        GetTree().CallDeferred(
            SceneTree.MethodName.ChangeSceneToFile,
            scenePath
        );
    }
}