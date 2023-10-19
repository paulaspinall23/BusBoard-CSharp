namespace BusBoard.Models;

public class ArrivalPrediction
{
    public int TimeToStation { get; set; }
    public int LineID { get; set; }
    public string? DestinationName { get; set; }
}
