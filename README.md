# 图书馆座位预约系统

轻量化的图书馆座位预约 Web 系统。学生在线查看座位状态、预约座位、取消预约；图书馆管理员后台管理座位和预约数据，确认签到和释放座位。

> **当前阶段**：Sprint 0 — 开发准备与环境搭建
> **技术栈**：ASP.NET Core MVC (.NET 8) + Razor + SQL Server LocalDB + EF Core + Bootstrap 5

---

## 目录

1. [项目简介](#项目简介)
2. [技术栈](#技术栈)
3. [目录结构](#目录结构)
4. [运行前提](#运行前提)
5. [快速开始](#快速开始)
6. [已实现范围](#已实现范围)
7. [演示账号](#演示账号)
8. [数据库初始化](#数据库初始化)
9. [已知限制](#已知限制)
10. [文档索引](#文档索引)

---

## 项目简介

一个面向校园场景的图书馆座位预约系统，支持学生查看空闲座位、按楼层筛选、预约座位、取消预约；管理员登录后台进行签到确认、释放座位、座位信息管理。

**核心链路**：查看座位 → 预约座位 → 签到入座 → 释放座位（一次完整的座位生命期闭环）。

---

## 技术栈

### 当前已确定

| 层 | 技术 | 版本 |
|----|------|------|
| 运行时 | .NET SDK | 8.0 |
| Web 框架 | ASP.NET Core MVC | 8.0 |
| ORM | Entity Framework Core | 8.0 |
| 数据库 | SQL Server LocalDB | 随 VS2022 |
| 前端框架 | Bootstrap | 5.x |
| 自定义样式 | libseat.css | — |

### 后续计划 / 待生成

| 项目 | 说明 |
|------|------|
| ASP.NET Core MVC 项目 `LibrarySeatSystem.Web.csproj` | Sprint 0 创建 |
| 解决方案文件 `LibrarySeatSystem.sln` | Sprint 0 创建 |
| EF Core 迁移（InitialCreate） | Sprint 0 生成 |
| 种子数据 | Sprint 0 初始化 |

---

## 目录结构

### 当前已存在（本阶段产物）

```
LibrarySeatSystem/
├── .gitignore                     # Git 忽略规则
├── README.md                      # 本文件 — 项目说明与开发指引（持续维护）
│
├── docs/                          # 项目文档目录
│   ├── 01-项目立项单.md            # 立项文档
│   ├── 02-需求分析与MVP确认.md      # 需求分析与 MVP 边界
│   ├── 03-PRD-Lite.md             # PRD 精简版
│   ├── 04-页面树与业务流程.md       # 页面结构与业务链路定义
│   ├── 05-页面卡与UI规范.md         # 页面卡片定义与 UI 设计系统
│   ├── 06-静态原型与原型评审.md      # 静态原型说明
│   ├── 07-系统设计说明.md           # 系统分层架构设计
│   ├── 08-数据库设计.md            # 数据库建模与表结构
│   ├── 09-关键链路详细设计.md       # 核心业务链路逐层展开
│   ├── 10-开发准备与Sprint0.md      # 开发环境准备与 Sprint 0 规划 ← 本阶段产物
│   └── 项目任务板与迭代记录.md       # 任务板与迭代跟踪 ← 本阶段产物
│
├── prototype/                     # 静态原型（HTML 版本）
│   ├── static-v1/                 # 9 页高保真原型
│   │   ├── index.html             # 用户首页
│   │   ├── user/                  # 用户端页面
│   │   ├── admin/                 # 管理端页面
│   │   └── assets/                # CSS + JS 资源
│   └── review-1/                  # 原型评审清单
│       └── 原型评审清单.md
```

### 后续计划 / 待生成

```
src/
└── LibrarySeatSystem.Web/         # ASP.NET Core MVC 项目（Sprint 0 创建）
    ├── Controllers/               # Controller 层
    ├── Models/                    # Entity / ViewModel / Enum
    ├── Services/                  # Service 层（业务逻辑）
    ├── Data/                      # DbContext + Fluent API 配置
    ├── Views/                     # Razor 视图（用户端）
    ├── Areas/Admin/Views/         # Razor 视图（管理端）
    ├── wwwroot/                   # 静态资源（CSS/JS/images）
    ├── Program.cs                 # 启动配置
    └── appsettings.json           # 数据库连接串
```

---

## 运行前提

### 环境要求

| 依赖 | 说明 |
|------|------|
| Windows 10/11 | LocalDB 仅在 Windows 上可用 |
| .NET 8 SDK | [下载地址](https://dotnet.microsoft.com/download/dotnet/8.0) |
| SQL Server LocalDB | 随 Visual Studio 2022 安装附带，或[单独安装](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) |
| Visual Studio 2022（推荐）| 或使用 VS Code + C# Dev Kit |

### 确认环境

```bash
dotnet --version
# 应输出 8.x
```

```bash
dotnet ef --version
# 如果找不到：dotnet tool install --global dotnet-ef
```

---

## 快速开始

> ⚠️ 以下步骤涉及 Sprint 0 待创建文件，当前为"操作指引"阶段，尚未生成 .sln 和 .csproj。

```bash
# 1. 克隆仓库
git clone https://github.com/Dong20051024/library.git
cd library

# 2. 进入项目目录
cd src/LibrarySeatSystem.Web

# 3. 还原依赖
dotnet restore

# 4. 创建数据库（执行 EF Core 迁移）
dotnet ef database update

# 5. 运行项目
dotnet run

# 6. 浏览器访问
# 用户端：http://localhost:5000/
# 管理端：http://localhost:5000/Admin/Login
```

---

## 已实现范围

> 本部分会随开发进度持续更新。

### Sprint 0 — 开发准备（当前）

- [x] 项目文档体系建立（01~09）
- [x] 静态原型完成（9 页 + 设计系统 + 评审清单）
- [x] 系统设计与数据库设计完成
- [x] 关键链路详细设计完成
- [x] Git 仓库初始化
- [ ] 创建 .sln 和 .csproj（进行中）
- [ ] 首次 `dotnet build` 通过
- [ ] 首次 `dotnet run` 成功
- [ ] EF Core 迁移建库
- [ ] 种子数据初始化

### Sprint 1~4 — 业务功能开发（计划中）

详细计划见 [docs/10-开发准备与Sprint0.md](./docs/10-开发准备与Sprint0.md)。

---

## 演示账号

### 学生端

| 账号 | 说明 |
|------|------|
| 学生1~学生5 | 5 个体验账号，在导航栏下拉切换 |

### 管理端

| 用户名 | 密码 | 说明 |
|--------|------|------|
| admin | 123456 | 管理员账号 |

---

## 数据库初始化

本项目使用 EF Core Code-First + 迁移管理数据库：

```bash
# 首次创建迁移
dotnet ef migrations add InitialCreate

# 更新数据库
dotnet ef database update
```

种子数据随 Migration 自动插入，包含：

- 14 个座位（2 个楼层、3 个区域）
- 6 条预约记录（覆盖全部 4 种状态）
- 1 个管理员账号

详细种子数据定义见 [docs/08-数据库设计.md](./docs/08-数据库设计.md) 第 9 节。

---

## 已知限制

| 限制 | 说明 | 是否计划处理 |
|------|------|-------------|
| 密码明文存储 | 课堂项目，LocalDB 本地使用 | 本期不做 |
| 无分页 | 演示数据量小 | 本期不做 |
| 学生自助签到（扫码） | 由管理员后台签到替代 | 本期不做 |
| 无操作日志审计 | 课堂项目不需要 | 本期不做 |
| 多馆区 | 本期只有一个图书馆 | 本期不做 |
| 高并发场景 | 不追求高并发下的完美正确性 | 本期不做 |
| 用户注册与密码找回 | 用体验账号切换替代 | 本期不做 |
| HTTPS | LocalDB 本地演示，HTTP 足够 | 本期不做 |

---

## 文档索引

| 文档 | 内容 | 状态 |
|------|------|------|
| [01-项目立项单](./docs/01-项目立项单.md) | 项目背景、目标、范围、风险评估 | ✅ 完成 |
| [02-需求分析与MVP确认](./docs/02-需求分析与MVP确认.md) | 需求优先级矩阵、MVP 边界 | ✅ 完成 |
| [03-PRD-Lite](./docs/03-PRD-Lite.md) | 用户故事、数据对象、状态机 | ✅ 完成 |
| [04-页面树与业务流程](./docs/04-页面树与业务流程.md) | 9 页页面树、主链路步骤、状态流转 | ✅ 完成 |
| [05-页面卡与UI规范](./docs/05-页面卡与UI规范.md) | 9 张页面卡、设计系统、响应式规范 | ✅ 完成 |
| [06-静态原型与原型评审](./docs/06-静态原型与原型评审.md) | 原型说明、状态覆盖、未接入逻辑清单 | ✅ 完成 |
| [07-系统设计说明](./docs/07-系统设计说明.md) | 分层架构、模块划分、Controller/Service 定义 | ✅ 完成 |
| [08-数据库设计](./docs/08-数据库设计.md) | 3 张数据表 DDL、索引、种子数据 | ✅ 完成 |
| [09-关键链路详细设计](./docs/09-关键链路详细设计.md) | Controller/Service/DataAccess 逐层代码 | ✅ 完成 |
| [10-开发准备与Sprint0](./docs/10-开发准备与Sprint0.md) | 开发环境搭建指引与 Sprint 0 规划 | ✅ 完成 |
| [项目任务板与迭代记录](./docs/项目任务板与迭代记录.md) | 任务追踪、Sprint 规划、迭代记录 | ✅ 完成 |

---

> 项目状态徽章（预留）：[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)]() [![Status](https://img.shields.io/badge/Status-Development-yellow)]()
