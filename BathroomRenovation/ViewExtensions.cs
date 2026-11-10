namespace BathroomRenovation;

public static class ViewExtensions
{
    public static string Repeat(this string value, int count)
    {
        return string.Concat(Enumerable.Repeat(value, Math.Clamp(count, 0, 5)));
    }
}
