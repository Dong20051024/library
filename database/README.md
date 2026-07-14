# 数据库初始化说明

## 方式：EF Core Code First

本项目使用 **Entity Framework Core Code First** 方式管理数据库。所有表结构由 C# 实体类（`Models/Entities/`）和 Fluent API 配置（`Data/`）定义，通过迁移文件（`Migrations/`）同步到 SQL Server LocalDB。

---

## 表结构（3 张表）

| 表名 | 说明 | 主要字段 |
|------|------|----------|
| `Seats` | 座位表 | Id, Floor, Area, SeatNumber, Status, Facilities, IsActive |
| `Reservations` | 预约表 | Id, SeatId(FK), StudentName, StartTime, EndTime, Status, CreatedAt |
| `Admins` | 管理员表 | Id, Username, Password, DisplayName |

完整字段定义见 `docs/08-数据库设计.md`。

---

## 首次建库建表

### 前提条件

- Windows 10/11
- SQL Server LocalDB（随 Visual Studio 2022 安装，或[单独安装](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)）
- .NET 8 SDK + dotnet-ef 工具

### 步骤

```bash
# 1. 进入项目目录
cd src/LibrarySeatReservation.Web

# 2. 确认 dotnet-ef 已安装
dotnet ef --version
# 如果未安装：dotnet tool install --global dotnet-ef

# 3. 应用迁移，创建数据库并插入种子数据
dotnet ef database update
```

该命令会：

1. 读取 `Migrations/` 下的迁移文件
2. 在 LocalDB 中创建 `LibrarySeatReservationDb` 数据库
3. 创建 `Seats`、`Reservations`、`Admins` 三张表及索引/外键

### 数据库连接串

位置：`src/LibrarySeatReservation.Web/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LibrarySeatReservationDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

---

## 种子数据

种子数据通过 `Models/SeedData/SeedData.cs` 中的 `SeedData.Initialize()` 方法在首次启动时自动插入。代码在 `Program.cs` 的启动流程中调用：

```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Initialize(dbContext);
}
```

**插入规则**：检查 `Seats` 表是否有记录，若已存在则跳过（幂等）。

### 座位数据（14 条）

| 楼层 | 区域 | 座位号 | 初始状态 | 设施标签 |
|------|------|--------|----------|----------|
| 3楼 | A区 | A-01 | 使用中 | 有插座,靠窗 |
| 3楼 | A区 | A-02 | 已预约 | 有插座 |
| 3楼 | A区 | A-03 | 空闲 | 靠窗 |
| 3楼 | A区 | A-04 | 空闲 | 有插座,电脑位 |
| 3楼 | A区 | A-05 | 空闲 | 有插座,靠窗 |
| 3楼 | A区 | A-06 | 空闲 | — |
| 3楼 | B区 | B-01 | 空闲 | 有插座 |
| 3楼 | B区 | B-02 | 空闲 | 靠窗,电脑位 |
| 3楼 | B区 | B-03 | 空闲 | 有插座 |
| 3楼 | B区 | B-04 | 空闲 | — |
| 5楼 | C区 | C-01 | 空闲 | 有插座,靠窗,电脑位 |
| 5楼 | C区 | C-02 | 空闲 | 有插座,靠窗 |
| 5楼 | C区 | C-03 | 空闲 | 有插座 |
| 5楼 | C区 | C-04 | 空闲 | 靠窗 |

### 预约数据（6 条，覆盖 4 种状态）

| 学生 | 座位 | 时段 | 状态 |
|------|------|------|------|
| 学生1 | A-01（使用中） | 当前−2h → 当前+1h | **使用中** |
| 学生2 | A-02（已预约） | 当前+1h → 当前+4h | **待签到** |
| 学生3 | A-01 | 昨天 9:00→12:00 | **已完成** |
| 学生4 | A-03 | 前天 14:00→17:00 | **已完成** |
| 学生1 | A-04 | 昨天 14:00→16:00 | **已取消** |
| 学生5 | A-05 | 3天前 10:00→12:00 | **已取消** |

### 管理员账号（1 条）

| 用户名 | 密码 | 显示名 |
|--------|------|--------|
| admin | 123456 | 管理员 |

---

## 重置数据库

如需清空数据重新开始：

```bash
# 删除数据库
dotnet ef database drop

# 重新创建
dotnet ef database update
```

---

## 不依赖 SQL Server Management Studio

本项目不需要额外安装 SSMS。只要 LocalDB 可用，`dotnet ef database update` 命令即可完成全部建库工作。
