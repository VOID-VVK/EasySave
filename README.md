# EasySave-CS

Godot 4.x C# 存档插件 — 基于 Resource 序列化的槽位存档系统。

## 特性

- **槽位管理**: 多槽位存档/读档/删除
- **Resource 序列化**: 利用 Godot 原生 Resource 系统，自动序列化 `[Export]` 字段
- **类型安全**: 泛型 API，继承 `EasySaveData` 定义自己的存档结构
- **跨场景传递**: `LoadToSlot` + `ConsumePendingLoad` 模式
- **信号通知**: `SlotSaved` / `SlotLoaded` 信号
- **零依赖**: 纯 C#，无第三方库

## 安装

### Godot Asset Library

在 Godot 编辑器中搜索 "EasySave" 并安装。

### 手动安装

将 `addons/EasySave/` 复制到项目的 `addons/` 目录，在 Project Settings → Plugins 中启用。

## 快速开始

### 1. 定义存档数据

```csharp
using EasySave.Core;
using Godot;

[GlobalClass]
public partial class MySave : EasySaveData
{
    [Export] public string CurrentScene { get; set; } = "";
    [Export] public int PlayerLevel { get; set; }
    [Export] public Vector2I PlayerPos { get; set; }
}
```

### 2. 存档

```csharp
var save = new MySave
{
    CurrentScene = GetTree().CurrentScene.SceneFilePath,
    PlayerLevel = 10,
    PlayerPos = new Vector2I(5, 3),
};
EasySave.EasySave.Instance!.SaveToSlot(1, save);
```

### 3. 读档

```csharp
var data = EasySave.EasySave.Instance!.LoadFromSlot<MySave>(1);
if (data != null)
    GD.Print($"Level: {data.PlayerLevel}, Pos: {data.PlayerPos}");
```

## API

| 方法 | 说明 |
|------|------|
| `SaveToSlot(slot, data)` | 存档到指定槽位 |
| `LoadFromSlot<T>(slot)` | 从槽位加载数据 |
| `LoadToSlot(slot)` | 加载并暂存（跨场景用） |
| `ConsumePendingLoad<T>()` | 取出暂存数据（取一次清空） |
| `PeekSlot(slot)` | 只读元数据 |
| `HasSave(slot)` | 检查槽位是否有存档 |
| `DeleteSlot(slot)` | 删除槽位 |

## 配置

通过 `EasySaveConfig` Resource 配置：

- `SlotCount` — 槽位数量（默认 3）
- `SaveDirectory` — 存档目录（默认 `user://saves`）

## License

MIT
