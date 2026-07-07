# 开发准备与 Sprint 0 —— 图书馆座位预约系统

> **文档定位**：本文档是项目开发前的准备工作清单和 Sprint 0 规划。涵盖仓库结构、分支策略、提交规范、Sprint 1-4 主线规划、里程碑节点。开发者完成 Sprint 0 后应能直接进入业务代码编写。
>
> 前置输入：docs/07-系统设计说明.md、docs/08-数据库设计.md、docs/09-关键链路详细设计.md
> 下一步：开发前一致性总审计

---

## 1. 仓库结构

### 1.1 远端仓库

| 项目 | 地址 |
|------|------|
| GitHub | `https://github.com/Dong20051024/library.git` |
| 本地路径 | `D:\AIWeb\3` |

### 1.2 仓库目录

```
LibrarySeatSystem/                     # Git 仓库根目录
├── .gitignore
├── README.md                          # 项目说明（持续维护）
│
├── docs/                              # 项目文档（全生命周期）
│   ├── 01-项目立项单.md               # 已定稿
│   ├── ...
│   ├── 10-开发准备与Sprint0.md         # 本文件
│   └── 项目任务板与迭代记录.md          # Sprint 规划与追踪
│
├── prototype/                         # 静态原型（交付后归档）
│   └── static-v1/
│
└── src/                               # 源代码目录（Sprint 0 创建）
    └── LibrarySeatSystem.Web/          # ASP.NET Core MVC 项目
```

### 1.3 文件组织原则

| 类型 | 位置 | 说明 |
|------|------|------|
| 项目文档 | `docs/` | 以 `01-`、`02-` 数字前缀编号，按生命周期顺序排列 |
| 原型文件 | `prototype/` | 静态原型 HTML 和相关资源 |
| 源代码 | `src/` | ASP.NET Core MVC 项目文件 |
| 开发报告 | `docs/` | 任务板、迭代记录以中文全名命名 |

---

## 2. 分支策略

### 2.1 分支约定

```
main                          # 生产就绪分支。只允许从 dev 合并
  └── dev                     # 开发主分支。日常开发基于此
       ├── feat/sprint-0      # Sprint 0：项目骨架搭建（合入 dev 后删除）
       ├── feat/sprint-1a     # Sprint 1 第一轮：P0 链路
       ├── feat/sprint-1b     # Sprint 1 第二轮：P0 完善
       ├── feat/sprint-2a     # Sprint 2 第一轮：P1 功能
       ├── feat/sprint-2b     # Sprint 2 第二轮：P1 完善
       ├── feat/sprint-3      # Sprint 3：P2 收尾
       └── feat/sprint-4      # Sprint 4：打磨与文档
```

### 2.2 分支命名规则

| 模式 | 示例 | 说明 |
|------|------|------|
| `main` | `main` | 生产就绪，受保护，不直接提交 |
| `dev` | `dev` | 开发主分支，日常合入目标 |
| `feat/<name>` | `feat/sprint-0` | 功能分支，从 dev 拉出，完成后 PR 合入 dev |
| `fix/<name>` | `fix/login-error` | 修复分支，从 dev 拉出，修复后 PR 合入 dev |
| `docs/<name>` | `docs/readme-update` | 文档更新分支 |

### 2.3 合并流程

```
1. 从 dev 拉出 feat/<name> 分支
2. 在 feat 分支上开发、本地提交
3. 推送到远端 feat/<name>
4. 创建 Pull Request → dev
5. 代码审查通过后合并（Squash merge）
6. 删除远端 feat 分支
```

---

## 3. 提交规范

### 3.1 提交信息格式

```
<type>(<scope>): <简短描述>

<详细说明（可选）>
```

### 3.2 Type 类型

| Type | 使用场景 |
|------|----------|
| `feat` | 新功能、新页面、新模块 |
| `fix` | 修复 bug |
| `docs` | 文档变更（README、docs/ 下文件） |
| `chore` | 项目配置、依赖、工具链 |
| `refactor` | 重构（不改功能，不改 bug） |
| `style` | 仅格式变更（缩进、空格、分号） |
| `db` | 数据库迁移、种子数据变更 |

### 3.3 提交示例

```
feat(seat): 实现座位列表页楼层筛选

- SeatsController.List 新增 floor 参数
- SeatService.GetSeatList 支持按楼层过滤
- 前端下拉框触发 GET /Seat/List?floor=3楼
```

### 3.4 提交粒度

| 原则 | 说明 |
|------|------|
| 每个提交只做一个逻辑变更 | 不要"改了 A 顺带改了 B" |
| 每完成一个任务卡提交一次 | 一次 commit = 一张任务卡 |
| 提交前运行 `dotnet build` | 保证提交的代码能编译通过 |

---

## 4. Sprint 0 目标

### 4.1 Sprint 0 概要

