using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
    public class DBConnection
    {
        private string stringconnection;
        public DBConnection()
        {
           stringconnection = @"Server=localhost;Database=SavingDataApp;User ID=sa;password=World@#2021;";
        }

        //public IDbConnection Connection
        //{
        //    get
        //    {
        //        return new SqlConnection(stringconnection);
        //    }
        //}        

        public IEnumerable<DataSavingModel> getData()
        {
            List<DataSavingModel> listData = new List<DataSavingModel>();
            string sql = @"SELECT * FROM SavingDataApp";

            using (SqlConnection con = new SqlConnection(stringconnection) )
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(sql,con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataSavingModel data = new DataSavingModel()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PhoneNumber = reader.GetString(2),
                            };
                            listData.Add(data);
                        }
                    }
                }
                con.Close();
            }
            return listData;
        }

    }
}
