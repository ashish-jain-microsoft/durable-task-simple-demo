using DurableTask.Core;

namespace DurableTaskApp
{
    internal class TripleActivity : TaskActivity<int, int>
    {
         protected override int Execute(TaskContext context, int input)
         {
             return input * 3;
         }
    }
}
