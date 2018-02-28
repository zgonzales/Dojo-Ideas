using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using belt2.Models;
using Microsoft.Extensions.Options;


namespace belt2.Factory{
    public class UserFactory : IFactory<User>{
        private readonly IOptions<MySqlOptions> mySqlConfig;
        public UserFactory(IOptions<MySqlOptions> config)
        {
            mySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mySqlConfig.Value.ConnectionString);
            }
        }

        public void AddNewUser(User user){
            using (IDbConnection dbConnection = Connection){
                string query = "INSERT INTO users5 (first_name, last_name, email, password, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, user);
            }
        }
        public List<User> GetUsers(){
            using (IDbConnection dbConnection = Connection){
                using(IDbCommand command = dbConnection.CreateCommand()){
                    string query = "SELECT * FROM users5";
                    dbConnection.Open();
                    return dbConnection.Query<User>(query).ToList();
                }
            }
        }

        public User GetUserById(int id){
            using (IDbConnection dbConnection = Connection){
                string query = $"select u.id, u.first_name, u.last_name, u.email, COUNT(l.users5_id) as num_likes, COUNT(DISTINCT l.users5_id) as num_posts from users5 as u LEFT join likes as l on l.users5_id = u.id LEFT join ideas as i on l.ideas_id = i.id WHERE u.id = {id};";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }
        public User GetUserByPoster(string poster){
            using(IDbConnection dbConnection = Connection){
                string query = $"Select * from users5 where first_name = '{poster}'";
                dbConnection.Open();
                return dbConnection.Query<User>(query).FirstOrDefault();
            }
        }
    }
}