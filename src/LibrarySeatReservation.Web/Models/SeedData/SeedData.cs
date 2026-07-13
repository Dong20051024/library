using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entities;
using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Models.SeedData;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Seats.Any())
            return;

        using var transaction = context.Database.BeginTransaction();
        try
        {
            // === 14 个座位 ===
            // 3楼 A区 (6个)
            var seats = new List<Seat>
            {
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-01", Status = SeatStatus.使用中, Facilities = "有插座,靠窗" },
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-02", Status = SeatStatus.已预约, Facilities = "有插座" },
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-03", Status = SeatStatus.空闲, Facilities = "靠窗" },
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-04", Status = SeatStatus.空闲, Facilities = "有插座,电脑位" },
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-05", Status = SeatStatus.空闲, Facilities = "有插座,靠窗" },
                new() { Floor = "3楼", Area = "A区", SeatNumber = "A-06", Status = SeatStatus.空闲, Facilities = "" },
                // 3楼 B区 (4个)
                new() { Floor = "3楼", Area = "B区", SeatNumber = "B-01", Status = SeatStatus.空闲, Facilities = "有插座" },
                new() { Floor = "3楼", Area = "B区", SeatNumber = "B-02", Status = SeatStatus.空闲, Facilities = "靠窗,电脑位" },
                new() { Floor = "3楼", Area = "B区", SeatNumber = "B-03", Status = SeatStatus.空闲, Facilities = "有插座" },
                new() { Floor = "3楼", Area = "B区", SeatNumber = "B-04", Status = SeatStatus.空闲, Facilities = "" },
                // 5楼 C区 (4个)
                new() { Floor = "5楼", Area = "C区", SeatNumber = "C-01", Status = SeatStatus.空闲, Facilities = "有插座,靠窗,电脑位" },
                new() { Floor = "5楼", Area = "C区", SeatNumber = "C-02", Status = SeatStatus.空闲, Facilities = "有插座,靠窗" },
                new() { Floor = "5楼", Area = "C区", SeatNumber = "C-03", Status = SeatStatus.空闲, Facilities = "有插座" },
                new() { Floor = "5楼", Area = "C区", SeatNumber = "C-04", Status = SeatStatus.空闲, Facilities = "靠窗" },
            };

            context.Seats.AddRange(seats);
            context.SaveChanges();

            // === 6 条预约记录 ===
            var now = DateTime.Now;
            var reservations = new List<Reservation>
            {
                // 使用中（A-01 已签到使用中）
                new() { SeatId = seats[0].Id, StudentName = "学生1", StartTime = now.AddHours(-2), EndTime = now.AddHours(1), Status = ReservationStatus.使用中, CreatedAt = now.AddHours(-3) },
                // 待签到（A-02 已预约等待签到）
                new() { SeatId = seats[1].Id, StudentName = "学生2", StartTime = now.AddHours(1), EndTime = now.AddHours(4), Status = ReservationStatus.待签到, CreatedAt = now.AddHours(-1) },
                // 已完成（历史预约 1）
                new() { SeatId = seats[0].Id, StudentName = "学生3", StartTime = now.AddDays(-1).AddHours(9), EndTime = now.AddDays(-1).AddHours(12), Status = ReservationStatus.已完成, CreatedAt = now.AddDays(-1).AddHours(8) },
                // 已完成（历史预约 2）
                new() { SeatId = seats[2].Id, StudentName = "学生4", StartTime = now.AddDays(-2).AddHours(14), EndTime = now.AddDays(-2).AddHours(17), Status = ReservationStatus.已完成, CreatedAt = now.AddDays(-2).AddHours(13) },
                // 已取消
                new() { SeatId = seats[3].Id, StudentName = "学生1", StartTime = now.AddDays(-1).AddHours(14), EndTime = now.AddDays(-1).AddHours(16), Status = ReservationStatus.已取消, CreatedAt = now.AddDays(-1).AddHours(13) },
                // 已取消
                new() { SeatId = seats[4].Id, StudentName = "学生5", StartTime = now.AddDays(-3).AddHours(10), EndTime = now.AddDays(-3).AddHours(12), Status = ReservationStatus.已取消, CreatedAt = now.AddDays(-3).AddHours(9) },
            };

            context.Reservations.AddRange(reservations);
            context.SaveChanges();

            // === 1 个管理员 ===
            if (!context.Admins.Any())
            {
                var admin = new Admin
                {
                    Username = "admin",
                    Password = "123456",
                    DisplayName = "管理员"
                };
                context.Admins.Add(admin);
                context.SaveChanges();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
