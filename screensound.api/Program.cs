using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using screensound.core.data;
using screensound.core.data.dal;
using screensound.core.models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace screensound.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<JsonOptions>(ConfigureJson);

            WebApplication app = builder.Build();

            app.MapGet("/", GetArtists);

            app.Run();
        }

        private static void ConfigureJson(JsonOptions options)
        {
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }

        private static List<Artist> GetArtists()
        {
            ScreenSoundContext context = new();
            DAL<Artist> dal = new(context);
            return dal.GetList();
        }
    }
}
