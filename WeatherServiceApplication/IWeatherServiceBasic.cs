using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WeatherServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWeatherServiceBasic" in both code and config file together.
    [ServiceContract]
    public interface IWeatherServiceBasic
    {
        [OperationContract]
        JavaMessage[] GetData();
    }
    [MessageContract]
    public class JavaMessage
    {
        private string place;
        private string time;
        private string temp;
        private string wind_speed;
        private string humidity;

        [MessageBodyMember]
        public string Place
        {
            get { return this.place; }
            set { this.place = value; }
        }
        [MessageBodyMember]
        public string Time
        {
            get { return this.time; }
            set { this.time = value; }
        }
        [MessageBodyMember]
        public string Temp
        {
            get { return this.temp; }
            set { this.temp = value; }
        }
        [MessageBodyMember]
        public string WindSpeed
        {
            get { return this.wind_speed; }
            set { this.wind_speed = value; }
        }
        [MessageBodyMember]
        public string Humidity
        {
            get { return this.humidity; }
            set { this.humidity = value; }
        }
    }
}
