namespace StravaReporter.Models.ActivityViewModels
{
    public interface IActivityFormatter
    {
        string ToPace(double distance, double time);
        string Icon { get; }
        string UnitOfSpeed { get; }
        string UnitOfMeasurement { get; }
        string UnitOfTime { get; }
        string NameForSpeedOrPace { get; }
        string FormatCadence(double cadence);
        string FormatTime(double timeInSeconds);
        string FormatDistance(double distanceInMeters);
    }
}