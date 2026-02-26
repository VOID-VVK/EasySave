using System;
using Godot;
using EasySave.Config;
using EasySave.Core;

namespace EasySave;

[GlobalClass]
public partial class EasySave : Node
{
    public static EasySave? Instance { get; private set; }

    [Export] public EasySaveConfig Config { get; set; } = new();

    [Signal] public delegate void SlotSavedEventHandler(int slot);
    [Signal] public delegate void SlotLoadedEventHandler(int slot);

    private EasySaveData? _pendingLoad;

    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// 存档到指定槽位。自动填充 SaveTime。
    /// </summary>
    public void SaveToSlot(int slot, EasySaveData data)
    {
        data.SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        SaveSlotManager.Save(slot, data, Config.SaveDirectory);
        EmitSignal(SignalName.SlotSaved, slot);
        GD.Print($"[EasySave] 已保存到槽位 {slot}");
    }

    /// <summary>
    /// 从指定槽位加载数据。
    /// </summary>
    public T? LoadFromSlot<T>(int slot) where T : EasySaveData
    {
        return SaveSlotManager.Load<T>(slot, Config.SaveDirectory);
    }

    /// <summary>
    /// 加载槽位数据并存入 PendingLoad，发射 SlotLoaded 信号。
    /// 用于跨场景传递：信号回调中切场景，新场景 _Ready() 调用 ConsumePendingLoad。
    /// </summary>
    public void LoadToSlot(int slot)
    {
        _pendingLoad = SaveSlotManager.Load<EasySaveData>(
            slot, Config.SaveDirectory);
        EmitSignal(SignalName.SlotLoaded, slot);
        GD.Print($"[EasySave] 已加载槽位 {slot}");
    }

    /// <summary>
    /// 取出跨场景传递的数据（取一次就清空）。
    /// </summary>
    public T? ConsumePendingLoad<T>() where T : EasySaveData
    {
        if (_pendingLoad is not T data) return null;
        _pendingLoad = null;
        return data;
    }

    /// <summary>
    /// 只读元数据（Description + SaveTime），不需要知道游戏数据类型。
    /// </summary>
    public EasySaveData? PeekSlot(int slot)
    {
        return SaveSlotManager.Peek(slot, Config.SaveDirectory);
    }

    public bool HasSave(int slot)
    {
        return SaveSlotManager.HasSave(slot, Config.SaveDirectory);
    }

    public void DeleteSlot(int slot)
    {
        SaveSlotManager.Delete(slot, Config.SaveDirectory);
        GD.Print($"[EasySave] 已删除槽位 {slot}");
    }
}
