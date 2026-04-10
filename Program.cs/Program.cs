using System;

class Program
{
    static void Main()
    {
        Board board = new Board();
        MoveGenerator moveGenerator = new MoveGenerator(); 
        
        // Load the specific test FEN you want
        FenUtility.LoadFromFen(TestPositions.perft9, board);

        // Run your existing lab test
        PerftTool.PerftDivide(board, moveGenerator, 4);

        Console.ReadLine(); 
    }
}
