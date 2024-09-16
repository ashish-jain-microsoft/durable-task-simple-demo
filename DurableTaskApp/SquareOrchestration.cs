using DurableTask.Core;

namespace DurableTaskApp
{
    internal class SquareOrchestration : TaskOrchestration<int, int>
    {
        public async override Task<int> RunTask(OrchestrationContext context, int input)
        {
            var result = await context.ScheduleTask<int>(typeof(SquareActivity), input);
            return result;
        }
    }
}
