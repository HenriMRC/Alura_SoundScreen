using Microsoft.Data.SqlClient;
using screensound.models;
using System;
using System.Collections.Generic;

namespace screensound.database
{
    internal class ArtistDAL
    {
        public static IEnumerable<Artista> EnumarateArtists()
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                connection.Open();

                const string COMMAND = "SELECT * FROM Artists";
                using (SqlCommand command = new SqlCommand(COMMAND, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = Convert.ToString(reader["Name"]);
                            string bio = Convert.ToString(reader["Bio"]);
                            int id = Convert.ToInt32(reader["Id"]);
                            Artista artist = new Artista(name, bio)
                            {
                                Id = id,
                            };
                            yield return artist;
                        }
                    }
                }
            }
        }

        public static void Add(Artista artist)
        {
            using (SqlConnection connection = Connection.GetConnection())
            {
                connection.Open();

                const string COMMAND =
                    "INSERT INTO Artists (Name, ProfileImage, Bio) VALUES (@nome, @perfilPadrao, @bio)";
                using (SqlCommand command = new SqlCommand(COMMAND, connection))
                {
                    command.Parameters.AddWithValue("@nome", artist.Nome);
                    command.Parameters.AddWithValue("@perfilPadrao", artist.FotoPerfil);
                    command.Parameters.AddWithValue("@bio", artist.Bio);

                    int result = command.ExecuteNonQuery();

                    Console.WriteLine($"Affected rows {result}");
                }
            }
        }
    }
}
