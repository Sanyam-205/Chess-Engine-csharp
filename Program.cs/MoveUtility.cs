using System;

//parses string into move type. necessary for UCI implementation 
public class MoveUtility
{
    public static Move MoveFromName(string moveString)
    {
        int startRank, startFile;
        int targetRank, targetFile;
        int targetSquare, startSquare;

        startFile = moveString[0] - 'a';
        targetFile = moveString[2] - 'a';

        startRank = moveString[1] - '1';
        targetRank = moveString[3] - '1';

        startSquare = startRank*8 + startFile;
        targetSquare = targetRank*8 + targetFile;


        //check for promotion
        int flag = 0;
        if(moveString.Length == 5)  
        {
            char promotionPiece = moveString[4];
            flag = promotionPiece switch
        {
            'q' => 1, // Queen promotion
            'r' => 3, // Rook promotion
            'b' => 4, // Bishop promotion
            'n' => 2, // Knight promotion
            _ => 0    // Default fallback
        };
        }
        
        Move newMove = new Move(startSquare, targetSquare, flag);

        return newMove;

        //e2e4 - e - start file, 2 - start rank. move = startFile startRank targetFile targetRank; 
    }



}