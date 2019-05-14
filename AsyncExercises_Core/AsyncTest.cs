using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncExercises_Core
{
    public class AsyncTests
    {
        public async Task RunTests()
        {
            var results = await GetResults();
            results = results.ToList();
            int[] needMatches = { 9, 1, 2, 3, 4, 5, 6 };
            foreach (var needMatch in needMatches)
            {
                var match = results.FirstOrDefault(r => r == needMatch);
                Console.WriteLine(match);
            }
        }

        private async Task<IEnumerable<int>> GetResults()
        {
            Console.WriteLine("Starting Tasks");
            var tasks = GetAsyncTasks(10);
            return await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Finished Waiting");
            return tasks.Select(t => t.Result);
        }

        private IEnumerable<Task<int>> GetAsyncTasks(int howMany)
        {
            return Enumerable.Range(1, howMany).Select(i => GetTheInt(i)).ToList();
        }

        private async Task<int> GetTheInt(int i)
        {
            Console.WriteLine($"{nameof(GetTheInt)}:{i}");
            await Task.Delay(100);
            return i;
        }
    }
}
