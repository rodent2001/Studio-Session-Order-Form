namespace StudioSessionCalc;

public class Studio
{
    public string StudioName { get; set; } = string.Empty;
    public StudioProducts StudioProducts { get; set; }
    public StudioCustomers StudioCustomers { get; set; }
    public StudioSessionDurations StudioSessionDurations { get; set; }

    public Studio(string studioName)
    {
        StudioName = studioName;

        StudioProducts = CollectionsMaker.MakeStudioProducts();
        StudioCustomers = CollectionsMaker.MakeStudioCustomers();
        StudioSessionDurations = CollectionsMaker.MakeStudioSessionDurations();
    }
}

