using DurableTask.Core;

namespace DurableTaskApp
{
    internal class AdditionActivity : TaskActivity<(int, int), int>
    {
         protected override int Execute(TaskContext context, (int, int) input)
         {
             return input.Item1 + input.Item2;
         }
    }
}
