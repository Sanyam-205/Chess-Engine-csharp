using System;
using System.Numerics;
using static Board;

public static class Zobrist
{
    
    public static ulong[][] pieceHash = new ulong[64][];
    public static ulong[] sideToMoveKeys = new ulong[2];
    public static ulong[] castlingKeys = new ulong[16];
    public static ulong[] enPassantKeys = new ulong[8]; 

    static ulong randomState = 1804289383UL; // Standard non-zero seed
    
    static Zobrist()
    {
        Initialize();
    }

    public static ulong GetRandomUlong()
    {
        unchecked 
        {
            ulong x = randomState;
            x ^= x >> 12;
            x ^= x << 25;
            x ^= x >> 27;
            randomState = x;
            return x * 2685821657736338717UL;
        }
    }

    public static void Initialize()
    {
        for(int i = 0; i < 64; i++)
        {
            pieceHash[i] = new ulong[12];
        }
        for(int i = 0; i < 64; i++)
        {
            for(int j = 0; j<12; j++)
            {
                pieceHash[i][j] = GetRandomUlong();
                //In magic number generation in attack tables, we used (GetRandomUlong() & GetRandomUlong() & GetRandomUlong()) to generate a magic number with very sparse 1's count. But in piece hash key, we use GetRandomUlong() so we have an even mix of 0s and 1s. If we use a hash key leaning towards one side that could either be using too few 0s compared to 1, we mght generate same hash key for multiple positions.
            }
        }
        for(int i = 0; i<2; i++)
        {
            sideToMoveKeys[i] = GetRandomUlong();
        }
        for(int i = 0; i < 8; i++)
        {
            enPassantKeys[i] = GetRandomUlong();
        }
        for(int i = 0; i < 16; i++)
        {
            castlingKeys[i] = GetRandomUlong();
        }
    }

//WhiteRooks, WhiteKnights, WhiteBishops, WhiteQueens, WhiteKing, WhitePawns, BlackRooks, BlackKnights, BlackBishops, BlackQueens, BlackKing, BlackPawns
    public static ulong GenerateHash(Board board) //Will be called for each board state inside the search function.
    {
        ulong currentHash = 0;

        //hash the pieces. Index perfectly matches the index of pieceBitBoard. White rook is 0, White knight is 1 and so on.
        for(int i = 0; i < 12; i++)
        {
            ulong pieceboard = board.pieceBitboards[i];
            while(pieceboard!=0)
            {
                int square = BitOperations.TrailingZeroCount(pieceboard);
                currentHash ^= pieceHash[square][i];

                pieceboard &= pieceboard - 1;
            }
        }
        
        //hash turn
        currentHash ^= sideToMoveKeys[board.colorToMove];

        //hash castling rights
        currentHash ^= castlingKeys[board.castlingRights];

        //hash en passant 
        if (board.enPassantSquare != 0)
        {
            int epSquare = BitOperations.TrailingZeroCount(board.enPassantSquare);
            int epFile = epSquare % 8; //convert square to file value
            currentHash ^= enPassantKeys[epFile];
        }

        return currentHash;
    }

}

public static class RunZobrist
{
    


    public static void PrintHashKeys()
    {
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                Console.Write($"Element at [{i}][{j}]: {Zobrist.pieceHash[i][j]} ");
            }
            Console.WriteLine();
        }
    }
}
