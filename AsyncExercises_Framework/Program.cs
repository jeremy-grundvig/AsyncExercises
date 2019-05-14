using System;

namespace AsyncExercises_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(AsyncExercises_Framework)}");
            Console.WriteLine($"{nameof(AsyncTests)}.RunTests()");
            var tests1 = new AsyncTests();
            tests1.RunTests().Wait();
            //Console.WriteLine($"{nameof(TestMany)}.RunTests()");
            //var tests2 = new TestMany();
            //tests2.RunTests().Wait();
            Console.WriteLine("All tests complete");
            Console.ReadLine();
        }
    }
}
