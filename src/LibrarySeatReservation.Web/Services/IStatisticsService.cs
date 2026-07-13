namespace LibrarySeatReservation.Web.Services;

public interface IStatisticsService
{
    StatisticsResult GetStatistics();
}

public class StatisticsResult
{
    public List<DailyStatItem> DailyStats { get; set; } = new();
    public SeatDistribution SeatDistribution { get; set; } = new();
    public double UtilizationPercentage { get; set; }
}

public class DailyStatItem
{
    public string Date { get; set; } = string.Empty;
    public string DayLabel { get; set; } = string.Empty;
    public int ReservationCount { get; set; }
}

public class SeatDistribution
{
    public int Free { get; set; }
    public int Reserved { get; set; }
    public int Occupied { get; set; }
    public int Total => Free + Reserved + Occupied;
}
