using ObServerPattern.Observer;
using System;

namespace ObServerPattern.System
{
    class CurrentConditionDisplayX : IObserver<WeatherMessage>, IDisplayElement
    {
        private IDisposable _unsubscribe;

        private float _temperature;
        private float _humidity;

        public void Subscribe(IObservable<WeatherMessage> provider)
        {
            if (provider != null)
            {
                _unsubscribe = provider.Subscribe(this);
            }
        }

        public void Unsubscribe()
        {
            _unsubscribe?.Dispose();
            _unsubscribe = null;
        }

        public void OnCompleted()
        {
            Console.WriteLine("CurrentConditionDisplayX: OnCompleted");
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine("CurrentConditionDisplayX: OnError");
        }

        public void OnNext(WeatherMessage value)
        {
            _temperature = value.Temperature;
            _humidity = value.Humidity;
            Display();
        }

        public void Display()
        {
            Console.WriteLine($"Current Conditions: {_temperature}F degress and {_humidity}% humidity");
        }
    }
}
