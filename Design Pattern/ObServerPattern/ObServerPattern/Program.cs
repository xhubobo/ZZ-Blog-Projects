using ObServerPattern.Observer;
using ObServerPattern.Subject;
using ObServerPattern.System;
using System;

namespace ObServerPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TestWeatherData:");
            TestWeatherData();
            Console.WriteLine();

            Console.WriteLine("TestWeatherDataX:");
            TestWeatherDataX();
            Console.WriteLine();

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            Console.WriteLine();
        }

        private static void TestWeatherData()
        {
            var weatherData = new WeatherData();
            var currentConditionDisplay = new CurrentConditionDisplay(weatherData);

            weatherData.SetMeasurements(80, 65, 30.4f);
            weatherData.SetMeasurements(82, 70, 29.2f);
            weatherData.SetMeasurements(78, 90, 29.2f);
        }

        private static void TestWeatherDataX()
        {
            var weatherData = new WeatherDataX();
            var currentConditionDisplay = new CurrentConditionDisplayX();

            currentConditionDisplay.Subscribe(weatherData);

            weatherData.SetMeasurements(new WeatherMessage()
            {
                Temperature = 80,
                Humidity = 65,
                Pressure = 30.4f
            });
            weatherData.SetMeasurements(new WeatherMessage()
            {
                Temperature = 82,
                Humidity = 70,
                Pressure = 29.2f
            });
            weatherData.SetMeasurements(new WeatherMessage()
            {
                Temperature = 78,
                Humidity = 90,
                Pressure = 29.2f
            });
            weatherData.EndTransmission();
        }
    }
}
