using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public class StockfishWrapper : IDisposable
{
    private readonly Process _stockfishProcess;
    private readonly StreamWriter _stdin;
    private readonly StreamReader _stdout;

    public StockfishWrapper(string path)
    {
        _stockfishProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };

        _stockfishProcess.Start();
        _stdin = _stockfishProcess.StandardInput;
        _stdout = _stockfishProcess.StandardOutput;

        // Initialize UCI
        _stdin.WriteLine("uci");
        _stdin.WriteLine("isready");
        
        // Wait for Stockfish to say "readyok"
        while (_stdout.ReadLine() != "readyok") { }
    }

    public Dictionary<string, long> GetPerftResults(string fen, int depth)
    {
        var results = new Dictionary<string, long>();

        // 1. Send the command
        _stdin.WriteLine($"position fen {fen}");
        _stdin.WriteLine($"go perft {depth}");

        // 2. Read the output line by line
        string? line;
        while (_stdout != null && (line = _stdout.ReadLine()) != null)
        {
            // Stockfish signals completion with this specific string
            if (line.StartsWith("Nodes searched:")) break;

            // Perft output lines look like "e2e4: 20"
            if (line.Contains(":"))
            {
                var parts = line.Split(':');
                string move = parts[0].Trim();
                if (long.TryParse(parts[1].Trim(), out long count))
                {
                    results[move] = count;
                }
            }
        }

        return results;
    }

    public void Dispose()
    {
        _stdin.WriteLine("quit");
        _stockfishProcess.WaitForExit();
        _stockfishProcess.Dispose();
    }
}