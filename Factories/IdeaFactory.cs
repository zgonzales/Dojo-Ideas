using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using belt2.Models;
using Microsoft.Extensions.Options;


namespace belt2.Factory{
    public class IdeaFactory : IFactory<Idea>{
        private readonly IOptions<MySqlOptions> mySqlConfig;
        public IdeaFactory(IOptions<MySqlOptions> config)
        {
            mySqlConfig = config;
        }
        internal IDbConnection Connection
        {
            get {
                return new MySqlConnection(mySqlConfig.Value.ConnectionString);
            }
        }
        public void AddIdea(Idea idea){
            using(IDbConnection dbConnection = Connection){
                string query = "Insert into ideas (description, poster, created_at, updated_at) Values (@description, @poster, NOW(), NOW())";
                dbConnection.Open();
                dbConnection.Execute(query, idea);
            }
        }
        public List<Idea> GetAllIdeas(){
            using(IDbConnection dbConnection = Connection){
                string query = "select i.id, i.description, i.poster, u.id as poster_id, group_concat(distinct u.first_name) as likers, count(u.first_name) as num_likes from ideas as i LEFT JOIN likes as l ON l.ideas_id = i.id LEFT JOIN users5 as u ON u.id = l.users5_id GROUP BY i.description";
                dbConnection.Open();
                return dbConnection.Query<Idea>(query).ToList();
            }
        }

        public Idea GetIdeaById(int id){
            using(IDbConnection dbConnection = Connection){
                string query = $"select * from ideas where id = {id}";
                dbConnection.Open();
                return dbConnection.Query<Idea>(query).FirstOrDefault();
            }
        }

        public void LikeIdea(int idea_id, int user_id){
            using(IDbConnection dbConnection = Connection){
                string query = $"Insert into likes (users5_id, Ideas_id) Values ({user_id}, {idea_id})";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

        public void UnlikeIdea(int idea_id, int user_id){
            using(IDbConnection dbConnection = Connection){
                string query =$"DELETE FROM likes WHERE users5_id = {user_id} and ideas_id = {idea_id}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

        public List<User> GetLikersByID(int idea_id){
            using(IDbConnection dbConnection = Connection){
                string query = $"select i.id, i.description, u.first_name, u.id as user_id, u.last_name from ideas as i JOIN likes as l ON l.ideas_id = i.id JOIN users5 as u ON u.id = l.users5_id Where i.id = {idea_id}";
                dbConnection.Open();
                return dbConnection.Query<User>(query).ToList();
            }
        }
        public void DeleteIdeaByID(int idea_id){
            using(IDbConnection dbConnection = Connection){
                string query=$"DELETE from ideas where id={idea_id}";
                dbConnection.Open();
                dbConnection.Execute(query);
            }
        }

    }
}