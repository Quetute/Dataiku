using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Dataiku.Components;
using Dataiku.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Dataiku
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Mission mission = GetMission();
            List<Route> routes = GetRoutes(mission);

            services.AddSingleton(new Computer(mission.Autonomy, mission.Departure, mission.Arrival, routes));
            services.AddMvc().AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Odds/Index", ""));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc( routes =>{
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Odds}/{action=Index}/{id?}"
                );
            });
        }

        private Mission GetMission()
        {
            Mission mission;
            using (StreamReader sr = new StreamReader(Configuration["MissionFileName"]))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                mission = new JsonSerializer().Deserialize<Mission>(reader);
            }

            return mission;
        }

        private static List<Route> GetRoutes(Mission missionConfig)
        {
            // We assume that routes cannot change between calls.
            // We just read the database at startup time. 
            var routes = new List<Route>();

            using (SQLiteConnection sQLiteConnection = new SQLiteConnection(GetSqliteConnectionString(missionConfig)))
            using (SQLiteCommand sQLiteCommand = new SQLiteCommand("Select * from ROUTES", sQLiteConnection))
            {
                sQLiteConnection.Open();
                var reader = sQLiteCommand.ExecuteReader();

                while (reader.Read())
                {
                    routes.Add(new Route
                    {
                        Origin = reader["Origin"].ToString(),
                        Destination = reader["Destination"].ToString(),
                        TravelTime = Int32.Parse(reader["Travel_Time"].ToString())
                    });
                }
                sQLiteConnection.Close();
            }

            return routes;
        }

        private static string GetSqliteConnectionString(Mission missionConfig) 
            => $"Data Source={missionConfig.RoutesDb}";
    }
}
