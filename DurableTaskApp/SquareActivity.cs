using DurableTask.Core;

namespace DurableTaskApp
{
    internal class SquareActivity : TaskActivity<int, int>
    {
        protected override int Execute(TaskContext context, int input)
        {
            return input * input;
        }
    }
}
