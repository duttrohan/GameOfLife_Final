using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayerGameOfLife
{
    public class GridRepository
    {
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GameOfLifeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";

        public void SavePattern(string name, List<(int x, int y)> aliveCells)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // 1. Clear old version (Updated Table Name: Cells)
                    string deleteSql = "DELETE FROM Cells WHERE PatternName = @name";
                    using (SqlCommand delCmd = new SqlCommand(deleteSql, conn))
                    {
                        delCmd.Parameters.AddWithValue("@name", name);
                        delCmd.ExecuteNonQuery();
                    }

                    // 2. Save new coordinates (Updated Table: Cells, Columns: X, Y)
                    foreach (var cell in aliveCells)
                    {
                        string sql = "INSERT INTO Cells (PatternName, X, Y) VALUES (@name, @x, @y)";
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
                // This prevents the app from crashing if the DB is down
                throw new Exception("Error saving to database: " + ex.Message);
            }
        }

        public List<(int x, int y)> LoadPattern(string name)
        {
            var cells = new List<(int x, int y)>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Updated Table: Cells, Columns: X, Y
                    string sql = "SELECT X, Y FROM Cells WHERE PatternName = @name";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 0 is X, 1 is Y
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
    }
}