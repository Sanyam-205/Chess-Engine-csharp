using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Numerics;
using System.Runtime.CompilerServices;
using static Board;
class Program
{
    
    static void Main()
    {
        Board board = new Board();

    
        
        string fen0 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        // string fen1 = "r3r1k1/1bpp1p1p/p2b1q2/1p1npnN1/B2PP3/1PN1B3/P1PQ1PPP/R3R1K1 b KQkq - 0 1";
        // string fen2 = "r1bqkbnr/pppp1ppp/2n5/4p2Q/2B1P3/8/PPPP1PPP/RNB1K1NR w KQkq - 3 4"; //scholar's mate
        // string fen3 = "rnbqkbnr/pp1ppppp/8/2p5/4P3/8/PPPP1PPP/RNBQKBNR w KQkq c6 0 2"; //scillian
        // string fen4 = "rnbqkbnr/ppppp1pp/8/4Pp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 1"; //enPassant
        // string fen5 = "r1bqkb1r/pppppppp/2n2n2/3p4/1q2p1p1/P4N2/1PPPPPPP/RNBQKB1R b KQkq - 0 1"; //custom testing fen
        // string fen6 = "r3rkq1/p2qbp1q/1q3n2/3p3q/5Bq1/1PN2Q2/P1P1pPPP/3R1RK1 b - - 0 2";
        // string fen7 = "1R6/5pk1/1P4pp/8/1r6/4p2P/3p2P1/3K4 w - - 0 2";
        // string fen8 = "11R6/1r3pk1/6pp/8/8/4pQ1P/3p2P1/3K4 w - - 0 2";
        // string fen8 = "1R6/1r4k1/6pp/8/5pP1/4p2P/3p4/3K4 b - g3 0 2";
        //string fen9 = "1R6/6k1/6pp/8/5p2/4pnbP/3p2P1/1r2K3 w - - 0 2";
        string fen10 = "r3k2r/pp2bppp/1qnppn2/2p5/2bPP3/2N1BN2/PPPQ1PPP/R3K2R w KQkq - 0 1"; // white kingside COULD NOT be dont
        string fen11 = "r3k2r/pp2bppp/1qnppn2/2p5/2bPP3/2NNB3/PPPQ1PPP/R3K2R w KQkq - 0 1"; // white kingside castling could be done
        

        string perft2 = "r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1";
        string perft3 = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1 ";
        string perft4 = "8/1P6/2K5/8/8/8/2kp4/8 w - - 0 1"; //promotion
        string perft5 = "8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1";

        string perft9 = "r3k2r/p6p/8/B7/1pp1p3/3b4/P6P/R3K2R w KQkq - 0 1";

        string perft6 = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - ";
        
        string perft7 = "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1";
        string perft7g2g1q = "n1n5/PPPk4/8/8/8/8/4Kp1p/5NqN w - - 0 2";
        string perft7b7c8q = "n1Q5/P1Pk4/8/8/8/8/4Kp1p/5NqN b - - 0 2";
        

        string perft7d7c8 = "n1k5/P1P5/8/8/8/8/4Kp1p/5NqN w - - 0 3";
        string perft7f1h2 = "n1k5/P1P5/8/8/8/8/4Kp1N/6qN b - - 0 3";
        string perft7f2f1q = "n1k5/P1P5/8/8/8/8/4K2N/5qqN w - - 0 4";

        //error not likely in these
        string perft7d7c6 = "n1Q5/P1P5/2k5/8/8/8/4Kp1p/5NqN w - - 1 3";
        string perft7f1d2 = "n1Q5/P1P5/2k5/8/8/8/3NKp1p/6qN b - - 2 3";
        string perft7Af2f1q = "n1Q5/P1P5/2k5/8/8/8/3NK2p/5qqN w - - 0 4";
        string perft7f2f1n = "n1Q5/P1P5/2k5/8/8/8/3NK2p/5nqN w - - 0 4";
        // -------------------

        string perft8 = "rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8";
        
        
        
        
        
        
        FenUtility.LoadFromFen(perft9, board);

    


        MoveGenerator moveGenerator = new MoveGenerator(); //create instance of MoveGenerator


        /*
        Move[] moveList = new Move[256]; // Local array for this specific depth
        int moveCount = 0;

        moveGenerator.GenerateKingMoves(board, moveList, ref moveCount);

        // for(int i = 0; i < moveCount; i++)
        // {
        //     Console.WriteLine($"StartSquare = {IndexToSquare(moveList[i].StartSquare)} and Target Square = {IndexToSquare(moveList[i].TargetSquare)}");
        // } 

        Console.WriteLine(board.castlingRights);
        Move castle = new Move(4,6, (int)Move.MoveFlag.whiteKingSideCastle);

        

        //((board.castlingRights & 1) != 0) && ((board.AllPieces & ((1UL << 5) | (1UL << 6))) == 0) && 

        
        ulong mask = (board.AllPieces & ((1UL << 5) | (1UL << 6)));
        PrintUlongBitboard(mask);


        

        if (((board.castlingRights & 1) != 0) && ( mask == 0) &&
            !board.IsSquareAttacked(4, 0) && !board.IsSquareAttacked(5, 0) 
                && !board.IsSquareAttacked(6, 0))
        {
            
            board.MakeMove(castle);
            PrintUlongBitboard(board.AllPieces);
            PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteKing]);
            PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteRooks]);
            
            Console.WriteLine("Castling done");

            
        }    
        else
        {
            

            PrintUlongBitboard(board.AllPieces);
            PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteKing]);
            PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteRooks]);
            Console.WriteLine("Castling not done");
        }    

        Console.WriteLine(board.castlingRights);
        //*/

       
        /*
        // 2. Prepare the move list and counter
        // Move[] moveList = new Move[256];
        // int moveCount = 0;


        // BoardPrinter.PrintBitboard(board);

       
        // Move myMove = new Move(12,53);

        // // Console.WriteLine("Enter Move\n");
        // // string move = Console.ReadLine();

        // board.MovePiece(MoveUtility.MoveFromName("e2e8q"));
        
        // Console.WriteLine("After move -");

        // BoardPrinter.PrintBitboard(board);
        
        // void MakeMove(Board board)
        // {

        //     // moveGenerator.GenerateKnightMoves(board, moveList, ref moveCount);
        //     // moveGenerator.GenerateKingMoves(board, moveList, ref moveCount);
        //     // moveGenerator.GeneratePawnMoves(board, moveList, ref moveCount);
            
        
        //     int count = 0;
        //     while(count<4)
        //     {
        //         BoardPrinter.PrintBitboard(board);
        //         Console.WriteLine($"Enter move ");
        //         string move = Console.ReadLine();
        //         board.MovePiece(MoveUtility.MoveFromName(move));
        //         Console.WriteLine(board.colorToMove);

        //         // moveGenerator.GenerateKnightMoves(board, moveList, ref moveCount);
        //         // moveGenerator.GenerateKingMoves(board, moveList, ref moveCount);
        //         // moveGenerator.GeneratePawnMoves(board, moveList, ref moveCount);

                for(int i = 0; i < moveCount; i++)
                {
                    Console.WriteLine($"StartSquare = {IndexToSquare(moveList[i].StartSquare)} and Target Square = {IndexToSquare(moveList[i].TargetSquare)}");
                } 
               
        //         count++;
               
        //     }
        
        // }
        
        // MakeMove(board);
        // AttackTables.GenerateRookAttackTable();


         // ulong king = (board.colorToMove == 0) ? board.pieceBitboards[(int)Piece.WhiteKing] : board.pieceBitboards[(int)Piece.BlackKing];
        
        // int kingPos = BitOperations.TrailingZeroCount(king);
        // Console.WriteLine(board.IsSquareAttacked(kingPos,board.colorToMove));


        // Move move = new Move(1,18);
        // board.MovePiece(move);
        // BoardPrinter.PrintBitboard(board);

        // for (int i = 0; i<64; i++)
        // {
        //     ulong rookAttack = AttackTables.GenerateRookMask(i);    
        //     string s = $"Rook an square {IndexToSquare(i)}";
        //     Console.WriteLine($"Bitboatd = {rookAttack}");
        //     PrintUlongBitboard(rookAttack, s);
            
        // }
        
        // for (int i = 0; i<64; i++)
        // {
        //     ulong bishopAttack = AttackTables.GenerateBishopAttacks(i, 0UL);    
        //     string s = $"bishop attack on fly {IndexToSquare(i)}";
        //     Console.WriteLine($"Bitboard = {bishopAttack}");
        //     PrintUlongBitboard(bishopAttack, s);
            
        // }
        
        
        // int[] relevantRookBits = new int[64];
        // for(int rank = 7; rank >= 0; rank--)
        // {
        //     for (int file = 0; file <= 7; file++)
        //     {
        //         int square = rank*8 + file;

        //         ulong rookAttack = AttackTables.GenerateBishopMask(square);
                
        //         int count = 0;
        //         while(rookAttack != 0)
        //         {
                        
                        
        //             // Console.WriteLine($"Bitboard = {bishopAttack}");
        //             // PrintUlongBitboard(bishopAttack, "with blocker on 36 ");
                    
        //             int possibleMoves = BitOperations.TrailingZeroCount(rookAttack);
        //             rookAttack &=rookAttack-1;
        //             count+=1;
                    
        //         }
        //         relevantRookBits[square] = count;
        //         Console.Write($"{relevantRookBits[square]}, ");
        //         //Console.Write($"{count}, ");
        //     }
        // }    

        
        
        //Generate knight moves
        // moveGenerator.GenerateKnightMoves(board, moveList, ref moveCount);
        
        
        // for(int rank = 7; rank >= 0; rank--)
        // {
        //     for (int file = 0; file <= 7; file++)
        //     {
        //         int square = rank*8 + file;
                
        //         AttackTables.CalculateRookTable(square);
        //         Console.Write($"{AttackTables.RookMagicNumbers[square]}, ");       


        //     }
        // }

        // for(int i = 0; i < 64; i++)
        // {
        //     AttackTables.CalculateBishopTable(i);
        //     Console.Write($"{AttackTables.BishopMagicNumbersArr[i]}, ");
        // }
        
        // AttackTables.PrintResult();
       
        
       
       
        // moveGenerator.GenerateKnightMoves(board, moveList, ref moveCount);
        // moveGenerator.GenerateKingMoves(board, moveList, ref moveCount);
        // moveGenerator.GeneratePawnMoves(board, moveList, ref moveCount);
        // moveGenerator.GenerateRookMoves(board, moveList, ref moveCount);
        // moveGenerator.GenerateBishopMoves(board, moveList, ref moveCount);
        // moveGenerator.GenerateQueenMoves(board, moveList, ref moveCount);
        
        // moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);
        // for(int i = 0; i < moveCount; i++)
        // {
        //     Console.WriteLine($"StartSquare = {IndexToSquare(moveList[i].StartSquare)} and Target Square = {IndexToSquare(moveList[i].TargetSquare)}");
        // }
    */    

        /*        
        Move[] moveList = new Move[256]; // Local array for this specific depth
        int moveCount = 0;

        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);
        Move newMove = new Move(51, 58);   
        Console.WriteLine("All Pieces");
        PrintUlongBitboard(board.AllPieces);

        Console.WriteLine("BlackPieces");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.BlackPieces]);
        Console.WriteLine("White Queen");
        PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteQueens]);

        board.MakeMove(newMove);

        Console.WriteLine("BlackPieces after capture");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.BlackPieces]);
        Console.WriteLine("WhitePieces after capture");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.WhitePieces]);
        Console.WriteLine("White Queen");
        PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteQueens]);

        board.UnmakeMove(newMove);
        Console.WriteLine("UNMAKING MOVE");
         Console.WriteLine("All Pieces restored");
        PrintUlongBitboard(board.AllPieces);

        Console.WriteLine("BlackPieces restored");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.BlackPieces]);

        Console.WriteLine("BlackPieces before capture");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.BlackPieces]);
        Console.WriteLine("WhitePieces before capture");
        PrintUlongBitboard(board.colorBitboard[(int)PieceTeam.WhitePieces]);
        Console.WriteLine("White Queen");
        PrintUlongBitboard(board.pieceBitboards[(int)Piece.WhiteQueens]);
        */
        
//=======================================================================================================================================================================
//                                                                          Perft Testing
//=======================================================================================================================================================================

       long Perft(int depth)
        {
            if (depth == 0) 
            {
                return 1;
            }

            long nodes = 0;
            Move[] moveList = new Move[256]; // Local array for this specific depth
            int moveCount = 0;
            
            moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount); 

            for (int i = 0; i < moveCount; i++)
            {
                Move move = moveList[i];

                //Debug

                
                AssertNoStateLeak(board, move);



                //Debug end

                board.MakeMove(move);

                int colorThatJustMoved = board.colorToMove ^ 1;

                // Ensure the move didn't leave our own king in check
                int kingSquare = board.GetKingSquare(colorThatJustMoved); 
                
                if (kingSquare != -1 && !board.IsSquareAttacked(kingSquare, colorThatJustMoved)) 
                {
                    nodes += Perft(depth - 1);
                }

                board.UnmakeMove(move);
            }

            
            return nodes;
        }





        
        void PerftDivide(int depth)
        {
            long totalNodes = 0;
            Move[] moveList = new Move[256];
            int moveCount = 0;
            
            moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);