| 项目 | 内容 |
|------|------|
| Sprint 名称 | Sprint 0 — 开发准备与环境搭建 |
| 周期 | 1 轮（一次性完成，不跨多轮） |
| 目标 | 搭建项目脚手架，确保 `dotnet build` 和 `dotnet run` 通过。不写任何业务代码 |
| 最低完成线 | `.sln` + `.csproj` 创建完成，`dotnet build` 通过，首条迁移生成数据库，种子数据成功插入 |
| 是否允许多轮 | 否（Sprint 0 是单轮一次性准备） |
| 前置条件 | 文档体系完成（01~09）、Git 仓库已初始化 |
| 后续入口 | Sprint 1 开始编写业务代码 |

### 4.2 Sprint 0 任务卡

| 编号 | 任务 | 类型 | 依赖 | 交付物 |
|------|------|------|------|--------|
| T00-01 | 创建解决方案 `.sln` 和 Web 项目 `.csproj` | chore | — | `LibrarySeatSystem.sln`、`LibrarySeatSystem.Web.csproj` |
| T00-02 | 安装 NuGet 依赖包 | chore | T00-01 | `Microsoft.EntityFrameworkCore.SqlServer`、`Microsoft.EntityFrameworkCore.Tools` 等 |
| T00-03 | 配置 `appsettings.json` 数据库连接串 | chore | T00-01 | 连接字符串指向 LocalDB |
| T00-04 | 编写 Entity 类（Seat / Reservation / Admin） | db | T00-01 | 三个 Entity 类文件 |
| T00-05 | 编写 Enum 类（SeatStatus / ReservationStatus） | db | T00-04 | 两个枚举文件 |
| T00-06 | 编写 AppDbContext 与 Fluent API 配置 | db | T00-04, T00-05 | `AppDbContext.cs` + 三个 Configuration 文件 |
| T00-07 | 注册 DbContext 和 Service 层 DI | chore | T00-06 | `Program.cs` 中的 DI 配置 |
| T00-08 | 编写种子数据 | db | T00-06 | `SeedData.cs`（14 座位 + 6 预约 + 1 管理员） |
| T00-09 | 生成初始迁移（InitialCreate） | db | T00-06, T00-08 | 迁移文件 + 数据库表 |
| T00-10 | 确认 `dotnet build` 通过 | chore | T00-09 | 编译无错误 |
| T00-11 | 确认 `dotnet run` 启动正常 | chore | T00-10 | 浏览器可访问首页 |
| T00-12 | 配置 `AdminAuthorizeAttribute` | feat | T00-07 | ActionFilter 文件 |

### 4.3 Sprint 0 验收标准

- [ ] `dotnet build` 零错误
- [ ] `dotnet run` 后浏览器访问 `http://localhost:5000/` 返回 200
- [ ] 数据库 `LibrarySeatSystemDb` 已创建，3 张表结构正确
- [ ] 种子数据已插入：14 个座位、6 条预约、1 个管理员
- [ ] `appsettings.json` 连接串可正常访问 LocalDB

---

## 5. Sprint 1-4 主 Sprint 粗计划

### 5.1 Sprint 概览

| Sprint | 名称 | 目标 | 轮次 | 阶段最低完成线 | 里程碑 |
|--------|------|------|------|---------------|--------|
| Sprint 1 | P0 链路闭环 | 实现"空闲→已预约→使用中→空闲"完整链路 | 2 轮（1a, 1b） | 学生可查看座位列表、提交预约；管理员可登录、签到、释放座位 | M1 |
| Sprint 2 | P1 功能完善 | 补全用户首页、我的预约+取消、座位管理 | 2 轮（2a, 2b） | 8 个页面全部可访问，核心操作无空白页 | M2 |
| Sprint 3 | P2 收尾 + 打磨 | 统计页、错误提示、响应式适配 | 1~2 轮 | 无 500 错误，页面显示友好提示 | M3 |
| Sprint 4 | 最终交付 | 全链路回归测试、文档最终化、仓库整理 | 1 轮 | 所有验收通过，README 更新完整 | M4 |

### 5.2 Sprint 1：P0 链路闭环

**周期**：2 轮推进（1a + 1b），每轮完成后可 PR 合入 dev，不等待所有轮次完成再合并。

| 轮次 | 范围 | 任务 |
|------|------|------|
| 1a（第一轮交付） | 管理端 P0 | 管理员登录、预约管理（签到/释放）、种子数据展示 |
| 1b（第二轮交付） | 用户端 P0 | 座位列表、预约提交（含时段冲突校验） |

**阶段最低完成线**：
- 一个完整的"空闲→已预约→使用中→空闲"链路能用浏览器演示
- 无硬编码假数据，所有数据从数据库读取

### 5.3 Sprint 2：P1 功能完善

**周期**：2 轮推进（2a + 2b）。

| 轮次 | 范围 | 任务 |
|------|------|------|
| 2a（第一轮） | 用户端 P1 | 用户首页（体验账号切换）、座位详情页、我的预约+取消预约 |
| 2b（第二轮） | 管理端 P1 | 座位管理（增删改）、删除校验 |

