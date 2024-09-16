using DurableTask.AzureStorage;
using DurableTask.Core;

namespace DurableTaskApp
{
    internal class Program
    {
        static async Task Main()
        {
            // Setup
            var storageAccountConnectionString = "<Enter your SA connection string>";
            var taskHubName = "DurableTaskDemoHub"; // Namespace for all my orchestrations while persisting to Azure Storage

            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageAccountClientProvider = new StorageAccountClientProvider(storageAccountConnectionString),
                TaskHubName = taskHubName,
            };

            var orchestrationService = new AzureStorageOrchestrationService(settings);
            var client = new TaskHubClient(orchestrationService);
            var worker = new TaskHubWorker(orchestrationService);

            await orchestrationService.CreateIfNotExistsAsync();

            Console.WriteLine("Starting worker...");

            // Registering orchestrations and activities
            worker.AddTaskOrchestrations(typeof(SquareOrchestration))
                  .AddTaskOrchestrations(typeof(ComplexOrchestration));

            worker.AddTaskActivities(typeof(SquareActivity))
                  .AddTaskActivities(typeof(TripleActivity))
                  .AddTaskActivities(typeof(DoubleActivity))
                  .AddTaskActivities(typeof(AdditionActivity));

            await worker.StartAsync();

            // Input
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Enter a number for the orchestrations:");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int validNumber))
            {
                throw new Exception("Invalid input. Please enter a valid number.");
            }

            // Square orchestration
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Starting Square orchestration...");

            var squareOrchestrationInstance = await client.CreateOrchestrationInstanceAsync(typeof(SquareOrchestration), validNumber);
            var squareOrchestrationInstanceStatus = await client.WaitForOrchestrationAsync(squareOrchestrationInstance, TimeSpan.FromSeconds(10));

            if (squareOrchestrationInstanceStatus.OrchestrationStatus == OrchestrationStatus.Completed)
            {
                Console.WriteLine($"Orchestration completed with result: {squareOrchestrationInstanceStatus.Output}");
            }
            else
            {
                Console.WriteLine("Orchestration failed or timed out. Orchestration status: {0}", squareOrchestrationInstanceStatus.OrchestrationStatus);
            }

            // Complex orchestration
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Starting complex orchestration...");

            var complexOrchestrationInstance = await client.CreateOrchestrationInstanceAsync(typeof(ComplexOrchestration), validNumber);
            var complexOrchestrationInstanceStatus = await client.WaitForOrchestrationAsync(complexOrchestrationInstance, TimeSpan.FromSeconds(10));

            if (complexOrchestrationInstanceStatus.OrchestrationStatus == OrchestrationStatus.Completed)
            {
                Console.WriteLine($"Orchestration completed with result: {complexOrchestrationInstanceStatus.Output}");
            }
            else
            {
                Console.WriteLine("Orchestration failed or timed out. Orchestration status: {0}", complexOrchestrationInstanceStatus.OrchestrationStatus);
            }

            // Cleanup
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Stopping worker...");
            await worker.StopAsync();
        }
    }
}
