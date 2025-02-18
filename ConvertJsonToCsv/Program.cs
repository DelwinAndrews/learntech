using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static void Main()
    {
        string jsonFilePath = "data.json";
        string csvFilePath = "data.csv";

        // Read the JSON file
        var jsonData = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        var jsonArray = JArray.Parse(jsonData);

        // Create a list to hold the CSV data
        var csvData = new List<string>();

        // Get the headers
        var headers = string.Join(",", ((JObject)jsonArray[0]).Properties().Select(p => p.Name));
        csvData.Add(headers);

        // Get the rows
        foreach (var item in jsonArray)
        {
            var row = string.Join(",", item.Values().Select(v => v.ToString()));
            csvData.Add(row);
        }

        // Write the CSV data to a file
        File.WriteAllLines(csvFilePath, csvData);

        Console.WriteLine("JSON data has been converted to CSV format.");
    }
}
