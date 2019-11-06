using ObServerPattern.Observer;
using ObServerPattern.Subject;
using System;

namespace ObServerPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var weatherData = new WeatherData();
            var currentConditionDisplay = new CurrentConditionDisplay(weatherData);

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 29.2f);
            weatherData.SetMeasurements(78, 90, 29.2f);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
