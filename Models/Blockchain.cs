using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockChain.Models
{
    public class Blockchain
    {
        public List<Block> Chain { get; private set; }
        public int Difficulty { get; private set; }
        public int BlockTimeInMilliseconds { get; } = 30000;

        public Blockchain()
        {
            // Add genesis block
            Chain = new List<Block>{new(DateTime.UtcNow)};
            Difficulty = 1;
        }

        public Block GetLastBlock() => Chain.Last();

        public void AddBlock(Block block)
        {
            block.Mine(Difficulty);
            Chain.Add(block);
            Difficulty += (DateTime.UtcNow - GetLastBlock().Timestamp).TotalMilliseconds < BlockTimeInMilliseconds ? 1 : -1;
            Console.WriteLine($"Mined block hash: {block.Hash}");
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CreateHash() ||
                    previousBlock.Hash != previousBlock.CreateHash())
                {
                    return false;
                }
            }

            return true;
        }
    }
}