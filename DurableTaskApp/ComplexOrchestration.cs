using DurableTask.Core;

namespace DurableTaskApp
{
     public class ComplexOrchestration : TaskOrchestration<int, int>
     {
         public override async Task<int> RunTask(OrchestrationContext context, int input)
         {
             // Initial activity
             int squaredNumber = await context.ScheduleTask<int>(typeof(SquareActivity), input);

             // Fan out
             Task<int> doubleTask = context.ScheduleTask<int>(typeof(DoubleActivity), squaredNumber);
             Task<int> tripleTask = context.ScheduleTask<int>(typeof(TripleActivity), squaredNumber);

             // Fan in
             await Task.WhenAll(doubleTask, tripleTask);
             int doubledNumber = doubleTask.Result;
             int tripledNumber = tripleTask.Result;
             int finalResult = await context.ScheduleTask<int>(typeof(AdditionActivity), (doubledNumber, tripledNumber));

             return finalResult;
         }
     }
}
