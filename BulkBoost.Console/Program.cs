using BulkBoost.Console.Constants;
using BulkBoost.Console.Helpers;
using BulkBoost.Service;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Spectre.Console;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        AnsiConsole.Write(new FigletText("Bulk Boost Demo").LeftJustified().Color(Color.Red));

        var totalNumberOfUsers = ConnectionStrings.Count();
        var numberOfUsersChoices = Enumerable.Range(1, totalNumberOfUsers).ToArray();

        var method = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select method:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more contacts)[/]")
                .AddChoices(["Create Multiple", "Execute Multiple"])
                .HighlightStyle(new Style(Color.Red)));

        var numberOfUsers = AnsiConsole.Prompt(
                   new SelectionPrompt<int>()
                       .Title("Number of users:")
                       .PageSize(10)
                       .MoreChoicesText("[grey](Move up and down to reveal more contacts)[/]")
                       .AddChoices(numberOfUsersChoices)
                       .HighlightStyle(new Style(Color.Red)));

        var numberOrContacts = AnsiConsole.Prompt(
                   new SelectionPrompt<int>()
                       .Title("Number of contacts:")
                       .PageSize(10)
                       .MoreChoicesText("[grey](Move up and down to reveal more contacts)[/]")
                       .AddChoices([5000, 10000, 20000, 30000, 100000])
                       .HighlightStyle(new Style(Color.Red)));

        var numberOfThreads = AnsiConsole.Prompt(new TextPrompt<int>("Number of threads: "));
        var batchSize = AnsiConsole.Prompt(new TextPrompt<int>("Batch size: "));

        var contacts = ContactGenerator.Generate(numberOrContacts);

        AnsiConsole.MarkupLine($"[white]Method: {method}[/]");
        AnsiConsole.MarkupLine($"[white]Number of users: {numberOfUsers}[/]");
        AnsiConsole.MarkupLine($"[white]Number of contacts: {contacts.Count}[/]");
        AnsiConsole.MarkupLine($"[white]Number of threads: {numberOfThreads}[/]");
        AnsiConsole.MarkupLine($"[white]Batch size: {batchSize}[/]");

        var durations = new List<int>();

        var connectionStrings = ConnectionStrings.Get(numberOfUsers);
        var multiplexingService = new MultiplexingServiceClient(connectionStrings);

        for (int i = 0; i < 1; i++)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            switch (method)
            {
                case "Create Multiple":
                    var contactEntities = contacts.Select(c => c.ToEntity()).ToList();

                    var cmr = new CreateMultipleRequest
                    {
                        Targets = new EntityCollection(contactEntities)
                        {
                            EntityName = "contact"
                        }
                    };

                    multiplexingService.Execute(cmr, numberOfThreads, batchSize);
                    break;
                case "Execute Multiple":
                    var requests = contacts.Select(c => new CreateRequest { Target = c.ToEntity() }).ToList();
                    var emr = new ExecuteMultipleRequest
                    {
                        Requests = [],
                        Settings = new ExecuteMultipleSettings
                        {
                            ContinueOnError = true,
                            ReturnResponses = false
                        }
                    };

                    emr.Requests.AddRange(requests);

                    multiplexingService.Execute(emr, numberOfThreads, batchSize);

                    break;
            }

            stopwatch.Stop();
            var seconds = Convert.ToInt32(stopwatch.ElapsedMilliseconds / 1000);
            durations.Add(seconds);
        }

        // median
        var median = durations.OrderBy(d => d).ElementAt(durations.Count / 2);
        AnsiConsole.MarkupLine($"[white]Median duration: {median} seconds[/]");
        // average
        var average = durations.Average();
        AnsiConsole.MarkupLine($"[white]Average duration: {average} seconds[/]");

        AnsiConsole.MarkupLine("[green]Done![/]");
    }

}