using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncExercises_Framework
{
    public class AsyncTests
    {
        /// <summary>
        /// Test Harness
        /// </summary>
        /// <returns></returns>
        public async Task RunTests()
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 1: Original Pattern that initiated this research");
            Console.WriteLine("Results are re-enumerated for each iteration: something like n+n!");
            Console.WriteLine(new string('*', 50));
            var results1 = await GetResultsOriginal().ConfigureAwait(false);
            MatchResult(results1);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 1: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 1a: Original Pattern using .tolist - this fixes issue, but requires changes in multiple places");
            Console.WriteLine("Results are only enumerated once, but missing changes on either side of the pattern could result in multiples");
            Console.WriteLine(new string('*', 50));
            var results1a = await GetResultsOriginalToList().ConfigureAwait(false);
            // just adding a .ToList here will still re-evaluate (already evaluated with the Task.WhenAll) the underlying methods,
            // it needs to be a list prior to this to avoid real duplication
            results1a = results1a.ToList();
            MatchResult(results1a);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 1a: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 2: Another pattern that we commonly use - Task.WhenAll, add results to another list either in loop or via a linq select");
            Console.WriteLine("The results are re-enumerated as they are projected into the List");
            Console.WriteLine(new string('*', 50));
            var results2 = await GetResultsAddResultsToList().ConfigureAwait(false);
            MatchResult(results2);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 2: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 3: Using the results of Task.WhenAll directly");
            Console.WriteLine("The results are only enumerated once");
            Console.WriteLine(new string('*', 50));
            var results3 = await GetResultsWhenAllAsync().ConfigureAwait(false);
            MatchResult(results3);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 3: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 4: Another pattern we use involving a Dictionary of Tasks with an id");
            Console.WriteLine("The results are only enumerated once because the tasks are stored in a Dictionary");
            Console.WriteLine(new string('*', 50));
            await GetDictionaryResultsOriginal().ConfigureAwait(false);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 4: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 5: Another pattern we use");
            Console.WriteLine(new string('*', 50));
            await GetOfflineImagesAsync().ConfigureAwait(false);
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 5: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();


            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 6: Another pattern we use - get an ienumerable and perform LINQ operations on it");
            Console.WriteLine(new string('*', 50));
            var results6 = await GetResultsOriginal().ConfigureAwait(false);
            var total = results6.Sum(x => x);
            var first = results6.FirstOrDefault();
            var last = results6.OrderByDescending(x => x).FirstOrDefault();
            Console.WriteLine($"Calculation work: {total} - {first} - {last}");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Round 6: Complete");
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        /// <summary>
        /// Mimics the business logic of iterating through results of the db aggregation
        /// </summary>
        /// <param name="results"></param>
        public void MatchResult(IEnumerable<int> results)
        {
            // Mimics usage of the results
            int[] needMatches = { 9, 1, 2, 3, 4, 5, 6 };
            foreach (var needMatch in needMatches)
            {
                // each iteration will reinvoke the underlying method if it has not already been resolved (e.g. via a tolist)
                var match = results.FirstOrDefault(r => r == needMatch);
                Console.WriteLine($"Matching value: {match}");
            }
            Console.WriteLine($"{nameof(MatchResult)}: Finished");
        }

        /// <summary>
        /// Mimics the Service/Business layer logic that calls the aggregation call in the data layer
        /// This version mimics the original buggy code
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetResultsOriginal()
        {
            Console.WriteLine("Starting Tasks");
            var tasks = GetAsyncTasks(10);
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Finished Waiting");
            // this turns "result" back into an enumerable - boo
            return tasks.Select(t => t.Result);
        }

        private async Task GetDictionaryResultsOriginal()
        {
            // setup the dictionary and its tasks
            int max = 10;
            var range = Enumerable.Range(1, max);
            var dictionaryTasks = GetAsyncTasksDictionary(max);
            await Task.WhenAll(dictionaryTasks.Select(t => t.Value));

            // loop over dictionary to match to starting point
            foreach (var item in range)
            {
                int i = dictionaryTasks[item].Result;
            }
        }

        /// <summary>
        /// Mimics the Service/Business layer logic that calls the aggregation call in the data layer
        /// This version mimics the original buggy code, but calls the List aggregate method
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetResultsOriginalToList()
        {
            Console.WriteLine("Starting Tasks");
            var tasks = GetAsyncTasksList(10);
            var result = await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine("Finished Waiting");
            // this turns "result" back into an enumerable - boo
            return tasks.Select(t => t.Result);
        }

        /// <summary>
        /// Mimics the Service/Business layer logic that calls the aggregation call in the data layer
        /// This version mimics a pattern we often use in API1
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetResultsAddResultsToList()
        {
            Console.WriteLine($"{nameof(GetResultsAddResultsToList)}:Starting Tasks");
            var tasks = GetAsyncTasks(10);
            await Task.WhenAll(tasks).ConfigureAwait(false);
            Console.WriteLine($"{nameof(GetResultsAddResultsToList)}:Finished Waiting");

            // We use this pattern many places
            List<int> otherTasks = new List<int>();
            otherTasks.AddRange(tasks.Select(t => t.Result));
            return otherTasks;
        }

        /// <summary>
        /// Mimics the Service/Business layer logic that calls the aggregation call in the data layer
        /// This version uses the results of the call to Task.WhenAll directly
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetResultsWhenAllAsync()
        {
            Console.WriteLine($"{nameof(GetResultsWhenAllAsync)}:Starting Tasks - will be awaited one level up");
            var tasks = GetAsyncTasks(10);
            // Task.WhenAll turns it into a list/array internally
            // https://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/Task.cs,6232
            return await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task<IEnumerable<int>> GetResultsDictionaryWhenAllAsync()
        {
            Console.WriteLine($"{nameof(GetResultsDictionaryWhenAllAsync)}:Starting Tasks - will be awaited one level up");
            var tasks = GetAsyncTasksDictionary(10);
            // Task.WhenAll turns it into a list/array internally
            // https://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/Task.cs,6232
            return await Task.WhenAll(tasks.Select(t => t.Value)).ConfigureAwait(false);
        }

        /// <summary>
        /// Mimics a data layer operation that is making DB calls for multiple objects and aggregating them into a collection
        /// </summary>
        /// <param name="howMany"></param>
        /// <returns></returns>
        private IEnumerable<Task<int>> GetAsyncTasks(int howMany)
        {
            return Enumerable.Range(1, howMany).Select(i => MyFakeDatabaseCallAsync(i));
        }

        /// <summary>
        /// Mimics a data layer operation that is making DB calls for multiple objects and aggregating them into a collection, in this case a list
        /// </summary>
        /// <param name="howMany"></param>
        /// <returns></returns>
        private IEnumerable<Task<int>> GetAsyncTasksList(int howMany)
        {
            return Enumerable.Range(1, howMany).Select(i => MyFakeDatabaseCallAsync(i)).ToList();
        }

        private Dictionary<int, Task<int>> GetAsyncTasksDictionary(int howMany)
        {
            Dictionary<int, Task<int>> results = new Dictionary<int, Task<int>>();
            var range = Enumerable.Range(1, howMany);
            foreach (var item in range)
            {
                results.Add(item, MyFakeDatabaseCallAsync(item));
            }
            return results;
        }

        /// <summary>
        /// Mimics an asynchronous call to a database. Task.Delay is used to mimic processing
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private async Task<int> MyFakeDatabaseCallAsync(int i)
        {
            Console.WriteLine($"{nameof(MyFakeDatabaseCallAsync)}:{i}");
            await Task.Delay(10);
            return i;
        }

        public async Task GetOfflineImagesAsync()
        {
            List<Task> contactTask = new List<Task>();
            List<Task<int>> imageTasks = new List<Task<int>>();
            Dictionary<int, HashSet<int>> imageIdToUserDeviceIdLookup = new Dictionary<int, HashSet<int>>();
            var range = Enumerable.Range(1, 10);
            foreach (var r in range)
            {
                imageTasks.Add(MyFakeDatabaseCallAsync(r));
            }

            await Task.WhenAll(imageTasks).ConfigureAwait(false);
            var offlineImages = imageTasks.Where(x => x.IsCompleted)?.Select(x => x.Result) ?? new List<int>();
            var offlineImageExceptions = imageTasks.Where(x => x.IsFaulted)?.Select(x => x.Result) ?? new List<int>();

            if (offlineImageExceptions.Any())
            {
                Console.WriteLine(string.Join(",", offlineImageExceptions.Select(x => x)));
            }

            foreach (var image in offlineImages)
            {
                int i = image;
            }

            foreach (var r in offlineImageExceptions)
            {
                Console.WriteLine($"ImageId [{r}] was not retrieved.");
            }
        }
    }
}
