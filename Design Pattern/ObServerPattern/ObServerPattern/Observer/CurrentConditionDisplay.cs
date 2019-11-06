using ObServerPattern.Subject;
using System;

namespace ObServerPattern.Observer
{
    public class CurrentConditionDisplay : IObserver, IDisplayElement
    {
        private readonly ISubject _weatherData;

        private float _temperature;
        private float _humidity;

        public CurrentConditionDisplay(ISubject weatherData)
        {
            _weatherData = weatherData;
            _weatherData?.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _temperature = temperature;
            _humidity = humidity;
            Display();
        }

        public void Display()
        {
            Console.WriteLine($"Current Conditions: {_temperature}F degress and {_humidity}% humidity");
        }
    }
}
