using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FirstSemestrWork.Configuration;

namespace FirstSemestrWork
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var server = new HttpServer())
                {
                    await server.StartAsync();
                    Console.WriteLine("sdsd");
                    server.ProcessStop();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Сервер завершил свою работу");
            }
        }
    }
}