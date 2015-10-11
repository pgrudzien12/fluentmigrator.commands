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
        private readonly IApplicationEnvironment _appEnv;

        public Program(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public IConfiguration Configuration { get; set; }

        public void Main(string[] args)
        {
            BuildConfiguration(args);
            Console.WriteLine(new MigrationProcessorFactoryProvider().ListAvailableProcessorTypes());

            Console.WriteLine(Configuration["command-text"]);
            var runnerContext = new RunnerContext(new ConsoleAnnouncer())
            {
                Database = "SqlServer",
                Connection = Configuration["Data:DefaultConnection:ConnectionString"],
                Targets = new string[] { Configuration["command-text"] },
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
    }


}
