using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using BlockChain.Models;

namespace BlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            const int blocksToMine = 3;
            Console.WriteLine($"Total blocks to mine: {blocksToMine}");
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var blockChain = new Blockchain();
            AddBlocksToChain(blocksToMine, blockChain);
            stopWatch.Stop();
            
            Console.WriteLine($"Total elapsed time: {stopWatch.Elapsed}");

            JsonSerializer.Serialize(blockChain.Chain);
        }

        private static void AddBlocksToChain(int blocksToMine, Blockchain blockChain)
        {
            var random = new Random();
            var payload = new List<object>(100);
            
            for (int i = 0; i < blocksToMine; i++)
            {
                for (var y = 0; y < payload.Capacity; y++)
                {
                    var data = new { From = GenerateFromRandomName(), To = GenerateToRandomName(), Amount = random.NextDouble() };
                    payload.Add(data);
                }

                blockChain.AddBlock(new Block(DateTime.UtcNow, blockChain.GetLastBlock().Hash, payload.ToArray()));
                payload.Clear();
            }
        }

        private static string GenerateFromRandomName()
        {
            var eligibleNames = new List<string>() { "Marcello", "Gustavo", "Maria", "Jessica", "Thais" };
            var randomIndex = new Random().Next(0, eligibleNames.Count - 1);
            return eligibleNames[randomIndex];
        }
        
        private static string GenerateToRandomName()
        {
            var eligibleNames = new List<string>() { "Ronaldo", "Kaka", "Ze Maria", "Felipe", "Adriano" };
            var randomIndex = new Random().Next(0, eligibleNames.Count - 1);
            return eligibleNames[randomIndex];
        }
    }
}