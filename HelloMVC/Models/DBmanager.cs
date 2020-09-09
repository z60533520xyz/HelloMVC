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
                        brith = reader.GetDateTime(reader.GetOrdinal("bith")).ToString(),
                        email = reader.GetString(reader.GetOrdinal("email")),
                        image = reader.GetString(reader.GetOrdinal("image")),
                        date = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),
                        logindate = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),

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
        public List<Userdata> GetUserdatas(string username)
        {
            List<Userdata> userdatas = new List<Userdata>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM userdata WHERE username='"+ username + "'");
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
                        brith = reader.GetDateTime(reader.GetOrdinal("bith")).ToString(),
                        email = reader.GetString(reader.GetOrdinal("email")),
                        image = reader.GetString(reader.GetOrdinal("image")),
                        date = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),
                        logindate = reader.GetDateTime(reader.GetOrdinal("date")).ToString(),

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
        public List<Conversation> GetConversations(string username,string sationname)
        {
            List<Conversation> conversations = new List<Conversation>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM (SELECT TOP 10 * FROM Conversation WHERE (username IN('"+ sationname + "','"+ username + "') AND sationname IN('"+ sationname + "','"+ username + "')) ORDER BY id DESC) a ORDER BY id");
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
            SqlCommand sqlCommand = new SqlCommand($"SELECT friends FROM tablelike WHERE username='" + username + "'", sqlConnection);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Target card = new Target
                    {
                        target = reader.GetString(reader.GetOrdinal("friends")).Split(","),
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

        public List<Like> GetLike(string username)
        {
            List<Like> likes = new List<Like>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"SELECT * FROM tablelike WHERE username='" + username + "'", sqlConnection);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Like like = new Like
                    {
                        tolikes = reader.GetString(reader.GetOrdinal("tolikes")),
                        forlikes = reader.GetString(reader.GetOrdinal("forlikes")),
                        unlikes = reader.GetString(reader.GetOrdinal("unlikes")),
                        friends = reader.GetString(reader.GetOrdinal("friends")),

                    };
                    likes.Add(like);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return likes;
        }

        public void SetTolike(string username,string tolike)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"UPDATE tablelike SET tolikes=@tolike WHERE username = '" + username + "'", sqlConnection);
            sqlCommand.Parameters.Add("@tolike", SqlDbType.NVarChar);
            sqlCommand.Parameters["@tolike"].Value = tolike;
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Setforlike(string username, string forlike)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"UPDATE tablelike SET forlikes @folike WHERE username = '" + username + "'", sqlConnection);
            sqlCommand.Parameters.Add("@forlike", SqlDbType.NVarChar);
            sqlCommand.Parameters["@forlike"].Value = forlike;
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Setunlike(string username, string unlike)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"UPDATE tablelike SET folikes @Setunlike WHERE username = '" + username + "'", sqlConnection);
            sqlCommand.Parameters.Add("@Setunlike", SqlDbType.NVarChar);
            sqlCommand.Parameters["@Setunlike"].Value = unlike;
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void Setfriend(string username, string friend)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand($"UPDATE tablelike SET folikes @friend WHERE username = '" + username + "'", sqlConnection);
            sqlCommand.Parameters.Add("@friend", SqlDbType.NVarChar);
            sqlCommand.Parameters["@friend"].Value = friend;
            sqlConnection.Open();
            _ = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

    }   
}
