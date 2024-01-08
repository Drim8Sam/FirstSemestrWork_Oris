using System.Data;
using Npgsql;
using NpgsqlTypes;
using FirstSemestrWork.Model;

namespace FirstSemestrWork;

public class DbContext
{
    private const string DbConnectionString =
        "Host=localhost;Username=postgres;Password=1234;Database=FirstSemestrWork;Port=5433";

    private readonly NpgsqlConnection _dbConnection = new NpgsqlConnection(DbConnectionString);
    
    public async Task AddPersonage(string namePersonage, string imagePath, CancellationToken ctx)
    {
        await _dbConnection.OpenAsync(ctx);

        try
        {
            await using var command = _dbConnection.CreateCommand();

            command.CommandText = "INSERT INTO Personage (name_personage, image_path, isActive) " +
                                  "VALUES (@name_personage, @image_path, true) RETURNING id_personage";

            command.Parameters.AddWithValue("name_personage", namePersonage);
            command.Parameters.AddWithValue("image_path", imagePath);

            // Получаем результат вставки
            var id = await command.ExecuteScalarAsync(ctx);
            if (id != null && id != DBNull.Value)
            {
                Console.WriteLine($"Personage added successfully with id: {id}");
            }
            else
            {
                Console.WriteLine("Error retrieving id after insertion.");
            }
        }
        finally
        {
            await _dbConnection.CloseAsync();
        }
    }


    public async Task DeletePersonage(string namePersonage, CancellationToken ctx)
    {
        try
        {
            await _dbConnection.OpenAsync(ctx);

            await using var command = _dbConnection.CreateCommand();

            command.CommandText = "DELETE FROM Personage " +
                                  "WHERE name_personage = @namePersonage";

            // Явно добавляем параметр с указанием типа
            command.Parameters.Add(new NpgsqlParameter
            {
                ParameterName = "@namePersonage",
                NpgsqlDbType = NpgsqlDbType.Text,
                Value = namePersonage
            });

            await command.ExecuteNonQueryAsync(ctx);

            Console.WriteLine($"Personage with name '{namePersonage}' deleted successfully.");
        }
        catch (Exception ex)
        {
            // Записываем информацию об ошибке
            Console.WriteLine($"Error deleting personage: {ex}");
            throw;  // Перебрасываем исключение для его дальнейшей обработки
        }
        finally
        {
            await _dbConnection.CloseAsync();
        }
    }

    
    public async Task<Personage> GetCurrentPersonage(string personageName, CancellationToken ctx = default)
    {
        var currentPersonage = new Personage();
        Console.WriteLine(personageName, "dvf");
        await using(NpgsqlConnection connection = new NpgsqlConnection(DbConnectionString))
        {
            await connection.OpenAsync();

            using (NpgsqlCommand command = new NpgsqlCommand(
                             "SELECT id_personage, name_personage, isActive, image_path, Kanji, Romadzi, Nicknames, Race, gender, birthday, age, Height, Hair_color, Eye_color, Appearance_page, Personality_page, Capabilities_page, Weight, main_page " +
                             "FROM Personage where name_personage = @personageName", connection))
            {
                command.Parameters.AddWithValue("@personageName", personageName);

                 using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        currentPersonage.IdPersonage = reader.GetInt32("id_personage");
                        currentPersonage.NamePersonage = reader.GetString("name_personage");
                        Console.WriteLine(currentPersonage.NamePersonage, "persona");
                        currentPersonage.IsActive = reader.GetBoolean("isActive");
                        currentPersonage.ImagePath = reader.GetString("image_path");
                        currentPersonage.Kanji = reader.GetString("Kanji");
                        currentPersonage.Romadzi = reader.GetString("Romadzi");
                        currentPersonage.Nicknames = reader.GetString("Nicknames");
                        currentPersonage.Race = reader.GetString("Race");
                        currentPersonage.Gender = reader.GetString("gender");
                        currentPersonage.Birthday = reader.GetString("birthday");
                        currentPersonage.Age = reader.GetInt32("age");
                        currentPersonage.Height = reader.GetDecimal("Height");
                        currentPersonage.HairColor = reader.GetString("Hair_color");
                        currentPersonage.EyeColor = reader.GetString("Eye_color");
                        currentPersonage.AppearancePage = reader.GetString("Appearance_page");
                        currentPersonage.PersonalityPage = reader.GetString("Personality_page");
                        currentPersonage.CapabilitiesPage = reader.GetString("Capabilities_page");
                        currentPersonage.Weight = reader.GetString("Weight");
                        currentPersonage.MainPage = reader.GetString("main_page");
                    }
                }
            }
        }

        return currentPersonage;
    }


    public async Task<List<Personage>> GetPersonage(CancellationToken ctx = default)
    {
        var result = new List<Personage>();
        await using (NpgsqlConnection connection = new NpgsqlConnection(DbConnectionString))
        {
            await connection.OpenAsync();

            await using (NpgsqlCommand command = new NpgsqlCommand(
                             "SELECT p.id_personage, p.name_personage, p.isActive, p.Kanji, p.Romadzi, p.Nicknames, p.Race, p.gender, p.birthday, p.age, p.Height, p.Hair_color, p.Eye_color, p.Appearance_page, p.Personality_page, p.Capabilities_page, p.Weight, p.main_page, i.image_path " +
                             "FROM Personage p " +
                             "INNER JOIN Images i ON i.fk_id_personage = p.id_personage " +
                             "WHERE p.isActive = true",
                             connection))
            {
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Personage
                        {
                            IdPersonage = reader.GetInt32("id_personage"),
                            NamePersonage = reader.GetString("name_personage"),
                            IsActive = reader.GetBoolean("isActive"),
                            ImagePath = reader.GetString("image_path"),
                            
                        });
                    }
                }
            }
        }

        return result;
    }
}