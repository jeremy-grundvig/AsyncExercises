using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncExercises_Framework
{
    public class TestMany
    {
        public async Task RunTests()
        {
            var results = await GetResults();
            Console.WriteLine("Got results");
            // just adding a .ToList here will still re-evaluate (already evaluated with the Task.WhenAll) the underlying methods,
            // it needs to be a list prior to this to avoid real duplication
            //results = results.ToList();
            int[] needMatches = { 9, 1, 2, 3, 4, 5, 6 };
            foreach (var needMatch in needMatches)
            {
                // each iteration will reinvoke the underlying method since it is deferred
                var match = results.FirstOrDefault(r => r == needMatch);
                Console.WriteLine(match);
            }
        }

        private async Task<IEnumerable<int>> GetResults()
        {
            Console.WriteLine("Starting Tasks");
            var tasks = GetAsyncTasks(10);
            // Task.WhenAll turns it into a list/array internally
            // https://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/Task.cs,6232
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Finished Waiting");
            // because this is acting on the actual object from Task.WhenAll it should still be a list/array
            return result.SelectMany(t => t);

            // return the result of the Task.WhenAll keeps the object in a list
            //return await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private IEnumerable<Task<IEnumerable<int>>> GetAsyncTasks(int howMany)
        {
            return Enumerable.Range(1, howMany).Select(i => GetTheInts(i));//.ToList();
        }

        private async Task<IEnumerable<int>> GetTheInts(int i)
        {
            Console.WriteLine($"{nameof(GetTheInts)}:{i}");
            await Task.Delay(100);
            return Enumerable.Range(0,10);
        }
    }
}
