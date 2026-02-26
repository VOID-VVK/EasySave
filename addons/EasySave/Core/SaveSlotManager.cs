using Godot;
using EasySave.Core;

namespace EasySave;

/// <summary>
/// 槽位文件操作。纯静态方法，不持有状态。
/// </summary>
public static class SaveSlotManager
{
    public static string GetSlotPath(int slot, string dir)
    {
        return $"{dir}/save_{slot}.tres";
    }

    public static void Save(int slot, EasySaveData data, string dir)
    {
        DirAccess.MakeDirRecursiveAbsolute(dir);
        var path = GetSlotPath(slot, dir);
        var err = ResourceSaver.Save(data, path);
        if (err != Error.Ok)
            GD.PushError($"[EasySave] 存档失败 slot={slot}: {err}");
    }

    public static T? Load<T>(int slot, string dir) where T : EasySaveData
    {
        var path = GetSlotPath(slot, dir);
        if (!ResourceLoader.Exists(path)) return null;
        return ResourceLoader.Load<T>(path);
    }

    public static EasySaveData? Peek(int slot, string dir)
    {
        var path = GetSlotPath(slot, dir);
        if (!ResourceLoader.Exists(path)) return null;
        return ResourceLoader.Load<EasySaveData>(path);
    }

    public static bool HasSave(int slot, string dir)
    {
        return ResourceLoader.Exists(GetSlotPath(slot, dir));
    }

    public static void Delete(int slot, string dir)
    {
        var path = GetSlotPath(slot, dir);
        if (FileAccess.FileExists(path))
            DirAccess.RemoveAbsolute(path);
    }
}
