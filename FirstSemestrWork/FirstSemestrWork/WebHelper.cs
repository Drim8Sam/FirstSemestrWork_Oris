using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FirstSemestrWork.Configuration;
using FirstSemestrWork.Model;


namespace FirstSemestrWork;
public static class WebHelper
{
    private static readonly DbContext _dbContext = new DbContext();
    private static Personage currentPersonage;
    private static async Task ShowFile(string path, HttpListenerContext context, CancellationToken ctx)
    {
        context.Response.StatusCode = 200;
        context.Response.ContentType = Path.GetExtension(path) switch
        {
            ".js" => "application/javascript",
            ".css" => "text/css",
            ".html" => "text/html",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            _ => "text/plain"
            
        };
        var file = await File.ReadAllBytesAsync(path, ctx);
        await context.Response.OutputStream.WriteAsync(file, ctx);
    }
    
    public static async Task ShowStatic(HttpListenerContext context, CancellationToken ctx)
    {
        var path = context.Request.Url?.LocalPath
            .Split("/")
            .Skip(1)
            .ToArray();
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "static");

        if (path != null)
        {
            for (var i = 0; i < path.Length - 1; i++)
            {
                basePath = Path.Combine(basePath, $@"{path[i]}");
            }
        }

        basePath = Path.Combine(basePath, path?[^1] ?? string.Empty);

        if (File.Exists(basePath))
        {
            await ShowFile(basePath, context, ctx);
        }
        else
        {
            await Show404(context, ctx);
        }
    }
    
    private static async Task Show404(HttpListenerContext context, CancellationToken ctx)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.StatusCode = 404;
        await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Нужная вам страница не найдена!"), ctx);
        await ShowFile(@"/Users/err/RiderProjects/Main/FirstSemestrWork/FirstSemestrWork/bin/Debug/net7.0/static/page404.html", context, ctx);
    }
    
    private static async Task Show500(HttpListenerContext context, Exception ex, CancellationToken ctx)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.StatusCode = 500;
        await context.Response.OutputStream.WriteAsync(
            Encoding.UTF8.GetBytes(
                $"Произошла ошибка на стороне сервера. Информация по ошибке: {ex.Message}"), ctx);
    }
    
    public static async Task ShowHome(HttpListenerContext context, CancellationToken ctx)
    { 
        await ShowFile(@"/Users/err/RiderProjects/Main/FirstSemestrWork/FirstSemestrWork/bin/Debug/net7.0/static/html/index.html", context, ctx);
    }
    public static async Task ShowAdminPanel(HttpListenerContext context, CancellationToken ctx)
    { 
        await ShowFile(@"/Users/err/RiderProjects/Main/FirstSemestrWork/FirstSemestrWork/bin/Debug/net7.0/static/html/adminPanel.html", context, ctx);
    }

    public static async Task AddNewPersonage(HttpListenerContext context, CancellationToken ctx)
    {
        try
        {
            using var sr = new StreamReader(context.Request.InputStream);
            var userInformationModel = JsonSerializer.Deserialize<UserInformationModel>(await sr.ReadToEndAsync().ConfigureAwait(false));

            var dbContext = new DbContext();
            await dbContext.AddPersonage(userInformationModel.NickName, userInformationModel.ImagePath, ctx);
            Console.WriteLine("Personage added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding personage: {ex}");
            await Show500(context, ex, ctx);
        }
    }



    public static async Task DeletePersonage(HttpListenerContext context, CancellationToken ctx)
    {
        try
        {
            using var sr = new StreamReader(context.Request.InputStream);
            var UserInformationModel =  JsonSerializer.Deserialize<UserInformationModel>($"{await sr.ReadToEndAsync().ConfigureAwait(false)}");
            var dbContext = new DbContext();
            await dbContext.DeletePersonage(UserInformationModel.NickName, ctx);
        }
        catch (Exception ex)
        {
            await Show500(context, ex, ctx);
        }
    }
    // Добавьте соответствующий метод в WebHelper
    public static void SetCurrentPersonage(HttpListenerContext context, CancellationToken ctx)
    {
        try
        {
            using var sr = new StreamReader(context.Request.InputStream);
            var jsonString = sr.ReadToEnd();
            var userInformationModel = JsonSerializer.Deserialize<UserInformationModel>(jsonString);

            Console.WriteLine(userInformationModel.NickName);

            currentPersonage = _dbContext.GetCurrentPersonage(userInformationModel.NickName, ctx).Result;
        }
        catch (Exception ex)
        {
            Show500(context, ex, ctx).Wait();
        }
    }

    public static void GetCurrentPersonage(HttpListenerContext context, CancellationToken ctx)
    {
        try
        {
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(currentPersonage);
            Console.WriteLine(currentPersonage);

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            context.Response.OutputStream.Write(jsonBytes, 0, jsonBytes.Length);
            Console.WriteLine('o');
        }
        catch (Exception ex)
        {
            Show500(context, ex, ctx).Wait();
        }
    }


    public static async Task ShowLogin(HttpListenerContext context, CancellationToken ctx)
    { 
        await ShowFile(@"/Users/err/RiderProjects/Main/FirstSemestrWork/FirstSemestrWork/bin/Debug/net7.0/static/html/login.html", context, ctx);
    }
    public static async Task ShowPersonageInformation(HttpListenerContext context, CancellationToken ctx)
    { 
        await ShowFile(@"/Users/err/RiderProjects/Main/FirstSemestrWork/FirstSemestrWork/bin/Debug/net7.0/static/html/SatoryGojo.html", context, ctx);
    }
    
    public static async Task LoginUserAsync(HttpListenerContext context, CancellationToken ctxToken)
    {
        using var sr = new StreamReader(context.Request.InputStream);
        var userLoginModel = JsonSerializer.Deserialize<UserLoginModel>(
            await sr.ReadToEndAsync().ConfigureAwait(false),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        try
        {
            if (!(userLoginModel.Password == "123" && userLoginModel.NickName == "Admin"))
            {
                context.Response.ContentType = "text/plain; charset=utf-8";
                context.Response.StatusCode = 400;
                await context.Response.OutputStream.WriteAsync(
                    Encoding.UTF8.GetBytes($"Неверно введен логин или пароль"));
                return;
            }
        }
        catch (Exception ex)
        {
            await Show500(context, ex, ctxToken);
        }
        
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.StatusCode = 200;
        await context.Response.OutputStream.WriteAsync(
            Encoding.UTF8.GetBytes("Поздравляем!"), ctxToken);
    }

    public static async Task GetPersonage(HttpListenerContext context, CancellationToken ctx)
    {
        try
        {
            var dbContext = new DbContext();
            var personages = await dbContext.GetPersonage(ctx);
            Console.WriteLine(personages);
            
            var json = JsonSerializer.Serialize(personages);
            Console.WriteLine(json);
            
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = 200;
            await context.Response.OutputStream.WriteAsync(
                Encoding.UTF8.GetBytes(json), ctx);
        }
        catch (Exception ex)
        {
            await Show500(context, ex, ctx);
        }
    }
}