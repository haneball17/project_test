# project_test

## 项目简介
`project_test` 是一个基于 **Godot 4.x .NET（C#）** 的 **2.5D ARPG Demo** 项目骨架。

项目当前重点是先建立可持续迭代的工程基础：
- 清晰的场景与脚本目录结构
- 可运行的角色移动与相机系统
- 可扩展的敌人与战斗反馈框架

## 当前状态（截至 2026-03-01）
- 仓库已初始化 Godot 工程与 `.NET` 配置
- 已建立完整的文档体系（`docs/` 目录）
- 已完成技术方案调研（Godot-MCP、AI 工具等）
- 业务代码、场景资源尚未落地
- 当前本地开发版本：`Godot Engine v4.6.1`

## 快速开始

### 新手路径
1. 📖 阅读 [开发职责分工表](./docs/02-开发指南/开发职责分工表.md) - 了解协作边界
2. 📖 阅读 [项目开发手册-C#版](./docs/02-开发指南/项目开发手册-C#版.md) - 了解完整开发流程
3. 🚀 开始开发！

### 安装步骤
1. 安装 `Godot Engine v4.6.1 .NET`
2. 用 Godot 打开仓库根目录下的 `project.godot`
3. 在编辑器菜单执行：`Project -> Tools -> C# -> Create C# Solution`
4. 按开发手册的里程碑从 `M0` 开始开发

## 文档导航

### 📚 完整文档中心
详见：[docs/README.md](./docs/README.md)

### 🎯 核心文档
- **[项目开发手册-C#版](./docs/02-开发指南/项目开发手册-C#版.md)** - 主手册，技术基线与里程碑
- **[开发职责分工表](./docs/02-开发指南/开发职责分工表.md)** - 协作职责与评审清单
- **[project_test游戏开发手册](./docs/02-开发指南/project_test游戏开发手册.md)** - 项目总览（原版参考）

### 🔧 技术方案
- **[Godot-MCP详细方案](./docs/03-技术方案/Godot-MCP详细方案.md)** - Godot AI 集成完整指南
- **[Godot-MCP具体用途](./docs/03-技术方案/Godot-MCP具体用途.md)** - 用途说明与对比

### 📊 调研报告
- **[AI Agent游戏开发工具调研报告](./docs/04-调研报告/AI-Agent游戏开发工具调研报告.md)** - 工具对比与选型
- **[文档评估报告](./docs/04-调研报告/文档评估报告.md)** - 文档质量评估与优化记录

## 外部参考（官方）

- Godot 官方下载页：https://godotengine.org/download/windows/
- Godot C# 基础文档：https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html
- Godot Web 导出文档：https://docs.godotengine.org/en/stable/tutorials/export/exporting_for_web.html
- Godot Demo 仓库：https://github.com/godotengine/godot-demo-projects

## 项目目标
1. 交付可运行的 2.5D ARPG Demo（移动、相机、敌人、战斗、基础 UI）
2. 保证文档与仓库状态一致，避免"文档先行但代码缺失"
3. 后续支持按里程碑持续迭代（M0 -> M3）
