using System.Data;
using System.Data.SQLite;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string ConnectionString = "Data Source=students.db;Version=3;";

        using (var connection = new SQLiteConnection(ConnectionString))
        {
            connection.Open();

            string CreateTable = @"CREATE TABLE IF NOT EXISTS Students (
                                        StudentID INTEGER PRIMARY KEY AUTOINCREMENT,
                                        LastName TEXT,
                                        FirstName TEXT,
                                        Patronymic TEXT,
                                        GroupName TEXT,
                                        AverageGrade REAL,
                                        MinSubject TEXT,
                                        MaxSubject TEXT);";

            using (var command = new SQLiteCommand(CreateTable, connection))
            {
                command.ExecuteNonQuery();
            }
            Console.WriteLine("Table created successfully.");
        }
        using (var connection = new SQLiteConnection(ConnectionString))
        {
            try
            {
                await connection.OpenAsync();
                Console.WriteLine("Сonnection is successful\n");
                Console.WriteLine("ALL INFOMATIN:\n\n");
                string code = "SELECT * FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID | LastName | FirstName | Patronymic | GroupName | AverageGrade | MinSubject | MaxSubject");
                        Console.WriteLine("----------------------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]} | {reader["LastName"]} | {reader["FirstName"]} | {reader["Patronymic"]} | {reader["GroupName"]} | {reader["AverageGrade"]} | {reader["MinSubject"]} | {reader["MaxSubject"]}");
                        }
                    }
                }
                Console.WriteLine("\n\nLFP:");
                code = "SELECT StudentID, LastName, FirstNAme, Patronymic FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID | LastName | FirstName | Patronymic");
                        Console.WriteLine("--------------------------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]} | {reader["LastName"]} | {reader["FirstName"]} | {reader["Patronymic"]}");
                        }
                    }
                }

                Console.WriteLine("\n\nGRADE:");
                code = "SELECT StudentID, AverageGrade FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID | AverageGrade ");
                        Console.WriteLine("--------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]} \t  | {reader["AverageGrade"]}");
                        }
                    }
                }


                Console.WriteLine("\n\nGOOD_GRADE:");
                code = "SELECT StudentID, AverageGrade FROM Students WHERE AverageGrade > 80";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID | AverageGrade ");
                        Console.WriteLine("--------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]} \t  | {reader["AverageGrade"]}");
                        }
                    }
                }

                Console.WriteLine("\n\nMinSubject:");
                code = "SELECT DISTINCT StudentID, MinSubject FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("StudentID | MinSubject ");
                        Console.WriteLine("--------------------------");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["StudentID"]} \t  | {reader["MinSubject"]}");
                        }
                    }
                }

                Console.WriteLine("\n\nAnalysis resultі:");
                code = "SELECT MIN(AverageGrade) AS MinAverageGrade FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMinimum Average Grade: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                code = "SELECT MAX(AverageGrade) AS MinAverageGrade FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nMaximum Average Grade: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                code = @"SELECT COUNT(CASE WHEN MinSubject == 'Mathematics' THEN 1 ELSE NULL END) AS MathCount FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nCount of students with minimal grade in math: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                code = @"SELECT COUNT(CASE WHEN MaxSubject == 'Mathematics' THEN 1 ELSE NULL END) AS MathCount FROM Students";
                using (var command = new SQLiteCommand(code, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        Console.WriteLine($"\nCount of students with maximum grade in math: {result}");
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                    }
                }

                code = @"SELECT GroupName, COUNT(*) AS StudentsCount FROM Students GROUP BY GroupName";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\n\nGroupName | StudentsCount");
                        Console.WriteLine("------------------------");

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["GroupName"]} | {reader["StudentsCount"]}");
                        }
                    }
                }

                code = @"SELECT GroupName, AVG(AverageGrade) AS AverageGroupGrade FROM Students GROUP BY GroupName";
                using (var command = new SQLiteCommand(code, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("\n\nGroupName | AverageGroupGrade");
                        Console.WriteLine("---------------------------");

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["GroupName"]} | {reader["AverageGroupGrade"]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    await connection.CloseAsync();
                    Console.WriteLine("\nDisconnect is successful");
                }
            }
        }
    }
}