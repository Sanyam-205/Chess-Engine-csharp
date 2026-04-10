using System;
using static Board;

public static class BoardUtils
{
    public static string IndexToSquare(int index)
    {
        char file = (char)('a' + (index % 8));
        char rank = (char)('1' + (index / 8));
        return $"{file}{rank}";
    }
    
    public static void PrintUlongBitboard(ulong bitboard)
    {
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

    public static string MoveToUci(Move move)
    {
        string uci = IndexToSquare(move.StartSquare) + IndexToSquare(move.TargetSquare);
        
        if (move.Flag >= (int)Move.MoveFlag.promoteToQueen && move.Flag <= (int)Move.MoveFlag.promoteToBishop)
        {
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
}