            for (int i = 0; i < moveCount; i++)
            {
                Move move = moveList[i];
                board.MakeMove(move);

                int colorThatJustMoved = board.colorToMove ^ 1;

                int kingSquare = board.GetKingSquare(colorThatJustMoved);
                
                if (!board.IsSquareAttacked(kingSquare, colorThatJustMoved))
                {
                    long nodes = Perft(depth - 1);
                    totalNodes += nodes;
                    
                    // We need to print the move and its specific node count here
                    Console.WriteLine($"{MoveToUci(move)}: {nodes}");
                }

                board.UnmakeMove(move);
            }

            Console.WriteLine($"\nTotal Nodes: {totalNodes}");
        }


        PerftDivide(4);



        /*
        
        f1d2, f1h2, f1g3, 


        f2f1q - (3,1)
        f2f1n - (25,23)

    My engine
        f2f1q: 4
        f2f1n: 8
        f2f1r: 9
        f2f1b: 5
        
    Stockfish
        f2f1q: 2
        f2f1r: 7
        f2f1b: 5
        f2f1n: 9

        */



        // for(int i = 1; i <= 3; i++)
        // {

        //     Console.WriteLine($"\nTotal Nodes for depth {i}: {Perft(i)}");

        // }

        // Console.WriteLine(Perft(1));



        // Console.WriteLine($"\nTotal Nodes for depth : {Perft(6)}");
        
        
        // int count = 0;
        // while(count<4)
        // {
        //     BoardPrinter.PrintBitboard(board);
        //     Console.WriteLine($" \nTurn : {board.colorToMove}");
        //     Console.WriteLine($"Enter move ");
        //     string move = Console.ReadLine();
        //     board.MakeMove(MoveUtility.MoveFromName(move));
        //     PrintUlongBitboard(board.AllPieces);
            
            
        //     count ++;
        // }
        
        // Move e2e4 = new Move(12,28,0);
        // Move e7e5 = new Move(52, 36, 0);
        // Move d1h5 = new Move(4, 39, 0);
        // board.MakeMove(e2e4);
        // PrintUlongBitboard(board.AllPieces);
        // board.MakeMove(e7e5);
        // PrintUlongBitboard(board.AllPieces);
        // board.MakeMove(d1h5);
        // PrintUlongBitboard(board.AllPieces);





        Console.ReadLine(); 
    }

    static string IndexToSquare(int index)
    {
        char file = (char)('a' + (index % 8));
        char rank = (char)('1' + (index / 8));
        
        return $"{file}{rank}";
    }
    
    static void PrintUlongBitboard(ulong bitboard)
    {
        // Console.WriteLine($"{title}:");
        for (int rank = 7; rank >= 0; rank--)
        {
            for (int file = 0; file <= 7; file++)
            {
                int square = rank * 8 + file;
                ulong mask = 1UL << square;
                char bitChar = (bitboard & mask) != 0 ? '1' : '.';
                Console.Write(bitChar + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }


    static string MoveToUci(Move move)
    {
        string uci = IndexToSquare(move.StartSquare) + IndexToSquare(move.TargetSquare);
        
        if (move.Flag >= (int)Move.MoveFlag.promoteToQueen && move.Flag <= (int)Move.MoveFlag.promoteToBishop)
        {
            // Append the piece type character
            switch (move.Flag)
            {
                case (int)Move.MoveFlag.promoteToQueen: uci += "q"; break;
                case (int)Move.MoveFlag.promoteToRook: uci += "r"; break;
                case (int)Move.MoveFlag.promoteToBishop: uci += "b"; break;
                case (int)Move.MoveFlag.promoteToKnight: uci += "n"; break;
            }
        }
        return uci;
    }

    // void TestStateLeak(Board board, Move move) 
    // {
    //     // 1. Create a deep copy of the perfectly clean board
    //     Board cleanBoard = board.Clone(); 

    //     // 2. Make and immediately Unmake the move on the real board
    //     board.MakeMove(move);
    //     board.UnmakeMove(move);

    //     // 3. Compare every single ulong
    //     if (board.AllPieces != cleanBoard.AllPieces) Console.WriteLine("AllPieces Leak!");
        
    //     for (int i = 0; i < 6; i++) {
    //         if (board.pieceBitboards[i] != cleanBoard.pieceBitboards[i]) {
    //             Console.WriteLine($"Piece {i} bitboard Leak!");
    //         }
    //     }
        
    //     for (int i = 0; i < 2; i++) {
    //         if (board.colorBitboard[i] != cleanBoard.colorBitboard[i]) {
    //             Console.WriteLine($"Color {i} bitboard Leak!");
    //         }
    //     }
        
    //     // Don't forget to check these!
    //     if (board.enPassantSquare != cleanBoard.enPassantSquare) Console.WriteLine("EP Leak!");
    //     if (board.castlingRights != cleanBoard.castlingRights) Console.WriteLine("Castle Leak!");
    // }
     



    static void AssertNoStateLeak(Board board, Move move) 
     {
    // 1. Cache the current state
        ulong preAll = board.AllPieces;
        ulong preWhite = board.colorBitboard[(int)PieceTeam.WhitePieces];
        ulong preBlack = board.colorBitboard[(int)PieceTeam.BlackPieces];
        
        ulong[] prePieces = new ulong[12];
        Array.Copy(board.pieceBitboards, prePieces, 12);
        
        // Cache the state variables directly
        ulong preEnPassant = board.enPassantSquare;
        int preCastling = board.castlingRights;

        // 2. Cycle the move
        board.MakeMove(move);
        board.UnmakeMove(move);

        // 3. Assert everything is identical
        if (preAll != board.AllPieces) throw new Exception($"AllPieces leaked on {MoveToUci(move)}");
        if (preWhite != board.colorBitboard[(int)PieceTeam.WhitePieces]) throw new Exception($"White occupancy leaked on {MoveToUci(move)}");
        if (preBlack != board.colorBitboard[(int)PieceTeam.BlackPieces]) throw new Exception($"Black occupancy leaked on {MoveToUci(move)}");
        
        for (int i = 0; i < 12; i++) {
            if (prePieces[i] != board.pieceBitboards[i]) {
                throw new Exception($"Piece type {i} bitboard leaked on {MoveToUci(move)}");
            }
        }
        
        if (preEnPassant != board.enPassantSquare) throw new Exception($"EP leaked on {MoveToUci(move)}");
        if (preCastling != board.castlingRights) throw new Exception($"Castling leaked on {MoveToUci(move)}");
    }

}
