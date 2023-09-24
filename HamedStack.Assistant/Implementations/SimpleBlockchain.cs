// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HamedStack.Assistant.Implementations;

/// <summary>
/// Represents a simple blockchain with generic data.
/// </summary>
/// <typeparam name="T">The type of data to be stored in the blockchain.</typeparam>
/// <remarks>
/// This is a basic blockchain implementation and may not include advanced features or security mechanisms of real-world blockchain solutions.
/// </remarks>
public class SimpleBlockchain<T>
{
    private readonly object _lockObject = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleBlockchain{T}"/> class.
    /// </summary>
    /// <remarks>
    /// The constructor initializes the blockchain with a genesis block.
    /// </remarks>
    public SimpleBlockchain()
    {
        Chain = new List<Block>
        {
            new(default)
        };
    }

    /// <summary>
    /// Gets the list of blocks that form the blockchain.
    /// </summary>
    public IList<Block> Chain { get; }

    /// <summary>
    /// Adds a new block to the blockchain.
    /// </summary>
    /// <param name="data">The data to be stored in the block.</param>
    /// <example>
    /// <code>
    /// var blockchain = new Blockchain&lt;string&gt;();
    /// blockchain.AddBlock("Hello, World!");
    /// </code>
    /// </example>
    public void AddBlock(T data)
    {
        lock (_lockObject)
        {
            var block = new Block(data);
            var latestBlock = Chain[^1];
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }
    }

    /// <summary>
    /// Represents a block in the blockchain.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="data">The data to be stored in the block.</param>
        internal Block(T? data)
        {
            Index = 0;
            TimeStampUtc = DateTimeOffset.UtcNow;
            PreviousHash = null;
            Data = data;
            Hash = CalculateHash();
        }

        /// <summary>
        /// Gets or sets the data stored in the block.
        /// </summary>
        internal T? Data { get; set; }

        /// <summary>
        /// Gets or sets the hash of the block.
        /// </summary>
        internal string Hash { get; set; }

        /// <summary>
        /// Gets or sets the index of the block in the blockchain.
        /// </summary>
        internal int Index { get; set; }

        /// <summary>
        /// Gets or sets the hash of the previous block in the blockchain.
        /// </summary>
        internal string? PreviousHash { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the block was created.
        /// </summary>
        internal DateTimeOffset TimeStampUtc { get; set; }

        /// <summary>
        /// Calculates the hash of the block.
        /// </summary>
        /// <returns>The calculated hash as a string.</returns>
        /// <remarks>
        /// The hash is calculated based on the block's timestamp, its previous block's hash, and its data.
        /// </remarks>
        internal string CalculateHash()
        {
            var data = JsonSerializer.Serialize(Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes($"{TimeStampUtc}-{PreviousHash ?? ""}-{data}");
            var outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
    }
}