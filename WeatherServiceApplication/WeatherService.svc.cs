using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace WeatherServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WeatherService : IWeatherService
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Rahul\source\repos\WeatherServiceApplication\WeatherServiceApplication\App_Data\WeatherDatabase.mdf;Integrated Security=True";
        ICallBackService icbs = null;
        private static List<ICallBackService> subscribers = new List<ICallBackService>();
        public void Broadcast(Message message)
        {
            foreach (ICallBackService subscriber in subscribers)
            {
                try
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand cmd = new SqlCommand();
                    connection.Open();
                    cmd.Connection = connection;
                    cmd.CommandText = "insert into Weather(place,time,temp,wind_speed,humidity) values (@place,@time,@temp,@windspeed,@humidity)";
                    cmd.Parameters.AddWithValue("@place", message.Place);
                    cmd.Parameters.AddWithValue("@time", message.Time);
                    cmd.Parameters.AddWithValue("@temp", Decimal.Parse(message.Temp));
                    cmd.Parameters.AddWithValue("@windspeed", Decimal.Parse(message.WindSpeed));
                    cmd.Parameters.AddWithValue("@humidity", Decimal.Parse(message.Humidity));
                    int i = cmd.ExecuteNonQuery();
                    if (((IChannel)subscriber).State == CommunicationState.Opened)
                        subscriber.Update(message);
                    else
                        subscribers.Remove(subscriber);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return;
        }

        public string Subscribe()
        {
            try
            {
                icbs = OperationContext.Current.GetCallbackChannel<ICallBackService>();
                subscribers.Add(icbs);
                Console.WriteLine(icbs.ToString());
                return "Successfully subscribed for weather updates!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }


        }

        public string Unsubscribe()
        {
            try
            {
                icbs = OperationContext.Current.GetCallbackChannel<ICallBackService>();
                lock (subscribers)
                {
                    subscribers.Remove(icbs);
                }
                return "Successfully unsubscribed!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Something went wrong! Try after some time.";
            }
        }

        public Message[] GetHistory()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT TOP 10 * FROM Weather ORDER BY Id DESC;";
            SqlDataReader reader = cmd.ExecuteReader();
            Message[] messages = new Message[10];
            int i = 0;
            while (reader.Read())
            {

                Message m = new Message();
                m.Place = reader.GetString(1);
                m.Time = reader.GetTimeSpan(2).ToString();
                m.Temp = reader.GetDecimal(3).ToString();
                m.WindSpeed = reader.GetDecimal(4).ToString();
                m.Humidity = reader.GetDecimal(5).ToString();
                messages[i] = m;
                i++;
            }

            return messages;
        }
    }
}
