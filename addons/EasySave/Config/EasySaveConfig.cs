using Godot;

namespace EasySave.Config;

[GlobalClass]
public partial class EasySaveConfig : Resource
{
    [Export] public int SlotCount { get; set; } = 3;
    [Export] public string SaveDirectory { get; set; } = "user://saves";
}
