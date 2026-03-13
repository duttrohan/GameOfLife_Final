using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DataLayerGameOfLife
{
    public class GridRepository
    {
        // Verified connection string - handles local SQL connection
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=GameOfLifeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";

        public void SavePattern(string name, List<(int x, int y)> aliveCells)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // 1. Clear old version to prevent stacking duplicates
                string deleteSql = "DELETE FROM SavedPatterns WHERE PatternName = @name";
                using (SqlCommand delCmd = new SqlCommand(deleteSql, conn))
                {
                    delCmd.Parameters.AddWithValue("@name", name);
                    delCmd.ExecuteNonQuery();
                }

                // 2. Save new coordinates
                foreach (var cell in aliveCells)
                {
                    string sql = "INSERT INTO SavedPatterns (PatternName, CellX, CellY) VALUES (@name, @x, @y)";
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

        public List<(int x, int y)> LoadPattern(string name)
        {
            var cells = new List<(int x, int y)>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string sql = "SELECT CellX, CellY FROM SavedPatterns WHERE PatternName = @name";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Column 0 is CellX, Column 1 is CellY
                                cells.Add((reader.GetInt32(0), reader.GetInt32(1)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logs the error to the console for debugging
                Console.WriteLine("Database Load Error: " + ex.Message);
            }

            return cells;
        }
    }
}