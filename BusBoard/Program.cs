using Newtonsoft.Json;
using BusBoard.Models;

namespace BusBoard;

internal class Program
{
    async static Task BusBoard()
    {
        Console.Write("Please select a stop code: ");
        string choice = Console.ReadLine()!;

        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.tfl.gov.uk/StopPoint/");
        // var json = await client.GetStringAsync("490000129R/Arrivals");
        var json = await client.GetStringAsync($"{choice}/Arrivals");
        var predictions = JsonConvert.DeserializeObject<List<ArrivalPrediction>>(json);

        if (predictions is null)
        {
            throw new Exception("Unable to retrieve predictions from JSON response");
        }
        var order = new Dictionary<int, int>();
        for (int i = 0; i < predictions.Count; i++)
        {
            order.Add(i, predictions[i].TimeToStation);
        }
        var value = from element in order
        orderby element.Value ascending
        select element;
        Console.WriteLine("The next 5 buses to arrive are: ");
        foreach (KeyValuePair<int, int> element in value.Take(5))
        {
            Console.WriteLine($"The {predictions[element.Key].LineID} bus to {predictions[element.Key].DestinationName}, arriving in {predictions[element.Key].TimeToStation / 60} minutes");
        }
    }
    
    async static Task Main(string[] args)
    {
        await BusBoard();
    }
}