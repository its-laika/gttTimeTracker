namespace GttTimeTracker.Extensions;

public static class DoubleExtensions
{
    public static (uint Hours, uint Minutes) ToHoursAndMinutes(this double totalMinutes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalMinutes);
        return ((uint)totalMinutes / 60, (uint)Math.Round(totalMinutes % 60));
    }
}