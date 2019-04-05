using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WeatherServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallBackService))]
    public interface IWeatherService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        string Subscribe();
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        string Unsubscribe();
        [OperationContract(IsOneWay = true)]
        void Broadcast(Message message);
        /*[OperationContract(IsOneWay = false, IsInitiating = true)]
        Message[] GetHistory();*/
    }
    public interface ICallBackService
    {
        [OperationContract(IsOneWay = true)]
        void Update(Message message);
    }
    [MessageContract]
    public class Message
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
