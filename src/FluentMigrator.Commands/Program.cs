using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Runtime;

namespace FluentMigrator.Commands
{
    public class Program
    {
        public IConfiguration Configuration { get; set; }

        public void Main(string[] args)
        {
            BuildConfiguration(args);
            PrintLogo();

            var csName = Configuration["cs"];
            var runnerContext = new RunnerContext(new ConsoleAnnouncer())
            {
                Database = "SqlServer",
                Connection = Configuration[$"Data:{csName}:ConnectionString"],
                Targets = new string[] { Configuration["target"] },
                PreviewOnly = false,
                NestedNamespaces = true,
                Task = "migrate",
                StartVersion = 0,
                NoConnection = false,
                Steps = 1,
                WorkingDirectory = ".",
                TransactionPerSession = true
            };

            new TaskExecutor(runnerContext).Execute();

            Console.ReadLine();
        }

        private void BuildConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddCommandLine(args);

            Configuration = builder.Build();
        }

        private void PrintLogo()
        {
            Console.WriteLine(@"
   ___ _                  _          _                 _             
  / __\ |_   _  ___ _ __ | |_  /\/\ (_) __ _ _ __ __ _| |_ ___  _ __ 
 / _\ | | | | |/ _ \ '_ \| __|/    \| |/ _` | '__/ _` | __/ _ \| '__|
/ /   | | |_| |  __/ | | | |_/ /\/\ \ | (_| | | | (_| | || (_) | |   
\/    |_|\__,_|\___|_| |_|\__\/    \/_|\__, |_|  \__,_|\__\___/|_|   
                                       |___/                         
   ___                                          _                    
  / __\___  _ __ ___  _ __ ___   __ _ _ __   __| |___                
 / /  / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` / __|               
/ /__| (_) | | | | | | | | | | | (_| | | | | (_| \__ \               
\____/\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|___/               
                                                                      
");
        }
    }
}
