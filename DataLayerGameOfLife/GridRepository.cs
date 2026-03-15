using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayerGameOfLife
{
    public class GridRepository : IInitialStateRepository
    {
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GameOfLifeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";

        public void Add(string name, List<(int x, int y)> aliveCells)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Using 'InitialStates' table name from Page 4 of the Exam PDF
                    string deleteSql = "DELETE FROM InitialStates WHERE Name = @name";
                    using (SqlCommand delCmd = new SqlCommand(deleteSql, conn))
                    {
                        delCmd.Parameters.AddWithValue("@name", name);
                        delCmd.ExecuteNonQuery();
                    }

                    foreach (var cell in aliveCells)
                    {
                        string sql = "INSERT INTO InitialStates (Name, X, Y) VALUES (@name, @x, @y)";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@x", cell.x);
                            cmd.Parameters.AddWithValue("@y", cell.y);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving to database: " + ex.Message);
            }
        }

        public List<(int x, int y)> Get(string name)
        {
            var cells = new List<(int x, int y)>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT X, Y FROM InitialStates WHERE Name = @name";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cells.Add((reader.GetInt32(0), reader.GetInt32(1)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading from database: " + ex.Message);
            }
            return cells;
        }

        public void Delete(string name) { /* Logic to delete by name */ }
        public void Update(string name, List<(int x, int y)> cells) { /* Logic to update */ }
    }
}