using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FirstSemestrWork.Configuration;
namespace FirstSemestrWork;
using FirstSemestrWork.Model;
public class HttpServer: IDisposable
{
    
    private static AppSettings _config = ConfigManager.GetConfig();

    private HttpListener _server = new HttpListener();
    private CancellationTokenSource _tokenSource = new();

    public HttpServer()
    {
        _server.Prefixes.Add($"http://{_config.Address}:{_config.Port}/");
        Console.WriteLine($"http://{_config.Address}:{_config.Port}/");
    }

    public async Task StartAsync()
    {
        _server.Start();
        Console.WriteLine("Сервер успешно запущен");
        var token = _tokenSource.Token;
        Task.Run(async () => { await Lisenning(token); });
        Console.WriteLine("Запрос обработан");
    }

    private async Task Lisenning(CancellationToken token)
    {
        try
        {
            while (_server.IsListening)
            {
                
                token.ThrowIfCancellationRequested();
                
                var context =  await _server.GetContextAsync();
                var response = context.Response;
                var request = context.Request;
                var ctx = new CancellationTokenSource();
                
                _ = Task.Run(async () =>
                {
                    switch (request.Url?.LocalPath)
                    {
                        case "/admin":
                            await WebHelper.ShowAdminPanel(context, ctx.Token);
                            break;
                        case "/addPersonage":
                            await WebHelper.AddNewPersonage(context, ctx.Token);
                            break;
                        case "/deletePersonage":
                            await WebHelper.DeletePersonage(context, ctx.Token);
                            break;
                        case "/home":
                            await WebHelper.ShowHome(context, ctx.Token);
                            break;
                        case "/personageInformation":
                            await WebHelper.ShowPersonageInformation(context, ctx.Token);
                            break;
                        case "/login":
                            await WebHelper.ShowLogin(context, ctx.Token);
                            break;
                        case "/signin":
                            await WebHelper.LoginUserAsync(context, ctx.Token);
                            break;
                        case "/getPersonages":
                            await WebHelper.GetPersonage(context, ctx.Token);
                            break; 
                        case "/getCurrentPersonage":
                            WebHelper.GetCurrentPersonage(context, ctx.Token);
                            break;
                        case "/setCurrentPersonage":
                            WebHelper.SetCurrentPersonage(context, ctx.Token);
                            break;
                        default:
                            await WebHelper.ShowStatic(context, ctx.Token);
                            break;
                        }
                    response.OutputStream.Close();
                    response.Close();
                });
            }
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            _server.Close();
            _tokenSource.Cancel();
            ((IDisposable)_server).Dispose();
            Console.WriteLine("Server has been stopped.");   
            ProcessStop();
        }
    }

    public void ProcessStop()
    {
        while (true)
        {
            Console.WriteLine("Для завершения работы сервера напишите \"stop\" ");
            string key = Console.ReadLine();
            if (key == "stop")
            {
                _tokenSource.Cancel();
                Console.WriteLine("Сервер завершил работу");
                break;
            }
            continue;
        }
    }

    public void Dispose()
    {
        
    }
}

