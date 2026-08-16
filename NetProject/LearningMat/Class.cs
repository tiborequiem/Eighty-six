namespace NetProject.LearningMat
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== 1. SYNCHRONOUS RUN ===");
            Stopwatch sw1 = Stopwatch.StartNew();
            FetchUserDataSync();
            FetchUserOrdersSync();
            sw1.Stop();
            Console.WriteLine($"Sync Total Time: {sw1.ElapsedMilliseconds} ms\n");

            Console.WriteLine("=== 2. ASYNCHRONOUS RUN ===");
            Stopwatch sw2 = Stopwatch.StartNew();

            // Start both tasks concurrently
            Task userTask = FetchUserDataAsync();
            Task ordersTask = FetchUserOrdersAsync();

            // Wait for both tasks to complete non-blockingly
            await Task.WhenAll(userTask, ordersTask);

            sw2.Stop();
            Console.WriteLine($"Async Total Time: {sw2.ElapsedMilliseconds} ms\n");
        }

        // --- SYNCHRONOUS METHODS (Blocks execution sequentially) ---
        static void FetchUserDataSync()
        {
            Console.WriteLine("Starting sync user fetch...");
            Thread.Sleep(2000); // Blocks the current thread for 2 seconds
            Console.WriteLine("Finished sync user fetch.");
        }

        static void FetchUserOrdersSync()
        {
            Console.WriteLine("Starting sync orders fetch...");
            Thread.Sleep(2000); // Blocks the current thread for 2 seconds
            Console.WriteLine("Finished sync orders fetch.");
        }

        // --- ASYNCHRONOUS METHODS (Frees the thread during wait) ---
        static async Task FetchUserDataAsync()
        {
            Console.WriteLine("Starting async user fetch...");
            await Task.Delay(2000); // Non-blocking wait for 2 seconds
            Console.WriteLine("Finished async user fetch.");
        }

        static async Task FetchUserOrdersAsync()
        {
            Console.WriteLine("Starting async orders fetch...");
            await Task.Delay(2000); // Non-blocking wait for 2 seconds
            Console.WriteLine("Finished async orders fetch.");
        }
    }
}
