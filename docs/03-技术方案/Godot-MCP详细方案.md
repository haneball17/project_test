# 🎮 Godot-MCP 完整方案详解

**更新日期：** 2026-03-01
**方案类型：** Godot 专用 AI 集成方案
**推荐指数：** ⭐⭐⭐⭐⭐

---

## 📑 目录

1. [方案概述](#方案概述)
2. [核心架构](#核心架构)
3. [系统要求](#系统要求)
4. [安装指南](#安装指南)
5. [功能详解](#功能详解)
6. [使用示例](#使用示例)
7. [高级配置](#高级配置)
8. [实战案例](#实战案例)
9. [故障排查](#故障排查)
10. [优缺点分析](#优缺点分析)

---

## 🎯 方案概述

### 什么是 Godot-MCP？

**Godot-MCP** 是一个基于 **Model Context Protocol (MCP)** 的服务器，让 AI 助手（如 Claude、Cursor、Cline）能够**直接与 Godot 游戏引擎通信**。

### 核心价值

```
传统方式：AI → 生成代码 → 开发者手动复制到 Godot → 测试
Godot-MCP：AI → 直接操作 Godot → 实时反馈 → 迭代优化
```

### 能做什么？

| 类别 | 功能 | 示例 |
|------|------|------|
| **场景操作** | 创建/保存场景 | "创建一个玩家场景" |
| **节点管理** | 添加/删除/修改节点 | "添加一个 Sprite2D 子节点" |
| **属性设置** | 设置节点属性 | "设置位置为 (100, 200)" |
| **脚本编写** | 生成 GDScript | "编写移动脚本" |
| **项目运行** | 启动/停止调试 | "运行项目并查看输出" |
| **资源管理** | 加载贴图/音频 | "加载 player.png 精灵" |

---

## 🏗️ 核心架构

### 双组件设计

```
┌─────────────────────────────────────────────────────────────┐
│                        Claude/AI 助手                        │
└────────────────────┬────────────────────────────────────────┘
                     │ MCP 协议
                     ↓
┌─────────────────────────────────────────────────────────────┐
│                   MCP Server (Node.js)                      │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ • 工具定义层 (Tool Definitions)                      │   │
│  │ • FastMCP 协议实现                                   │   │
│  │ • TypeScript 类型系统                                │   │
│  └─────────────────────────────────────────────────────┘   │
└────────────────────┬────────────────────────────────────────┘
                     │ WebSocket / stdio
                     ↓
┌─────────────────────────────────────────────────────────────┐
│                   Godot 插件 (GDScript)                     │
│  ┌─────────────────────────────────────────────────────┐   │
│  │ • 命令处理器 (Command Processor)                     │   │
│  │ • WebSocket 服务器                                   │   │
│  │ • UI 面板 (MCP Panel)                                │   │
│  │ • 连接管理器 (Connection Manager)                    │   │
│  └─────────────────────────────────────────────────────┘   │
└────────────────────┬────────────────────────────────────────┘
                     │
                     ↓
┌─────────────────────────────────────────────────────────────┐
│                      Godot Engine 4.x                       │
└─────────────────────────────────────────────────────────────┘
```

### 项目结构

```
Godot-MCP/
├── addons/
│   └── godot_mcp/              # Godot 插件
│       ├── plugin.cfg           # 插件配置
│       ├── plugin.gd            # 插件主脚本
│       ├── commands/            # 命令定义
│       │   ├── node_commands.gd
│       │   ├── scene_commands.gd
│       │   └── resource_commands.gd
│       ├── ui/                  # UI 界面
│       │   └── mcp_panel.tscn
│       └── utils/               # 工具函数
│           ├── websocket_server.gd
│           └── command_parser.gd
├── server/                      # MCP Server
│   ├── src/
│   │   ├── index.ts            # 入口文件
│   │   ├── tools/              # 工具定义
│   │   │   ├── editor.ts
│   │   │   ├── scene.ts
│   │   │   └── node.ts
│   │   └── utils/
│   ├── package.json
│   ├── tsconfig.json
│   └── dist/                   # 编译输出
│       └── index.js
├── docs/                        # 文档
└── TestScene.tscn              # 测试场景
```

---

## 💻 系统要求

| 组件 | 版本要求 | 说明 |
|------|----------|------|
| **Godot Engine** | 4.2+ (推荐 4.3+) | 必须是 .NET 版本（如果使用 C#） |
| **Node.js** | 18.0+ | 用于运行 MCP Server |
| **npm** | 最新版 | 包管理器 |
| **TypeScript** | 最新版 | 开发环境（运行时不需要） |
| **AI 助手** | Claude Desktop / Cursor / Cline | MCP 兼容的工具 |

### 检查环境

```bash
# 检查 Node.js
node --version  # 应显示 v18.0.0 或更高

# 检查 npm
npm --version

# 检查 Godot
godot --version  # 应显示 Godot Engine 4.2+
```

---

## 🚀 安装指南

### 方法一：NPM 快速安装（推荐新手）

#### 步骤 1：安装插件到项目

```bash
# 在您的 Godot 项目目录执行
cd H:/game-dev/project_test

# 安装 Godot-MCP 插件
npx @satelliteoflove/godot-mcp --install-addon .
```

这会自动下载插件到 `addons/godot_mcp/` 目录。

#### 步骤 2：在 Godot 中启用插件

1. 打开 Godot，加载您的项目
2. 菜单：**项目 → 项目设置**
3. 切换到 **插件** 选项卡
4. 找到 **Godot MCP**，勾选 **启用**
5. 在编辑器底部会看到 **MCP** 面板

#### 步骤 3：配置 Claude Desktop（可选）

如果您使用 Claude Desktop，需要配置：

**Windows:** `%APPDATA%\Claude\claude_desktop_config.json`
**macOS:** `~/Library/Application Support/Claude/claude_desktop_config.json`
**Linux:** `~/.config/Claude/claude_desktop_config.json`

```json
{
  "mcpServers": {
    "godot-mcp": {
      "command": "node",
      "args": [
        "H:/game-dev/project_test/addons/godot_mcp/server/dist/index.js"
      ],
      "env": {
        "MCP_TRANSPORT": "stdio"
      }
    }
  }
}
```

---

### 方法二：手动克隆安装（推荐高级用户）

#### 步骤 1：克隆仓库

```bash
# 选择一个目录存放 MCP Server
cd H:/game-dev/
git clone https://github.com/Coding-Solo/godot-mcp.git
cd godot-mcp
```

#### 步骤 2：安装依赖并构建

```bash
cd server
npm install
npm run build
cd ..
```

构建成功后会生成 `server/dist/index.js`。

#### 步骤 3：安装插件到项目

```bash
# 复制插件到您的项目
cp -r addons/godot_mcp H:/game-dev/project_test/addons/
```

#### 步骤 4：在 Godot 中启用插件

同方法一，步骤 2。

#### 步骤 5：配置 Claude Desktop

```json
{
  "mcpServers": {
    "godot-mcp": {
      "command": "node",
      "args": [
        "H:/game-dev/godot-mcp/server/dist/index.js"
      ],
      "env": {
        "MCP_TRANSPORT": "stdio"
      },
      "disabled": false,
      "autoApprove": [
        "launch_editor",
        "run_project",
        "get_debug_output",
        "stop_project",
        "get_godot_version",
        "list_projects",
        "get_project_info",
        "create_scene",
        "add_node",
        "load_sprite",
        "save_scene",
        "get_uid",
        "update_project_uids"
      ]
    }
  }
}
```

**注意：** `autoApprove` 列表中的工具会被自动批准，无需每次确认。

---

### 方法三：Cursor/Cline 配置

如果您使用 **Cursor** 或 **Cline**：

1. 打开 **Settings → MCP**
2. 添加服务器配置：
```json
{
  "godot-mcp": {
    "command": "node",
    "args": ["H:/game-dev/godot-mcp/server/dist/index.js"],
    "env": {
      "MCP_TRANSPORT": "stdio"
    }
  }
}
```

3. 保存并重启编辑器

---

## ⚙️ 功能详解

### 1. 编辑器控制工具 (Editor Control)

| 工具名 | 功能 | 参数 | 返回值 |
|--------|------|------|--------|
| `launch_editor` | 启动 Godot 编辑器 | `project_path: string` | `success: boolean` |
| `run_project` | 运行项目（调试模式） | 无 | `output: string` |
| `stop_project` | 停止运行的项目 | 无 | `success: boolean` |
| `get_debug_output` | 获取控制台输出 | 无 | `output: string` |
| `get_godot_version` | 获取 Godot 版本 | 无 | `version: string` |

**使用示例：**
```
你: "启动我的 Godot 项目"
Claude: [调用 launch_editor]
Claude: "已启动 Godot 编辑器，正在加载项目..."

你: "运行项目并显示控制台输出"
Claude: [调用 run_project]
Claude: [调用 get_debug_output]
Claude: "项目运行中，控制台输出：..."
```

---

### 2. 项目管理工具 (Project Management)

| 工具名 | 功能 | 参数 | 返回值 |
|--------|------|------|--------|
| `list_projects` | 列出目录中的所有项目 | `directory: string` | `projects: array` |
| `get_project_info` | 获取项目详细信息 | `project_path: string` | `info: object` |

**返回的数据结构：**
```json
{
  "name": "project_test",
  "path": "H:/game-dev/project_test",
  "godot_version": "4.4",
  "scenes": ["res://scenes/Main.tscn", "res://scenes/Player.tscn"],
  "scripts": ["res://scripts/PlayerController.cs"],
  "resources": ["res://assets/sprites/"]
}
```

---

### 3. 场景管理工具 (Scene Management)

| 工具名 | 功能 | 参数 | 返回值 |
|--------|------|------|--------|
| `create_scene` | 创建新场景 | `root_type: string, name: string` | `scene_path: string` |
| `save_scene` | 保存场景 | `scene_path: string, variant: string` | `success: boolean` |
| `add_node` | 添加节点到场景 | `parent_path: string, node_type: string, name: string` | `node_path: string` |
| `load_sprite` | 加载精灵到节点 | `node_path: string, texture_path: string` | `success: boolean` |

**使用示例：**
```
你: "创建一个名为 Player 的场景，根节点是 CharacterBody3D"
Claude: [调用 create_scene("CharacterBody3D", "Player")]
Claude: "已创建场景 res://scenes/Player.tscn"

你: "给 Player 添加一个 CollisionShape3D 子节点"
Claude: [调用 add_node("res://scenes/Player.tscn", "CollisionShape3D", "CollisionShape3D")]
Claude: "已添加 CollisionShape3D 到 Player"
```

---

### 4. 高级功能 (Godot 4.4+)

| 工具名 | 功能 | 说明 |
|--------|------|------|
| `get_uid` | 获取文件 UID | 用于资源引用 |
| `update_project_uids` | 更新项目 UID 引用 | 批量更新 |
| `export_mesh_library` | 导出网格库 | 用于 GridMap |

---

## 💡 使用示例

### 示例 1：创建完整的玩家角色

**对话流程：**

```
┌─────────────────────────────────────────────────────────────┐
│ 你：                                                        │
│ "创建一个玩家角色，包含碰撞体、精灵和移动脚本"              │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│ Claude（通过 Godot-MCP）：                                  │
│                                                              │
│ 1. [create_scene]                                          │
│    创建 CharacterBody3D 场景 "Player"                       │
│                                                              │
│ 2. [add_node]                                              │
│    添加 CollisionShape3D                                    │
│    设置形状为 CapsuleShape3D                                │
│                                                              │
│ 3. [add_node]                                              │
│    添加 Sprite3D                                           │
│                                                              │
│ 4. [load_sprite]                                           │
│    加载纹理 (如果提供路径)                                  │
│                                                              │
│ 5. [生成脚本]                                               │
│    创建 PlayerController.gd                                 │
│    编写移动逻辑                                             │
│                                                              │
│ 6. [save_scene]                                            │
│    保存场景                                                │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│ Claude：                                                    │
│ "完成！已创建玩家场景，包含：                               │
│  • CharacterBody3D 根节点                                   │
│  • CollisionShape3D 碰撞体                                  │
│  • Sprite3D 精灵                                            │
│  • PlayerController.gd 移动脚本                             │
│                                                              │
│ 场景已保存到 res://scenes/Player.tscn"                     │
└─────────────────────────────────────────────────────────────┘
```

---

### 示例 2：快速原型开发

```
你：帮我做一个简单的平台跳跃游戏

Claude：好的，我来为您创建：

1. [create_scene] 创建主场景 Main.tscn
2. [add_node] 添加 Player (CharacterBody2D)
3. [编写脚本] 创建移动和跳跃逻辑
4. [create_scene] 创建平台场景
5. [add_node] 多个 StaticBody2D 平台
6. [load_sprite] 加载平台贴图
7. [run_project] 运行测试

已创建基础游戏结构，正在运行...
[显示调试输出]

看起来移动速度有点快，我帮您调整一下...
[修改脚本参数]

您要试试看吗？
```

---

### 示例 3：调试辅助

```
你：游戏运行时玩家会卡住，帮我看看

Claude：让我帮您调试：

1. [run_project] 启动游戏
2. [get_debug_output] 获取控制台输出

输出显示：
```
ERROR: CollisionShape3D not set in Player
```

3. [get_project_info] 检查 Player 场景
4. 分析发现问题：CollisionShape3D 没有设置 shape 属性

5. [add_node] 重新配置碰撞体
6. 设置 shape = CapsuleShape3D
7. [save_scene] 保存

问题已修复！碰撞体现在已正确配置。再次运行测试一下？
```

---

## 🔧 高级配置

### 1. 自定义命令

您可以在 `addons/godot_mcp/commands/` 目录添加自定义命令：

```gdscript
# addons/godot_mcp/commands/custom_commands.gd
extends Reference

class_name CustomCommands

static func create_enemy(position: Vector3, enemy_type: String) -> Dictionary:
    """创建敌人实例"""
    var enemy = preload("res://scenes/Enemy.tscn").instantiate()
    enemy.position = position
    enemy.type = enemy_type

    var scene = Engine.get_main_loop().current_scene
    scene.add_child(enemy)

    return {
        "success": true,
        "node_path": enemy.get_path()
    }
```

### 2. UI 面板自定义

修改 `addons/godot_mcp/ui/mcp_panel.tscn` 来自定义面板界面。

### 3. WebSocket 配置

如果使用 WebSocket 传输（而非 stdio）：

```json
{
  "mcpServers": {
    "godot-mcp": {
      "command": "node",
      "args": ["H:/game-dev/godot-mcp/server/dist/index.js"],
      "env": {
        "MCP_TRANSPORT": "websocket",
        "WS_PORT": "8765"
      }
    }
  }
}
```

---

## 🎮 实战案例

### 案例：2.5D ARPG Demo 开发

**项目目标：** 使用 Godot-MCP + Claude 快速创建 ARPG 原型

#### 第一阶段：基础结构（5分钟）

```
你：创建 2.5D ARPG 的基础场景结构

Claude：
1. ✓ [create_scene] Main.tscn (Node3D)
2. ✓ [add_node] 地面 (MeshInstance3D + PlaneMesh)
3. ✓ [add_node] 方向光 (DirectionalLight3D)
4. ✓ [create_scene] Player.tscn (CharacterBody3D)
5. ✓ [add_node] 碰撞体 (CollisionShape3D)
6. ✓ [add_node] 精灵 (Sprite3D)
7. ✓ [add_node] 相机跟随点 (CameraPivot)

基础场景结构已完成！
```

#### 第二阶段：玩家控制（10分钟）

```
你：添加玩家移动和相机跟随

Claude：
1. [生成脚本] PlayerController.cs
   • 8 方向移动
   • 对角线速度修正
   • 平滑加减速
   • 动画状态管理

2. [生成脚本] CameraFollow.cs
   • 平滑跟随
   • 可配置偏移

3. [附加脚本] 到对应节点

4. [save_scene] 保存所有场景

已完成！可以试试控制玩家移动了。
```

#### 第三阶段：敌人系统（15分钟）

```
你：添加一个会追击玩家的敌人

Claude：
1. [create_scene] Enemy.tscn
2. [add_node] AI 节点
3. [生成脚本] EnemyController.cs
   • 巡逻状态
   • 追击状态
   • 攻击状态
   • 状态机切换

4. [add_node] 攻击检测区域 (Area3D)
5. [配置属性] 追击半径、攻击范围

敌人 AI 已完成！会巡逻和追击玩家。
```

#### 第四阶段：战斗系统（15分钟）

```
你：添加攻击和血量系统

Claude：
1. [生成脚本] HealthComponent.cs
   • 当前/最大血量
   • 受击事件
   • 死亡事件

2. [生成脚本] AttackDetector.cs
   • 攻击判定
   • 伤害计算

3. [create_scene] HUD.tscn
4. [生成 UI] 血条 (TextureProgressBar)
5. [生成 UI] 伤害数字 (DamageNumber)

战斗系统完成！玩家可以攻击敌人并看到伤害反馈。
```

**总耗时：约 45 分钟完成完整原型！**

---

## 🔍 故障排查

### 问题 1：MCP 连接失败

**症状：** Claude 显示 "Failed to connect to MCP server"

**解决方案：**
```bash
# 1. 检查 MCP Server 是否构建
cd H:/game-dev/godot-mcp/server
ls dist/index.js  # 确认文件存在

# 2. 重新构建
npm run build

# 3. 检查配置文件路径
# 确保使用绝对路径，不要用 ~ 或相对路径

# 4. 测试 MCP Server
node dist/index.js
# 应该看到 MCP 服务器启动日志
```

---

### 问题 2：Godot 插件不显示

**症状：** 项目设置中找不到 Godot MCP 插件

**解决方案：**
```
1. 确认插件文件存在：
   addons/godot_mcp/plugin.cfg

2. 检查 plugin.cfg 内容：
   [plugin]

   name="Godot MCP"
   description="MCP integration for Godot"
   author="Your Name"
   version="1.0"
   script="plugin.gd"

3. 重启 Godot 编辑器
```

---

### 问题 3：命令执行失败

**症状：** AI 调用工具时返回错误

**解决方案：**
```
1. 打开 Godot 的 MCP 面板（编辑器底部）
2. 查看错误日志
3. 常见问题：
   • 场景未保存 → 先保存场景
   • 节点路径错误 → 检查节点名称
   • 资源不存在 → 确认资源路径
```

---

### 问题 4：Node.js 权限错误（Windows）

**症状：** PowerShell 报错 "无法加载文件"

**解决方案：**
```powershell
# 以管理员身份运行 PowerShell
Set-ExecutionPolicy RemoteSigned -Scope LocalMachine

# 或者临时允许
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

---

## ⚖️ 优缺点分析

### ✅ 优势

| 优势 | 说明 | 影响力 |
|------|------|--------|
| **深度集成** | AI 直接操作 Godot，无需手动复制代码 | ⭐⭐⭐⭐⭐ |
| **实时反馈** | 立即看到修改结果，快速迭代 | ⭐⭐⭐⭐⭐ |
| **自然语言** | 用对话代替复杂的编辑器操作 | ⭐⭐⭐⭐⭐ |
| **端到端自动化** | 从场景创建到运行的完整流程 | ⭐⭐⭐⭐ |
| **C# 支持** | 通过 Godot 4.x .NET 版本支持 | ⭐⭐⭐⭐ |
| **开源免费** | 完全开源，无使用成本 | ⭐⭐⭐⭐⭐ |

---

### ❌ 劣势

| 劣势 | 说明 | 影响力 |
|------|------|--------|
| **学习曲线** | 需要了解 Node.js 和 MCP 协议 | ⭐⭐⭐ |
| **配置复杂** | 初次安装配置步骤较多 | ⭐⭐⭐ |
| **依赖外部工具** | 需要 Node.js 环境 | ⭐⭐ |
| **调试困难** | 出错时难以定位问题源头 | ⭐⭐⭐ |
| **Godot 版本限制** | 需要 Godot 4.2+ | ⭐⭐ |
| **文档不足** | 中文文档和案例较少 | ⭐⭐⭐⭐ |

---

### 📊 与其他方案对比

| 特性 | Godot-MCP | 纯 Claude Code | Cursor |
|------|-----------|----------------|--------|
| **Godot 集成深度** | ⭐⭐⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐ |
| **上手难度** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **多文件理解** | ⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **实时反馈** | ⭐⭐⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐ |
| **成本** | 免费 | 免费 | $16/月 |

---

## 🎯 推荐使用场景

### ✅ 推荐使用

| 场景 | 理由 |
|------|------|
| **快速原型** | 45 分钟完成可玩 Demo |
| **新手学习** | 自然语言降低学习门槛 |
| **单人开发** | AI 作为协作者提高效率 |
| **重复性任务** | 批量创建相似场景/节点 |
| **调试辅助** | 快速定位和修复问题 |

### ❌ 不推荐使用

| 场景 | 理由 |
|------|------|
| **大型团队** | 配置管理复杂 |
| **已有成熟项目** | 迁移成本高 |
| **纯 C# 开发** | MCP 工具对 C# 支持有限 |
| **离线开发** | 需要稳定的 AI 连接 |

---

## 📚 资源汇总

### 官方资源

| 资源 | 链接 |
|------|------|
| **GitHub 主仓库** | [github.com/Coding-Solo/godot-mcp](https://github.com/Coding-Solo/godot-mcp) |
| **备用仓库** | [github.com/satelliteoflove/godot-mcp](https://github.com/satelliteoflove/godot-mcp) |
| **MCP 官方文档** | [modelcontextprotocol.io](https://modelcontextprotocol.io) |
| **Godot 官方文档** | [docs.godotengine.org](https://docs.godotengine.org) |

### 学习资源

| 资源 | 链接 | 语言 |
|------|------|------|
| **Bilibili 教程** | [BV1P9jRzXEXU](https://www.bilibili.com/video/BV1P9jRzXEXU) | 中文 |
| **实战案例** | Trae + Godot MCP 打砖块游戏 | 中文 |

### 社区讨论

| 平台 | 说明 |
|------|------|
| SegmentFault | 中文技术讨论 |
| Juejin | 中文案例分享 |
| CSDN | 教程和问答 |

---

## 🚀 下一步行动建议

### 如果您决定使用 Godot-MCP：

**第 1 步：环境准备**
```bash
# 检查 Node.js
node --version

# 检查 Godot 版本（需要 4.2+）
# 在 Godot 编辑器中：帮助 → 关于
```

**第 2 步：安装 MCP Server**
```bash
# 克隆仓库
cd H:/game-dev/
git clone https://github.com/Coding-Solo/godot-mcp.git
cd godot-mcp/server
npm install && npm run build
```

**第 3 步：安装插件到项目**
```bash
# 复制插件
cp -r addons/godot_mcp H:/game-dev/project_test/addons/

# 在 Godot 中启用插件
# 项目 → 项目设置 → 插件 → Godot MCP → 启用
```

**第 4 步：配置 Claude Desktop**
```json
{
  "mcpServers": {
    "godot-mcp": {
      "command": "node",
      "args": ["H:/game-dev/godot-mcp/server/dist/index.js"],
      "env": {
        "MCP_TRANSPORT": "stdio"
      },
      "autoApprove": ["create_scene", "add_node", "save_scene"]
    }
  }
}
```

**第 5 步：测试连接**
```
在 Claude 中说："检查 Godot 项目信息"
应该返回项目的详细信息
```

---

## 💬 需要帮助？

如果您在安装或使用过程中遇到问题，可以：

1. **查看本文档的故障排查部分**
2. **告诉我具体错误信息**，我可以帮您诊断
3. **访问 GitHub Issues** 查看是否有类似问题
4. **加入社区讨论** 获取更多帮助

---

**您想要我协助您安装 Godot-MCP 吗？我可以：**
- ✅ 检查您的环境配置
- ✅ 提供详细的安装命令
- ✅ 帮助排查错误
- ✅ 创建自定义命令脚本

**Sources:**
- [Godot-MCP GitHub 主仓库](https://github.com/Coding-Solo/godot-mcp)
- [Godot-MCP 备用仓库](https://github.com/satelliteoflove/godot-mcp)
- [MCP 官方协议文档](https://modelcontextprotocol.io)
- [Godot 官方文档](https://docs.godotengine.org)
- [Bilibili 视频教程](https://www.bilibili.com/video/BV1P9jRzXEXU)
