using Godot;

namespace EasySave.Core;

/// <summary>
/// 存档数据基类。游戏继承此类，加 [Export] 字段定义自己的存档结构。
/// </summary>
[GlobalClass]
public partial class EasySaveData : Resource
{
    [Export] public string Description { get; set; } = "";
    [Export] public string SaveTime { get; set; } = "";
}
