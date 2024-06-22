namespace Candy.Service.API.Commons;

public class DateTimeHelper
{
    public static DateTime GetSpecificDayInThisWeek(int targetDay)
    {
        DateTime today = DateTime.Today;
        DayOfWeek currentDayOfWeek = today.DayOfWeek;
        int daysToTarget = (targetDay - (int)currentDayOfWeek + 7) % 7;

        DateTime specificDay = today.AddDays(daysToTarget);
        return specificDay;
    }
    
    public static DateTime GetDayOfCurrentMonth(int day)
    {
        // 获取当前日期
        DateTime currentDate = DateTime.Today;
        // 获取当前月份
        int currentMonth = currentDate.Month;
        // 获取当前年份
        int currentYear = currentDate.Year;
        // 构建目标日期
        DateTime targetDate = new DateTime(currentYear, currentMonth, day);
        return targetDate;
    }
    
    public static DateTime GetDateFromDayOfYear(int dayOfYear)
    {
        var year = DateTime.Now.Year;
        return new DateTime(year, 1, 1).AddDays(dayOfYear - 1);
    }
}