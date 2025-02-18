using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    static void Main()
    {
        //string projectDirectory = Directory.GetCurrentDirectory();
        string jsonFilePath = "C:\\Users\\dandrews2\\source\\repos\\learntech\\ConvertJsonToCsv\\ConvertJsonToCsv\\Json\\AS_flight_Feb18.json";
        string csvFilePath = "C:\\Users\\dandrews2\\source\\repos\\learntech\\ConvertJsonToCsv\\ConvertJsonToCsv\\Csv\\AS_flight_Feb18.csv";

        // Read the JSON file
        var jsonData = File.ReadAllText(jsonFilePath);

        // Parse the JSON data
        var jsonObject = JsonNode.Parse(jsonData).AsObject();
        var jsonArray = jsonObject["flights"].AsArray();

        // Create a list to hold the CSV data
        var csvData = new List<string>();

        // Get the headers
        var headers = string.Join(",", jsonArray[0].AsObject().Select(p => p.Key));
        //csvData.Add(headers);

        var row = string.Join(",", "sourceSystemName", "lastEventType", "flightDate", "flightNumber", "Etd", "Origin", "Destination", "FleetType", "AircraftRegistration", "Delay", "AirlineCode", "AirlineName");
        csvData.Add(row);

        // Get the rows
        foreach (var item in jsonArray)
        {
            JsonObject jsonObj = item["flightDetails"].AsObject();

            JsonObject jsonObjFl = item["flightDetails"].AsObject();



            var sourceSystemName = "";
            var lastEventType = "";
            var flightDate = "";
            var flightNumber = "";
            var actualDepartureStation = "";
            var actualArrivalStation = "";
            var aircraftRegistration = "";
            var fleetType = "";
            var airlineCode = "";
            var airlineName = "";
            var etd = "";
            var delay = "";

            foreach (var flightItem in jsonObj)
            {


                var jsonFltObj = flightItem;

                if (jsonFltObj.Key == "sourceSystemName")
                {
                    sourceSystemName = jsonFltObj.Value.ToString();
                }

                if (jsonFltObj.Key == "scheduledFlightOriginationDateLocal")
                {
                    flightDate = jsonFltObj.Value.ToString();
                }


                // sourceSystemName =  jsonFltObj.Key == "sourceSystemName" ? jsonFltObj.Value.ToString() : string.Empty;

                if (jsonFltObj.Key == "lastEventType")
                {
                    lastEventType = jsonFltObj.Value.ToString();
                }

                if (jsonFltObj.Key == "operatingFlightNumber")
                {
                    flightNumber = jsonFltObj.Value.ToString();
                }


                //read the flight legs
                if (jsonFltObj.Key == "flightLegs")
                {
                    var flightLegs = jsonFltObj.Value.AsArray();

                    //iterate through the flight legs
                    foreach (var flightLeg in flightLegs)
                    {
                        var jsonFltLegObj = flightLeg.AsObject();


                        foreach (var fltLegItem in jsonFltLegObj)
                        {

                            if (fltLegItem.Key == "actualDepartureStation")
                            {
                                actualDepartureStation = fltLegItem.Value["airportCode"].ToString();
                            }

                            if (fltLegItem.Key == "actualArrivalStation")
                            {
                                actualArrivalStation = fltLegItem.Value["airportCode"].ToString();
                            }

                            if (fltLegItem.Key == "estimatedDateTimeUTC")
                            {
                                etd = fltLegItem.Value["out"].ToString();

                                //check for  null value
                                if (fltLegItem.Value["delayInformation"] != null)
                                {

                                    //foreach (var delayItem in fltLegItem.Value["delayInformation"].AsObject())
                                    //{
                                    //    delay = delayItem.Value["description"].ToString();
                                    //}
                                    //delay = fltLegItem.Value["delayInformation"].ToString();
                                }

                                //delay = fltLegItem.Value["delayInformation"].ToString()
                            }

                            if (fltLegItem.Key == "aircraft")
                            {
                                aircraftRegistration = fltLegItem.Value["aircraftRegistration"].ToString();
                                fleetType = fltLegItem.Value["fleetType"].ToString();
                            }

                            //operatingAirlineCode
                            if (fltLegItem.Key == "operatingAirlineCode")
                            {
                                airlineCode = fltLegItem.Value.ToString();
                            }

                            if (fltLegItem.Key == "operatingAirlineName")
                            {
                                airlineName = fltLegItem.Value.ToString();
                            }

                        }


                    }
                }
            }

            row = string.Join(",", sourceSystemName, lastEventType, flightDate, flightNumber, etd, actualDepartureStation, actualArrivalStation, fleetType, aircraftRegistration, delay, airlineCode, airlineName);

            csvData.Add(row);

        }

        // Write the CSV data to a file
        File.WriteAllLines(csvFilePath, csvData);

        Console.WriteLine("JSON data has been converted to CSV format.");
    }
}