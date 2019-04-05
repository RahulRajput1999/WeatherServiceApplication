using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WeatherServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WeatherServiceBasic" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WeatherServiceBasic.svc or WeatherServiceBasic.svc.cs at the Solution Explorer and start debugging.
    public class WeatherServiceBasic : IWeatherServiceBasic
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Rahul\source\repos\WeatherServiceApplication\WeatherServiceApplication\App_Data\WeatherDatabase.mdf;Integrated Security=True";

        public JavaMessage[] GetData()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            connection.Open();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT TOP 10 * FROM Weather ORDER BY Id DESC;";
            SqlDataReader reader = cmd.ExecuteReader();
            JavaMessage[] messages = new JavaMessage[10];
            int i = 0;
            while (reader.Read())
            {

                JavaMessage m = new JavaMessage();
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
