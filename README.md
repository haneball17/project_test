# project_test

## 项目简介
`project_test` 是一个基于 **Godot 4.x .NET（C#）** 的 **2.5D ARPG Demo** 项目骨架。

项目当前重点是先建立可持续迭代的工程基础：
- 清晰的场景与脚本目录结构
- 可运行的角色移动与相机系统
- 可扩展的敌人与战斗反馈框架

## 当前状态（截至 2026-02-28）
- 仓库已初始化 Godot 工程与 `.NET` 配置。
- 业务代码、场景资源、自动化测试尚未落地。
- 已完成文档体系重构，统一入口见本文档。
- 当前本地开发版本：`Godot Engine v4.6.1`。

## 项目目标
1. 交付可运行的 2.5D ARPG Demo（移动、相机、敌人、战斗、基础 UI）。
2. 保证文档与仓库状态一致，避免“文档先行但代码缺失”。
3. 后续支持按里程碑持续迭代（M0 -> M3）。

## 快速开始
1. 安装 `Godot Engine v4.6.1 .NET`（当前项目版本基线）。
2. 用 Godot 打开仓库根目录下的 `project.godot`。
3. 在编辑器菜单执行：`Project -> Tools -> C# -> Create C# Solution`。
4. 按 [`项目开发手册-C#版.md`](./项目开发手册-C#版.md) 的里程碑从 `M0` 开始开发。

## 文档导航
- [项目开发手册-C#版.md](./项目开发手册-C#版.md)：主手册，包含技术基线、里程碑、验收标准。
- [project_test游戏开发手册.md](./project_test游戏开发手册.md)：项目总览与范围边界（非实现细节）。
- [开发职责分工表.md](./开发职责分工表.md)：协作职责、交付节奏与评审清单。
- [文档评估报告.md](./文档评估报告.md)：文档评估结论与优化记录（含外部参考）。

## 外部参考（官方与 GitHub）
- Godot 官方下载页（稳定版信息）：https://godotengine.org/download/windows/
- Godot C# 基础文档：https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html
- Godot Web 导出文档（C# 限制）：https://docs.godotengine.org/en/stable/tutorials/export/exporting_for_web.html
- Godot Demo 仓库：https://github.com/godotengine/godot-demo-projects
