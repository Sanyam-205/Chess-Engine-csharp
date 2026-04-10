using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
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