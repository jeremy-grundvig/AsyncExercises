using System;
using System.Threading.Tasks;

namespace AsyncExercises_Core
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"{nameof(AsyncExercises_Core)}");
            Console.WriteLine($"{nameof(AsyncTests)}.RunTests()");
            var tests = new AsyncTests();
            await tests.RunTests();
            Console.ReadLine();
        }
    }
}
