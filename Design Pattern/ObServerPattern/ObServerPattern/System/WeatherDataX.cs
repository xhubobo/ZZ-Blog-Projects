using System;
using System.Collections.Generic;

namespace ObServerPattern.System
{
    public class WeatherDataX : IObservable<WeatherMessage>
    {
        private readonly List<IObserver<WeatherMessage>> _observerList;

        public WeatherDataX()
        {
            _observerList = new List<IObserver<WeatherMessage>>();
        }

        /// <summary>
        /// 通知提供程序：某观察程序将要接收通知。
        /// </summary>
        /// <param name="observer">要接收通知的对象。</param>
        /// <returns>使资源释放的观察程序的接口。</returns>
        public IDisposable Subscribe(IObserver<WeatherMessage> observer)
        {
            if(!_observerList.Contains(observer))
            {
                _observerList.Add(observer);
            }
            return new Unsubcriber(_observerList, observer);
        }

        public void SetMeasurements(Nullable<WeatherMessage> message)
        {
            foreach (var observer in _observerList)
            {
                if (!message.HasValue)
                {
                    observer.OnError(new MessageUnknownException());
                }
                else
                {
                    observer.OnNext(message.Value);
                }
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in _observerList.ToArray())
            {
                if (_observerList.Contains(observer))
                {
                    observer.OnCompleted();
                }
            }
            _observerList.Clear();
        }

        private class Unsubcriber : IDisposable
        {
            private List<IObserver<WeatherMessage>> _observerList;
            private IObserver<WeatherMessage> _observer;

            public Unsubcriber(List<IObserver<WeatherMessage>> observerList, IObserver<WeatherMessage> observer)
            {
                _observerList = observerList;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observerList != null && _observerList.Contains(_observer))
                {
                    _observerList.Remove(_observer);
                }
            }
        }
    }
}
