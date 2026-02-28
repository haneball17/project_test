# 🎮 Godot Engine 4.x — 2.5D ARPG 场景 Demo 开发执行方案（C# 版）

> **适用版本：** Godot 4.4+ .NET 版本
> **编程语言：** C# / .NET 8+
> **更新日期：** 2026-03-01
> **状态：** ✅ 已优化

---

## 📑 目录

1. [概要](#概要)
2. [开发阶段划分](#开发阶段划分)
3. [环境准备](#环境准备)
4. [项目基础搭建](#项目基础搭建)
5. [场景与地图制作](#场景与地图制作)
6. [玩家角色控制](#玩家角色控制)
7. [相机系统](#相机系统)
8. [敌人系统与战斗反馈](#敌人系统与战斗反馈)
9. [UI 与反馈效果](#ui-与反馈效果)
10. [Demo 迭代与优化](#demo-迭代与优化)
11. [参考资源](#参考资源)
12. [验收标准](#验收标准)

---

## 🚀 概要

本方案旨在使用 **Godot Engine 4.x .NET 版本** 创建一个 **2.5D 视角 ARPG 场景 Demo**，包含基础场景、主角控制、相机设置、敌人互动和简单战斗反馈。

### 🎯 技术选型理由

| 特性 | 说明 |
|------|------|
| **Godot 4.x .NET** | MIT 开源许可，支持 C# 10/11/12，.NET 6/7/8/9 |
| **C# 语言** | 强类型、成熟生态系统、适合大型项目和团队协作 |
| **2.5D 视角** | 结合 2D 精灵与 3D 空间，营造伪 3D 效果 |
| **性能优势** | C# 在计算密集型场景（AI、物理）中性能优于 GDScript |

### ⚠️ 重要限制

- **Web 平台不支持** C# 项目无法导出到 Web（如需 Web 支持，请使用 GDScript）
- **移动平台支持** Godot 4.2+ 开始支持 Android/iOS 导出（实验性）

---

## 📆 开发阶段划分

```
阶段1: 环境准备        → 安装软件、配置开发环境
阶段2: 项目基础搭建    → 初始化项目、配置结构、输入映射
阶段3: 场景与地图制作  → 创建主场景、搭建基础地图
阶段4: 玩家角色控制    → 实现移动、动画、状态机
阶段5: 相机系统        → 配置 2.5D 视角相机跟随
阶段6: 敌人系统        → AI 逻辑、战斗碰撞、状态管理
阶段7: UI 与反馈      → 血条、伤害数字、音效
阶段8: 迭代与优化      → 性能调优、功能扩展
```

---

## 🧰 1. 环境准备

### ✅ 软件安装清单

| 软件 | 版本要求 | 用途 | 安装方式 |
|------|----------|------|----------|
| **Godot Engine** | 4.4+ .NET 版 | 游戏引擎 | [官网下载 .NET 版本](https://godotengine.org/download) |
| **.NET SDK** | 8.0+ （推荐）| C# 运行时 | [Microsoft 官网](https://dotnet.microsoft.com/download) |
| **IDE（三选一）** | - | 代码编辑与调试 | Visual Studio 2022 / JetBrains Rider / VS Code |
| **Git（可选）** | 最新版 | 版本控制 | [git-scm.com](https://git-scm.com/) |

### ⚙️ 开发环境配置步骤

#### 步骤1：安装 .NET SDK
```bash
# 验证安装
dotnet --version
# 应显示：8.0.x 或更高
```

#### 步骤2：安装 Godot .NET 版本
1. 下载 `Godot_v4.4-*.net.zip`（注意选择 .NET 版本，非标准版）
2. 解压到任意目录
3. 运行 `Godot_v4.4-*.exe` 确认启动正常

#### 步骤3：配置 IDE

**Visual Studio 2022：**
- 安装工作负荷：`.NET 桌面开发`
- 确保安装 `.NET 8 SDK` 组件

**JetBrains Rider：**
- 2024.2+ 版本内置 Godot 支持（无需插件）

**VS Code：**
- 安装扩展：`C# Dev Kit` 和 `C#`

---

## 🌍 2. 项目基础搭建

### 🗂 项目结构（优化版）

```
/project_test
├─ .godot/                 # Godot 编辑器数据（不提交）
├─ scenes/                 # 场景文件
│   ├─ levels/            # 关卡场景
│   │   └─ Main.tscn
│   ├─ characters/        # 角色场景
│   │   ├─ Player.tscn
│   │   └─ Enemy.tscn
│   └─ ui/                # UI 场景
│       └─ HUD.tscn
├─ scripts/                # C# 脚本
│   ├─ autoload/          # 自动加载单例
│   │   └─ EventBus.cs
│   ├─ components/        # 可复用组件
│   │   ├─ HealthComponent.cs
│   │   └─ StateMachine.cs
│   ├─ characters/        # 角色脚本
│   │   ├─ PlayerController.cs
│   │   └─ EnemyController.cs
│   └─ utils/             # 工具类
│       └─ Helpers.cs
├─ assets/                 # 资源文件
│   ├─ sprites/           # 精灵图
│   ├─ audio/             # 音频文件
│   ├─ shaders/           # 着色器
│   └─ data/              # 数据资源（.tres）
├─ ui/                    # UI 控件场景
├─ project.godot          # 项目配置
└─ project_test.sln       # C# 解决方案（自动生成）
```

### 📌 基础配置

#### 2.1 创建 C# 项目

**【开发者B - 手动操作】**

1. 启动 Godot .NET 版本
2. 点击 `新建项目`
3. 选择项目路径：`H:/game-dev/project_test`
4. **关键步骤**：勾选 `渲染器` 选项中的 `Forward+` 或 `Compatibility`
5. **关键步骤**：勾选 `.NET` 支持（会有标识）
6. 点击 `创建并编辑`

#### 2.2 生成 C# 解决方案

**【开发者B - 手动操作】**

```
菜单：Project → Tools → C# → Create C# Solution
```

成功后会在编辑器右上角看到 **构建图标（小锤子）**。

#### 2.3 配置外部编辑器

**【开发者B - 手动操作】**

```
编辑器 → Editor Settings → .NET → Editor → External Editor
选择：Visual Studio / JetBrains Rider / Visual Studio Code
```

#### 2.4 输入映射配置

**【开发者B - 手动操作】**

```
项目 → 项目设置 → 输入映射
```

添加以下输入动作：

| 动作名称 | 默认按键 | 说明 |
|----------|----------|------|
| `move_up` | W / 方向键上 | 向上移动 |
| `move_down` | S / 方向键下 | 向下移动 |
| `move_left` | A / 方向键左 | 向左移动 |
| `move_right` | D / 方向键右 | 向右移动 |
| `attack` | 鼠标左键 / Space | 普通攻击 |
| `dash` | Shift / 鼠标右键 | 冲刺 |
| `interact` | E | 交互 |

**【开发者A - 可协助】**

也可以直接编辑 `project.godot` 文件添加配置，然后用户在编辑器中查看。

#### 2.5 项目设置建议

```ini
# 建议在 project.godot 中配置
[application]

config/name="2.5D ARPG Demo"
config/features=PackedStringArray("4.4", "C#", "Forward Plus")
run/main_scene="res://scenes/levels/Main.tscn"

[display]

window/size/viewport_width=1280
window/size/viewport_height=720
window/size/mode=2  # 窗口模式

[dotnet]

project/assembly_name="ProjectTest"
```

---

## 🧱 3. 场景与地图制作

### 📍 创建主场景

**【开发者B - 手动操作】**

1. 在编辑器中点击 `+` 创建新场景
2. 选择根节点类型：`Node3D`
3. 重命名为 `Main`
4. 保存到 `scenes/levels/Main.tscn`

### 🏗️ 搭建基础地图

**【开发者B - 手动操作】**

1. 在 `Main` 场景中添加地面：
   - 右键 `Main` → `添加子节点`
   - 选择 `MeshInstance3D`
   - 在 Inspector 中设置 Mesh：`New PlaneMesh`
   - 调整 `PlaneMesh` 的 `Size` 为 (20, 20)

2. 添加方向光：
   - 添加子节点 `DirectionalLight3D`
   - 调整旋转以模拟日光角度
   - 开启 `Shadow` → `Enabled`

3. 添加环境光（可选）：
   - 添加子节点 `WorldEnvironment`
   - 创建 `Environment` 资源并配置天空和雾效

### 🏙️ 2.5D 场景实现方案

#### 方案对比

| 方案 | 优点 | 缺点 | 推荐度 |
|------|------|------|--------|
| **Sprite3D + 正交相机** | 完全 2D 渲染，无透视变形 | 失去 3D 深度效果 | ⭐⭐⭐ |
| **Sprite3D + 透视相机（固定角度）** | 真正的 2.5D 效果，有深度感 | 需要精确调整角度 | ⭐⭐⭐⭐⭐ |
| **社区 Node25D 插件** | 编辑器内可视化编辑 | 需要额外依赖，维护性差 | ⭐⭐ |

#### 推荐实现：Sprite3D + 透视相机

**【开发者B - 手动操作】**

1. 创建玩家场景：
   - 新建场景，根节点 `CharacterBody3D`，命名为 `Player`
   - 添加子节点 `Sprite3D`（用于显示角色）
   - 添加子节点 `CollisionShape3D`（用于物理碰撞）
   - 保存到 `scenes/characters/Player.tscn`

2. 调整 Sprite3D：
   - 在 Inspector 中设置 `Texture`（暂无贴图可留空）
   - 设置 `Billboard` 为 `Disabled`
   - 调整 `Pixel Size` 控制显示大小

3. 将 Player 实例化到 Main 场景

4. 创建相机：
   - 在 `Main` 场景中添加子节点 `Camera3D`
   - 位置设置为 `(0, 10, 12)`（在玩家上方）
   - 旋转设置为 `X: -55°`（向下俯视）
   - 勾选 `Keep Aspect` 为 `Keep Height`

---

## 🕹️ 4. 玩家角色控制

### 🧍‍♂️ 角色节点结构

```
Player (CharacterBody3D)
├─ Sprite3D              # 视觉表现
├─ CollisionShape3D      # 物理碰撞
└─ CameraPivot (Node3D)  # 相机旋转中心（可选）
    └─ Camera3D          # 相机（如需独立控制）
```

### 🛠️ 控制逻辑实现

**【开发者A - 代码生成】**

我将为您创建完整的 C# 玩家控制器脚本。

---

### 📄 PlayerController.cs

```csharp
using Godot;

namespace ProjectTest.Scripts.Characters
{
    /// <summary>
    /// 玩家控制器 - 负责玩家移动、动画状态管理和基础交互
    /// </summary>
    public partial class PlayerController : CharacterBody3D
    {
        #region 常量定义

        private const float SPEED = 5.0f;              // 移动速度
        private const float ACCELERATION = 15.0f;       // 加速度
        private const float FRICTION = 20.0f;           // 摩擦力
        private const float JUMP_VELOCITY = 4.5f;       // 跳跃速度（如需要）
        private const float SPRINT_MULTIPLIER = 1.6f;   // 冲刺倍率
        private const float DASH_COOLDOWN = 0.5f;       // 冲刺冷却（秒）

        #endregion

        #region 私有字段

        private Vector3 _velocity;                      // 当前速度
        private float _dashTimer;                       // 冲刺计时器
        private bool _isDashing;                        // 是否正在冲刺
        private Sprite3D? _sprite;                      // 精灵组件引用
        private AnimationTree? _animationTree;          // 动画树引用

        #endregion

        #region Godot 生命周期

        public override void _Ready()
        {
            // 获取组件引用
            _sprite = GetNode<Sprite3D>("Sprite3D");
            _animationTree = GetNodeOrNull<AnimationTree>("AnimationTree");

            GD.Print($"[PlayerController] 玩家初始化完成");
        }

        public override void _PhysicsProcess(double delta)
        {
            // 处理输入
            Vector3 inputDirection = GetInputDirection();

            // 应用移动
            ApplyMovement(inputDirection, (float)delta);

            // 更新动画状态
            UpdateAnimation(inputDirection);

            // 移动角色
            MoveAndSlide();
        }

        #endregion

        #region 输入处理

        /// <summary>
        /// 获取标准化后的输入方向向量
        /// </summary>
        private Vector3 GetInputDirection()
        {
            Vector3 input = Vector3.Zero;

            // 使用 GetAxis 替代组合调用，更简洁
            input.X = Input.GetAxis("move_left", "move_right");
            input.Z = Input.GetAxis("move_up", "move_down");

            // 归一化并应用对角线速度修正
            if (input.LengthSquared() > 0.001f)
            {
                input = input.Normalized();
            }

            return input;
        }

        #endregion

        #region 移动逻辑

        /// <summary>
        /// 应用移动物理计算
        /// </summary>
        private void ApplyMovement(Vector3 inputDirection, float delta)
        {
            float currentSpeed = SPEED;

            // 冲刺处理
            if (_isDashing)
            {
                currentSpeed *= SPRINT_MULTIPLIER;
                _dashTimer -= delta;
                if (_dashTimer <= 0f)
                {
                    _isDashing = false;
                }
            }
            else
            {
                // 检测冲刺输入
                if (Input.IsActionJustPressed("dash"))
                {
                    StartDash();
                }
            }

            // 根据输入计算目标速度
            Vector3 targetVelocity = Vector3.Zero;
            if (inputDirection.LengthSquared() > 0.001f)
            {
                targetVelocity = inputDirection * currentSpeed;
            }

            // 应用加速度/摩擦力
            if (inputDirection.LengthSquared() > 0.001f)
            {
                _velocity = _velocity.Lerp(targetVelocity, ACCELERATION * delta);
            }
            else
            {
                _velocity = _velocity.Lerp(Vector3.Zero, FRICTION * delta);
            }

            // 应用速度
            Velocity = _velocity;
        }

        /// <summary>
        /// 开始冲刺
        /// </summary>
        private void StartDash()
        {
            _isDashing = true;
            _dashTimer = DASH_COOLDOWN;

            // TODO: 播放冲刺特效和音效
            GD.Print("[PlayerController] 冲刺!");
        }

        #endregion

        #region 动画管理

        /// <summary>
        /// 更新动画状态
        /// </summary>
        private void UpdateAnimation(Vector3 inputDirection)
        {
            // TODO: 根据输入方向和速度更新动画参数
            // 示例（需要 AnimationTree 和 BlendSpace）：
            // _animationTree?.Set("parameters/IdleWalk/blend_position", inputDirection.Length());

            // 简单的精灵翻转（基于移动方向）
            if (_sprite != null && inputDirection.X != 0f)
            {
                // _sprite.FlipH = inputDirection.X < 0f;
            }
        }

        #endregion
    }
}
```

---

### 📄 使用说明

**【开发者B - 手动操作】**

1. 将上述代码保存到 `scripts/characters/PlayerController.cs`
2. 在 Godot 中打开 `Player.tscn` 场景
3. 在 Inspector 中点击 `附加脚本`
4. 选择 `PlayerController.cs`
5. 点击编辑器右上角的 **构建图标** 编译 C# 代码
6. 运行场景测试

---

## 📷 5. 相机系统

### 🔍 相机配置

#### 方案A：简单跟随（适合初期）

**【开发者B - 手动操作】**

在 `Main` 场景中：

1. 选中 `Camera3D` 节点
2. 设置位置为 `(0, 12, 10)`
3. 设置旋转为 `(X: -60, Y: 0, Z: 0)`
4. 勾选 `Enabled` 中的 `Doppler Tracking`

#### 方案B：平滑跟随（推荐）

**【开发者A - 代码生成】**

创建相机跟随脚本：

```csharp
using Godot;

namespace ProjectTest.Scripts.Components
{
    /// <summary>
    /// 相机跟随控制器 - 平滑跟随目标节点
    /// </summary>
    public partial class CameraFollow : Node3D
    {
        #region 导出字段（可在 Inspector 中设置）

        [ExportCategory("跟随设置")]
        [Export] public Node3D? Target { get; set; }           // 跟随目标
        [Export] public Vector3 Offset = new(0, 12, 10);       // 相对偏移
        [Export] public float SmoothSpeed = 5.0f;              // 平滑速度

        #endregion

        #region 私有字段

        private Camera3D? _camera;

        #endregion

        #region Godot 生命周期

        public override void _Ready()
        {
            _camera = GetNode<Camera3D>("Camera3D");

            if (_camera == null)
            {
                GD.PushError("[CameraFollow] 未找到 Camera3D 子节点!");
            }
        }

        public override void _Process(double delta)
        {
            if (Target == null || _camera == null) return;

            // 计算目标位置
            Vector3 targetPosition = Target.GlobalPosition + Offset;

            // 平滑插值
            _camera.GlobalPosition = _camera.GlobalPosition.Lerp(
                targetPosition,
                (float)delta * SmoothSpeed
            );

            // 始终看向目标
            _camera.LookAt(Target.GlobalPosition, Vector3.Up);
        }

        #endregion
    }
}
```

**【开发者B - 手动操作】**

1. 在 `Main` 场景中创建新节点 `CameraFollow`（类型 `Node3D`）
2. 添加子节点 `Camera3D`
3. 将 `CameraFollow` 脚本附加到 `CameraFollow` 节点
4. 在 Inspector 中：
   - 设置 `Target` 为 `Player`
   - 调整 `Offset` 和 `SmoothSpeed`

---

## ⚔️ 6. 敌人系统与战斗反馈

### 🤖 敌人节点结构

```
Enemy (CharacterBody3D)
├─ Sprite3D              # 视觉表现
├─ CollisionShape3D      # 物理碰撞
├─ Area3D                # 攻击检测区域
│   └─ CollisionShape3D  # 攻击范围形状
└─ EnemyController.cs    # 控制脚本
```

### 🛠️ 敌人AI实现

**【开发者A - 代码生成】**

```csharp
using Godot;
using System;

namespace ProjectTest.Scripts.Characters
{
    /// <summary>
    /// 敌人控制器 - 实现巡逻、追击和战斗AI
    /// </summary>
    public partial class EnemyController : CharacterBody3D
    {
        #region 常量

        private const float PATROL_SPEED = 2.0f;
        private const float CHASE_SPEED = 4.5f;
        private const float CHASE_RADIUS = 8.0f;
        private const float ATTACK_RANGE = 2.0f;
        private const float PATROL_WAIT_TIME = 2.0f;

        #endregion

        #region 导出字段

        [ExportCategory("AI 设置")]
        [Export] public Node3D? Player { get; set; }
        [Export] public float PatrolRadius = 5.0f;

        #endregion

        #region 私有字段

        private Vector3 _startPosition;
        private Vector3 _targetPatrolPoint;
        private float _patrolTimer;
        private enum EnemyState { Patrol, Chase, Attack }
        private EnemyState _currentState = EnemyState.Patrol;

        #endregion

        #region Godot 生命周期

        public override void _Ready()
        {
            _startPosition = GlobalPosition;
            _targetPatrolPoint = GetRandomPatrolPoint();

            // 如果未指定玩家，尝试自动查找
            if (Player == null)
            {
                Player = GetTree().GetFirstNodeInGroup("player") as Node3D;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Player == null) return;

            // 状态机
            switch (_currentState)
            {
                case EnemyState.Patrol:
                    UpdatePatrol((float)delta);
                    break;
                case EnemyState.Chase:
                    UpdateChase((float)delta);
                    break;
                case EnemyState.Attack:
                    UpdateAttack((float)delta);
                    break;
            }
        }

        #endregion

        #region 状态逻辑

        private void UpdatePatrol(float delta)
        {
            // 检测玩家
            float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
            if (distanceToPlayer < CHASE_RADIUS)
            {
                _currentState = EnemyState.Chase;
                return;
            }

            // 巡逻移动
            Vector3 direction = (_targetPatrolPoint - GlobalPosition);
            if (direction.Length() < 0.5f)
            {
                // 到达巡逻点，等待
                _patrolTimer -= delta;
                if (_patrolTimer <= 0f)
                {
                    _targetPatrolPoint = GetRandomPatrolPoint();
                    _patrolTimer = PATROL_WAIT_TIME;
                }
                Velocity = Vector3.Zero;
            }
            else
            {
                Velocity = direction.Normalized() * PATROL_SPEED;
            }

            MoveAndSlide();
        }

        private void UpdateChase(float delta)
        {
            float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);

            if (distanceToPlayer > CHASE_RADIUS * 1.5f)
            {
                _currentState = EnemyState.Patrol;
                return;
            }

            if (distanceToPlayer < ATTACK_RANGE)
            {
                _currentState = EnemyState.Attack;
                return;
            }

            // 追逐玩家
            Vector3 direction = (Player.GlobalPosition - GlobalPosition);
            Velocity = direction.Normalized() * CHASE_SPEED;
            MoveAndSlide();
        }

        private void UpdateAttack(float delta)
        {
            Velocity = Vector3.Zero;

            float distanceToPlayer = GlobalPosition.DistanceTo(Player.GlobalPosition);
            if (distanceToPlayer > ATTACK_RANGE * 1.2f)
            {
                _currentState = EnemyState.Chase;
            }

            // TODO: 执行攻击逻辑
        }

        #endregion

        #region 辅助方法

        private Vector3 GetRandomPatrolPoint()
        {
            float randomX = (float)(GD.Randf() * 2 - 1) * PatrolRadius;
            float randomZ = (float)(GD.Randf() * 2 - 1) * PatrolRadius;
            return _startPosition + new Vector3(randomX, 0, randomZ);
        }

        #endregion
    }
}
```

### 🥊 攻击碰撞检测

**【开发者A - 代码生成】**

创建攻击检测组件：

```csharp
using Godot;
using System;

namespace ProjectTest.Scripts.Components
{
    /// <summary>
    /// 攻击检测器 - 使用 Area3D 检测攻击命中
    /// </summary>
    public partial class AttackDetector : Area3D
    {
        #region 事件

        /// <summary>
        /// 攻击命中事件
        /// </summary>
        public event Action<Node3D>? OnHit;

        #endregion

        #region 导出字段

        [Export] public int Damage { get; set; } = 10;
        [Export] public float KnockbackForce { get; set; } = 5.0f;

        #endregion

        #region Godot 生命周期

        public override void _Ready()
        {
            // 监听碰撞信号
            BodyEntered += OnBodyEntered;
        }

        #endregion

        #region 碰撞处理

        private void OnBodyEntered(Node3D body)
        {
            // TODO: 验证目标是否可攻击
            GD.Print($"[AttackDetector] 命中: {body.Name}");
            OnHit?.Invoke(body);
        }

        #endregion
    }
}
```

---

## 📊 7. UI 与反馈效果

### 🔹 UI 场景结构

```
HUD (CanvasLayer)
├─ MarginContainer       # 主容器
│   ├─ HBoxContainer     # 顶部栏
│   │   ├─ HealthBar     # 血条
│   │   └─ ScoreLabel    # 分数
│   └─ VBoxContainer     # 底部栏
│       └─ ActionHint    # 操作提示
```

### 🛠️ 血条实现

**【开发者A - 代码生成】**

```csharp
using Godot;

namespace ProjectTest.Scripts.UI
{
    /// <summary>
    /// 血条组件 - 显示当前生命值
    /// </summary>
    public partial class HealthBar : TextureProgressBar
    {
        #region 导出字段

        [Export] public Label? ValueLabel { get; set; }

        #endregion

        #region 公共方法

        public void SetHealth(int current, int max)
        {
            MaxValue = max;
            Value = current;

            if (ValueLabel != null)
            {
                ValueLabel.Text = $"{current} / {max}";
            }
        }

        public void SetHealthPercent(float percent)
        {
            Value = Mathf.Clamp(percent, 0, 100);
        }

        #endregion
    }
}
```

### 🔹 伤害数字效果

**【开发者A - 代码生成】**

```csharp
using Godot;
using System;

namespace ProjectTest.Scripts.UI
{
    /// <summary>
    /// 伤害数字飘字效果
    /// </summary>
    public partial class DamageNumber : Label
    {
        #region 导出字段

        [Export] public float FloatSpeed = 50.0f;
        [Export] public float FadeDuration = 1.0f;

        #endregion

        #region 私有字段

        private float _fadeTimer;

        #endregion

        #region 公共静态方法

        public static void Show(Node3D target, int damage, Node parent)
        {
            var damageNumber = GD.Load<PackedScene>("res://scenes/ui/DamageNumber.tscn").Instantiate<DamageNumber>();
            parent.AddChild(damageNumber);
            damageNumber.GlobalPosition = target.GlobalPosition + Vector3.Up * 2;
            damageNumber.SetText(damage);
        }

        #endregion

        #region 公共方法

        public void SetText(int damage)
        {
            Text = $"-{damage}";
            Modulate = damage > 50 ? Colors.Red : Colors.Yellow;
        }

        #endregion

        #region Godot 生命周期

        public override void _Process(double delta)
        {
            // 向上飘
            GlobalPosition += Vector3.Up * FloatSpeed * (float)delta;

            // 淡出
            _fadeTimer += (float)delta;
            Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 1.0f - (_fadeTimer / FadeDuration));

            if (_fadeTimer >= FadeDuration)
            {
                QueueFree();
            }
        }

        #endregion
    }
}
```

---

## 🧪 8. Demo 迭代与优化

### 📌 性能优化建议

| 优化项 | 具体措施 | 预期收益 |
|--------|----------|----------|
| **对象池** | 敌人、特效使用对象池，避免频繁实例化 | 减少 GC 压力 |
| **精灵图集** | 使用 SpriteFrames 或 AtlasTexture 合并贴图 | 减少 Draw Call |
| **遮挡剔除** | 启用 BVH 和遮挡剔除 | 减少渲染负担 |
| **阴影优化** | 使用 Geometry + Blend Skins 模式 | 提升阴影性能 |
| **音频流** | 使用 OggVorbis 压缩音频 | 减少内存占用 |

### 📌 功能扩展方向

1. **增加状态机系统**
   - 使用状态机管理玩家/敌人状态
   - 支持状态动画过渡

2. **增加敌人种类**
   - 近战型、远程型、飞行型
   - 不同 AI 行为模式

3. **增加技能系统**
   - 技能冷却、消耗
   - 技能特效

4. **增加存档系统**
   - 使用 Godot 的 ConfigFile 或 JSON

---

## 📚 参考资源

### 官方资源

| 资源名称 | 链接 | 说明 |
|----------|------|------|
| Godot 官方文档 | [docs.godotengine.org](https://docs.godotengine.org) | 权威技术文档 |
| Godot C# 基础 | [C# Basics](https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_basics.html) | C# 开发指南 |
| Godot Demo 项目 | [GitHub](https://github.com/godotengine/godot-demo-projects) | 官方示例合集 |

### 社区资源

| 资源名称 | 链接 | 说明 |
|----------|------|------|
| 2.5D 教程 | [PHP中文网](https://www.php.cn/faq/1921632.html) | 2.5D 实现教程 |
| YouTube 教程 | [2.5D Tutorial](https://www.youtube.com/watch?v=YdXtlE3PQYw) | 视频教程 |

### 性能对比

根据社区测试数据：

- **AI 计算密集场景**：C# 稳定 60 FPS，GDScript 降至 42 FPS
- **2D 渲染**：两者相当，GDScript 略快
- **编译速度**：GDScript 更快，C# 首次编译较慢但热重载快

---

## ✅ 验收标准

你的场景 Demo 应达到以下标准：

| 类别 | 验收项 | 状态 |
|------|--------|------|
| **角色控制** | 玩家可 8 方向移动，有平滑过渡 | ⬜ |
| **动画系统** | 角色播放多方向动画 | ⬜ |
| **相机系统** | 相机平滑跟随玩家，呈现 2.5D 视角 | ⬜ |
| **敌人AI** | 简单敌人存在巡逻/追击逻辑 | ⬜ |
| **战斗系统** | 玩家可攻击敌人，有命中反馈 | ⬜ |
| **反馈效果** | 显示伤害数字、血条变化 | ⬜ |
| **UI 系统** | 基础 HUD 界面正常显示 | ⬜ |
| **性能** | 场景帧率稳定在 60 FPS | ⬜ |

---

## 📌 下一步行动

### 【开发者B】立即执行
1. ✅ 确认 Godot .NET 版本已正确安装
2. ✅ 创建新项目并启用 .NET 支持
3. ✅ 生成 C# 解决方案
4. ✅ 配置外部编辑器
5. ✅ 设置输入映射

### 【开发者A】准备就绪
1. 📝 根据需求编写 C# 脚本
2. 📝 创建项目文件结构
3. 📝 提供技术方案建议
4. 📝 代码审查与优化

---

**祝开发顺利！** 🎮

---

**Sources:**
- [Godot 4 C# vs GDScript Performance Comparison](https://www.reddit.com/r/godot/comments/1fzyjgc/comment/lov019u/)
- [Godot C# Basics Official Documentation](https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_basics.html)
- [Godot .NET Setup Guide](https://docs.godotengine.org/en/4.4/tutorials/scripting/c_sharp/c_sharp_basics.html)
- [2.5D Tutorial](https://www.php.cn/faq/1921632.html)
