using System;
using System.Linq;
public static class UCIUtility
{
    
    public static void Loop(Board board, MoveGenerator moveGenerator)
    {
        // Console.WriteLine("Custom Engine UCI initialized.")

        while (true)
        {
            string input = Console.ReadLine() ?? "";

            
            //GUI might send null input if pipeline breaks somehow
            if (string.IsNullOrEmpty(input)) continue;

            string[] tokens = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = tokens[0].ToLower();

            switch (command)
            {
                case "uci":
                    Console.WriteLine("id name BlowFISH 1.0");
                    Console.WriteLine("id author Sanyam");
                    // TODO: Print available options here (e.g., Hash size)
                    Console.WriteLine("uciok");
                    break;

                case "isready":
                    // The GUI uses this to ping the engine and ensure it has finished loading
                    Console.WriteLine("readyok");
                    break;

                case "ucinewgame":
                    // Clear hash tables, reset history heuristics, etc.
                    break;

                case "position":
                    ParsePosition(input, board, moveGenerator);
                    break;

                case "go":
                    ParseGo(input, board, moveGenerator);
                    break;

                case "perft":
                    if (tokens.Length > 1 && int.TryParse(tokens[1], out int pDepth))
                    {
                        PerftTool.PerftDivide(board, moveGenerator, pDepth);
                    }
                    break;

                case "stop":
                    // TODO: Set a flag to interrupt your search immediately
                    break;

                case "quit":
                    return; // Exit the loop and close the application

                default:
                    Console.WriteLine($"Unknown command: {input}");
                    break;
            }

        }


    }


    private static void ParsePosition(string input, Board board, MoveGenerator moveGenerator)
    {
        // Example inputs:
        // "position startpos"
        // "position startpos moves e2e4 e7e5"
        // "position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 moves e2e4"

        string[] tokens = input.Split(' ');
        int movesIndex = Array.IndexOf(tokens, "moves");

        // 1. Setup the initial board state
        if (tokens[1] == "startpos")
        {
            FenUtility.LoadFromFen(TestPositions.fen0, board);
        }
        else if (tokens[1] == "fen")
        {
            // Reconstruct the FEN string from the tokens
            int fenEndIndex = movesIndex == -1 ? tokens.Length : movesIndex;
            string fen = string.Join(" ", tokens.Skip(2).Take(fenEndIndex - 2));
            FenUtility.LoadFromFen(fen, board);
        }

        // 2. Apply any moves played after the initial position
        if (movesIndex != -1)
        {
            for (int i = movesIndex + 1; i < tokens.Length; i++)
            {
                string uciMove = tokens[i];
                ApplyUciMove(uciMove, board, moveGenerator);
            }
        }
    }


    private static void ApplyUciMove(string uciMove, Board board, MoveGenerator moveGenerator)
    {
        int startRank, startFile;
        int targetRank, targetFile;
        int targetSquare, startSquare;

        startFile = uciMove[0] - 'a';
        targetFile = uciMove[2] - 'a';

        startRank = uciMove[1] - '1';
        targetRank = uciMove[3] - '1';

        startSquare = startRank*8 + startFile;
        targetSquare = targetRank*8 + targetFile;


        //check for promotion
        int promotionFlag = 0;
        if(uciMove.Length == 5)  
        {
            char promotionPiece = uciMove[4];
            promotionFlag = promotionPiece switch
        {
            'q' => 1, // Queen promotion
            'r' => 3, // Rook promotion
            'b' => 4, // Bishop promotion
            'n' => 2, // Knight promotion
            _ => 0    // Default fallback
        };
        }
        
        Move[] moveList = new Move[256];
        int moveCount = 0;
        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);

        for (int i = 0; i < moveCount; i++)
        {
            Move move = moveList[i];
            if (move.StartSquare == startSquare && move.TargetSquare == targetSquare)
            {
                bool isPromotion = move.Flag >= (int)Move.MoveFlag.promoteToQueen && move.Flag <= (int)Move.MoveFlag.promoteToBishop;
                
                if (isPromotion)
                {
                    if (move.Flag == promotionFlag)
                    {
                        board.MakeMove(move);
                        return;
                    }
                }
                else if (!isPromotion && promotionFlag == 0)
                {
                    board.MakeMove(move);
                    return;
                }
            }
        }
    }

    private static void ParseGo(string input, Board board, MoveGenerator moveGenerator)
    {
        // The GUI can send many parameters: "go depth 5", "go wtime 30000 btime 30000", etc.
        string[] tokens = input.Split(' ');
        
        int depth = 5; // Default depth if none is provided
        
        for (int i = 0; i < tokens.Length; i++)
        {
            if (tokens[i] == "perft" && i + 1 < tokens.Length)
            {
                int.TryParse(tokens[i + 1], out depth);
                PerftTool.PerftDivide(board, moveGenerator, depth);
                return;
            }
            if (tokens[i] == "depth" && i + 1 < tokens.Length)
            {
                int.TryParse(tokens[i + 1], out depth);
            }
        }

        // TODO: Call your actual search function here
        // Move bestMove = Search.GetBestMove(board, moveGenerator, depth);
        // Console.WriteLine($"bestmove {BoardUtils.MoveToUci(bestMove)}");

        // For now, to test that UCI is working, we can just run perft:
        Console.WriteLine($"info string Running perft {depth} from UCI...");
        PerftTool.PerftDivide(board, moveGenerator, depth);
        // Placeholder for actual search. 
        // If 'go depth X' is called, it should ideally run Minimax/Negamax, not Perft.
        // Console.WriteLine($"info string Search not yet implemented. Depth requested: {depth}");
    }





}