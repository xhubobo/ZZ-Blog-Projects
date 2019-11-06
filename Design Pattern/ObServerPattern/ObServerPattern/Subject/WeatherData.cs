using ObServerPattern.Observer;
using System.Collections.Generic;

namespace ObServerPattern.Subject
{
    public class WeatherData : ISubject
    {
        //观察者列表
        private readonly List<IObserver> _observerList;

        //天气数据
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public WeatherData()
        {
            _observerList = new List<IObserver>();
        }

        /// <summary>
        /// 订阅观察者
        /// </summary>
        /// <param name="o">观察者对象</param>
        public void RegisterObserver(IObserver o)
        {
            _observerList.Add(o);
        }

        /// <summary>
        /// 移除观察者
        /// </summary>
        /// <param name="o">观察者对象</param>
        public void RemoveObserver(IObserver o)
        {
            if (_observerList.Contains(o))
            {
                _observerList.Remove(o);
            }
        }

        /// <summary>
        /// 发布数据
        /// </summary>
        public void NotifyObservers()
        {
            foreach (var observer in _observerList)
            {
                observer.Update(_temperature, _humidity, _pressure);
            }
        }

        /// <summary>
        /// 数据发生改变
        /// </summary>
        private void MeasurementChanged()
        {
            NotifyObservers();
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="temperature">温度</param>
        /// <param name="humidity">湿度</param>
        /// <param name="pressure">气压</param>
        public void SetMeasurements(float temperature, float humidity, float pressure)
        {
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;
            MeasurementChanged();
        }
    }
}
