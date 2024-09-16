using DurableTask.Core;

namespace DurableTaskApp
{
    internal class DoubleActivity : TaskActivity<int, int>
    {
         protected override int Execute(TaskContext context, int input)
         {
             return input * 2;
         }
    }
}
