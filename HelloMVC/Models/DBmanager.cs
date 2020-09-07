using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HelloMVC.Models;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace HelloMVC.Models
{
    public class DBmanager
    {
        //private readonly string ConnStr = @"Data Source=DESKTOP-UGURTF9\SQLEXPRESS;Initial Catalog=project;User ID=sean;Password=123456";
        private readonly string ConnStr = @"Data Source=LCC-PC\SQLEXPRESS;Initial Catalog=pair;User ID=sean;Password=123456";
        public List<Userdata> GetUserdatas()
        {
            List<Userdata> userdatas = new List<Userdata>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM userdata");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Userdata card = new Userdata
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        username = reader.GetString(reader.GetOrdinal("username")),
                        password = reader.GetString(reader.GetOrdinal("password")),
                        name = reader.GetString(reader.GetOrdinal("name")),
                        sex = reader.GetBoolean(reader.GetOrdinal("sex")),
                        address = reader.GetString(reader.GetOrdinal("address")),
                        brith = reader.GetDateTime(reader.GetOrdinal("brith")).ToString(),
                        email = reader.GetString(reader.GetOrdinal("email")),
                        date = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),

                    };
                    userdatas.Add(card);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return userdatas;
        }
        public List<Conversation> GetConversations()
        {
            List<Conversation> conversations = new List<Conversation>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT TOP 10 * FROM Conversation WHERE (username IN('mary','sean') AND sationname IN('mary','sean')) ORDER BY id DESC) a ORDER BY id");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Conversation card = new Conversation
                    {
                        username = reader.GetString(reader.GetOrdinal("username")),
                        sationname = reader.GetString(reader.GetOrdinal("sationname")),
                        text = reader.GetString(reader.GetOrdinal("text")),
                        date = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),

                    };
                    conversations.Add(card);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return conversations;
        }

        public void SendMag(string username,string sationname,string send)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO Conversation(username,sationname,text) VALUES(@username,@sationname,@text)", sqlConnection);
            
            sqlCommand.Parameters.Add("@username", SqlDbType.NVarChar);
            sqlCommand.Parameters["@username"].Value = username;
            sqlCommand.Parameters.Add("@sationname", SqlDbType.NVarChar);
            sqlCommand.Parameters["@sationname"].Value = sationname;
            sqlCommand.Parameters.Add("@text", SqlDbType.NVarChar);
            sqlCommand.Parameters["@text"].Value = send;
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Login> GetLogins(string username)
        {
            List<Login> logins = new List<Login>(); 
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"SELECT username,password FROM userdata WHERE username='"+username+"'", sqlConnection);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Login card = new Login
                    {
                        username = reader.GetString(reader.GetOrdinal("username")),
                        password = reader.GetString(reader.GetOrdinal("password")),
                    };
                    logins.Add(card);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return logins;
        }

        public List<Target> GetTargets(string username)
        {
            List<Target> targets = new List<Target>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"SELECT friend FROM table WHERE username='" + username + "'", sqlConnection);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Target card = new Target
                    {
                        target = reader.GetString(reader.GetOrdinal("username")).Split(","),
                    };
                    targets.Add(card);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return targets;
        }



    }   
}