**阶段最低完成线**：
- 全部 8 个 MVP 页面可访问
- 每个页面数据从后端读取，无静态假数据
- 预约取消后座位状态正确恢复
- 座位管理增删改操作正常

### 5.4 Sprint 3：P2 收尾 + 打磨

**周期**：1~2 轮推进。

| 轮次 | 范围 | 任务 |
|------|------|------|
| 3a（第一轮） | 统计页 | `StatisticsService`、`Admin/Statistics` 视图（如时间不够可只实现后端） |
| 3b（第二轮，可选） | 打磨 | 错误提示统一检查、响应式适配调整 |

**阶段最低完成线**：
- 用户端和管理端页面无 500 错误
- 所有错误场景有友好提示（不显示异常堆栈）
- 管理端操作有操作反馈（TempData 消息）

### 5.5 Sprint 4：最终交付

**周期**：1 轮。

| 范围 | 任务 |
|------|------|
| 全链路回归 | 按原型评审清单逐个页面检查功能完整性 |
| 文档最终化 | README 更新、已知限制更新、演示账号确认 |
| 仓库整理 | 清理 .omo/ 等 AI 工具生成文件，确认 .gitignore 完整 |
| 最终提交 | 创建 Git Tag `v1.0.0`，推送所有变更 |

**阶段最低完成线**：
- 所有 Sprint 1-3 任务完成
- 文档与代码一致
- 仓库可 clone 即 run

---

## 6. 里程碑节点

| 里程碑 | 对应 Sprint | 截止条件 | 期望日期 |
|--------|------------|----------|----------|
| **M1: P0 链路可演示** | Sprint 1 | 可在浏览器上演示"空闲→已预约→使用中→空闲"完整流程 | Sprint 1 结束后 |
| **M2: MVP 功能完成** | Sprint 2 | 全部 8 页可访问，所有 P0+P1 功能可用 | Sprint 2 结束后 |
| **M3: 系统稳定可交付** | Sprint 3 | 无未处理的错误页，统计页可用（或标记"开发中"） | Sprint 3 结束后 |
| **M4: 项目正式发布** | Sprint 4 | 全链路回归通过，README 完善，仓库打 Tag | Sprint 4 结束后 |

---

## 7. 本地仓库 / GitHub 初始化建议

### 7.1 已完成操作

```bash
# 1. 创建 .gitignore
# 2. git init
# 3. git add . && git commit -m "chore(init): 初始化项目仓库"
# 4. git remote add origin https://github.com/Dong20051024/library.git
# 5. git branch -M main
# 6. git push -u origin main
# 当前状态：仓库已推送至 GitHub main 分支，包含全部文档和原型
```

### 7.2 首次推送

```bash
# 首次添加所有文件
git add .

# 首次提交
git commit -m "chore(init): 初始化项目仓库

- 项目文档体系（01~09）
- 静态原型 9 页（static-v1）
- 原型评审清单
- .gitignore"

# 推送到远端
git remote add origin https://github.com/Dong20051024/library.git
git branch -M main
git push -u origin main
```

### 7.3 后续每次 Sprint 操作

```bash
# 创建 dev 分支
git checkout -b dev
git push -u origin dev

# 从 dev 拉功能分支
git checkout dev
git pull
git checkout -b feat/sprint-1a

# 完成后合并回 dev
git checkout dev
git merge feat/sprint-1a
git push origin dev

# 删除功能分支
git branch -d feat/sprint-1a
git push origin --delete feat/sprint-1a
```

---

## 8. 默认补足项 / 当前假设

### 8.1 默认补足项

| 补足项 | 内容 | 来源 |
|--------|------|------|
| 分支策略 | `main` / `dev` / `feat/<name>` 三层分支 | 前序文档未指定分支模型 |
| 提交规范 | `<type>(<scope>): <description>` 格式 | 前序文档未指定提交规范 |
| Sprint 0 任务分解 | T00-01 ~ T00-12 共 12 张卡片 | 前序文档提到"搭建项目骨架 + 数据库 + 种子数据"但未分解 |
| Sprint 1-4 轮次设计 | 主 Sprint 内可多轮推进，每轮独立合入 dev | 前序文档只分了三轮开发，未规划 Sprint |
| 里程碑 | M1~M4 四个里程碑 | 前序文档未定义里程碑节点 |
| PR 流程 | Squash merge 策略 | 前序文档未指定合并策略 |

### 8.2 当前假设

| 假设 | 说明 |
|------|------|
| 开发机为 Windows + VS2022 | LocalDB 仅 Windows 可用 |
| 无 CI/CD | 课堂项目，手动构建演示即可 |
| 单人开发 | 分支策略按单人设计，不涉及多人冲突处理 |
| GitHub 远端仓库已创建 | 地址为 `https://github.com/Dong20051024/library.git` |
| 不涉及 release / hotfix 分支 | 项目周期短，不维护多个发布版本 |

---

*文档版本：v1.0 | 前置输入：docs/07-系统设计说明.md、docs/08-数据库设计.md、docs/09-关键链路详细设计.md | 下一步：开发前一致性总审计*
