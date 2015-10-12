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

        public Program(IApplicationEnvironment appEnv, IRuntimeEnvironment env, IRuntimeOptions opt)
        {
            _appEnv = appEnv;
        }

        public IConfiguration Configuration { get; set; }

        public void Main(string[] args)
        {
            BuildConfiguration(args);

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
    }
}
