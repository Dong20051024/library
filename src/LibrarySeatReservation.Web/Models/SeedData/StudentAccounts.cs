namespace LibrarySeatReservation.Web.Models.SeedData;

public static class StudentAccounts
{
    private static readonly string[] Accounts = { "学生1", "学生2", "学生3", "学生4", "学生5" };

    public static string[] GetAccounts() => Accounts;
}
