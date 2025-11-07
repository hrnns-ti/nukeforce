# NukeForce

NukeForce is a high-performance brute-force password tester in C# (.NET) designed for modern multicore systems.

It exhaustively tests all possible password combinations without relying on wordlists, efficiently distributing each password length into separate processes or terminal windows. Each process further divides its workload into multiple thread-based blocks, ensuring balanced and fully parallel execution across all CPU cores.

## Features

- **Pure brute-force**: Tests every possible password combination without wordlists
- **Multi-level parallelism**:
  - Runs separate processes for each password length (digit)
  - Each process divides its workload into multiple thread-based blocks for maximum CPU core usage
- **Fully parallel**: Optimized for maximum speed and efficiency across all CPU cores
- **Real-time monitoring**: Dedicated terminal windows per password length to track progress instantly
- **No overlap**: Intelligent work distribution ensures no duplicate testing
- **Instant success notification**: Displays password immediately when found

## Requirements

- .NET 6.0 or newer runtime and SDK
- Linux environment (tested on Arch Linux)
- Terminal emulator (`xterm`, `kitty`, `xfce4-terminal`, etc.) for auto run --faster


## Performance Tips

- Adjust the number of blocks based on your CPU core count (typically 8-32)
- For systems with many cores (16+), use higher block counts
- Small password lengths (1-2 characters) may not fully utilize all threads
- Optimal performance typically starts at 4+ character passwords

## How It Works

1. **Multi-Process Architecture**: Each password length runs in a separate process
2. **Block Division**: Each process divides the total combination space into blocks
3. **Parallel Execution**: Multiple threads process different blocks simultaneously
4. **Dynamic Chunking**: Threads automatically pick up new blocks as they complete
5. **No Overlap Guarantee**: Index-based distribution ensures no duplicate testing
6. **Would Use IP Spoof**: Soon, it can use IP Spoof to make it more deadly :D

## License

MIT License

## Disclaimer

This tool is for educational and authorized security testing purposes only. Unauthorized access to computer systems is illegal. Always obtain proper authorization before testing.

## Author

Created by [Haerunnas](https://github.com/hrnns-ti)

## Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the [issues page](https://github.com/hrnns-ti/nukeforce/issues).
