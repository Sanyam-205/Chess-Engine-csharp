using System;
using static Board;

public static class PerftTool
{
    public static long Perft(Board board, MoveGenerator moveGenerator, int depth)
    {
        if (depth == 0) 
        {
            return 1;
        }

        long nodes = 0;
        Move[] moveList = new Move[256]; 
        int moveCount = 0;
        
        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount); 

        for (int i = 0; i < moveCount; i++)
        {
            Move move = moveList[i];

            // AssertNoStateLeak(board, move);

            board.MakeMove(move);

            int colorThatJustMoved = board.colorToMove ^ 1;
            int kingSquare = board.GetKingSquare(colorThatJustMoved); 
            
            if (kingSquare != -1 && !board.IsSquareAttacked(kingSquare, colorThatJustMoved)) 
            {
                nodes += Perft(board, moveGenerator, depth - 1);
            }

            board.UnmakeMove(move);
        }
        
        return nodes;
    }

    public static Dictionary<string, long> PerftDivide(Board board, MoveGenerator moveGenerator, int depth)
    {
        var results = new Dictionary<string, long>();
        
        long totalNodes = 0;
        Move[] moveList = new Move[256];
        int moveCount = 0;
        
        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);

        for (int i = 0; i < moveCount; i++)
        {
            Move move = moveList[i];

            AssertNoStateLeak(board, move);

            board.MakeMove(move);

            int colorThatJustMoved = board.colorToMove ^ 1;
            int kingSquare = board.GetKingSquare(colorThatJustMoved);
            
            if (!board.IsSquareAttacked(kingSquare, colorThatJustMoved))
            {
                long nodes = Perft(board, moveGenerator, depth - 1);
                totalNodes += nodes;


                string uciMove = BoardUtility.MoveToUci(move);
            
                // Add to our dictionary for Stockfish comparison
                results[uciMove] = nodes; 
                
                Console.WriteLine($"{uciMove}: {nodes}");

                
                // Console.WriteLine($"{BoardUtils.MoveToUci(move)}: {nodes}");
            }

            board.UnmakeMove(move);
        }

        Console.WriteLine($"\nTotal Nodes: {totalNodes}");
        Console.WriteLine($"\nEngine Total Nodes: {totalNodes}");
        return results;

    }

    public static void AssertNoStateLeak(Board board, Move move) 
    {
        ulong preAll = board.AllPieces;
        ulong preWhite = board.colorBitboard[(int)PieceTeam.WhitePieces];
        ulong preBlack = board.colorBitboard[(int)PieceTeam.BlackPieces];
        
        ulong[] prePieces = new ulong[12];
        Array.Copy(board.pieceBitboards, prePieces, 12);
        
        ulong preEnPassant = board.enPassantSquare;
        int preCastling = board.castlingRights;

        board.MakeMove(move);
        board.UnmakeMove(move);

        if (preAll != board.AllPieces) throw new Exception($"AllPieces leaked on {BoardUtility.MoveToUci(move)}");
        if (preWhite != board.colorBitboard[(int)PieceTeam.WhitePieces]) throw new Exception($"White occupancy leaked on {BoardUtility.MoveToUci(move)}");
        if (preBlack != board.colorBitboard[(int)PieceTeam.BlackPieces]) throw new Exception($"Black occupancy leaked on {BoardUtility.MoveToUci(move)}");
        
        for (int i = 0; i < 12; i++) {
            if (prePieces[i] != board.pieceBitboards[i]) {
                throw new Exception($"Piece type {i} bitboard leaked on {BoardUtility.MoveToUci(move)}");
            }
        }
        
        if (preEnPassant != board.enPassantSquare) throw new Exception($"EP leaked on {BoardUtility.MoveToUci(move)}");
        if (preCastling != board.castlingRights) throw new Exception($"Castling leaked on {BoardUtility.MoveToUci(move)}");
    }
}
