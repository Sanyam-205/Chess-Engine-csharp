using System;
using System.Numerics;
using System.Text.RegularExpressions;
using static Board;

public class Search
{
    public int nodeCount, leafCount;
    //alpha is the highest score the AI is guaranteed to achieve. Good for AI
    //beta is the lowest score the opponent can have. Good for human
    /*
                depth 0
           (A)  /     \  (B)
               3       1
              / \     / \
            -1   3  -5   1          
    White to move. +ve value= = better for white. -ve value = better for black. This is standard mimimax. Negamax is slightly different which will be explained later.
    
    In this example, for branch A, alpha is 3 which is the best score for white and beta is -1, the best score for black.
    For branch B, alpha is 1, the best score for white and beta is -5, the best score for black. We always assume that the opponent makes the best move for them leading to highest beta value.
    Beta can also be explained as the absolute best score we can have for that particular branch. For example, if -1 and -5 both continued downwards, these two scores would be the absolute best we would be looking to get if we went down those paths.
    Since at depth 0, the function must return the move order leading to best possible evaluation for you meaning the branch with best Alpha value. 
    
    */

    Move[,] pvTable = new Move[64,64]; //fixed 2d array to store the move and depth score.
    int[] pvLength = new int[64]; //need to track how long the move sequence is at each depth so it can be copied downward at the next depth level

    /* A principal variation table is used to store the sequence of moves that will result in the best move for that depth. At depth 6, the alpha value (score) will show the evaluation after the board has reached that particular stage. The root node is the final move at that particular depth. We need to store the sequence of moves. Here's how we do it.
    A 2d array in which, one element referes to the depth value as depth decreases in each recursive call of the loop and another sotring the best move found.
    Lets say, the best sequence of moves for a position is this -> e2e4, e7e5, d2d4, d7d5, g1f3, b8c6. The pvTable will look like this -
    depth    move sequence
      6      .e2e4
      5      .e2e4 .e7e5
      4      .e2e4 .e7e5 .d2d4 
      3      .e2e4 .e7e5 .d2d4 .d7d5 
      2      .e2e4 .e7e5 .d2d4 .d7d5 .g1f3
      1      .e2e4 .e7e5 .d2d4 .d7d5 .g1f3 .b8c6
    *If we were only storing the root node, it will just give is b8c6*

    To copy the content of the previous depth onto the next depth, we need another pvLength table.
    */

    //debug code
    public int StartSearch(Board board, MoveGenerator moveGenerator, Evaluation evaluation, int depth, int alpha, int beta, int ply)
    {
        nodeCount = 0; // Clear the board for the new search
        leafCount = 0;
        return NegaMax(board, moveGenerator, evaluation, depth, alpha, beta, ply);
    }

    public int NegaMax(Board board, MoveGenerator moveGenerator, Evaluation evaluation, int depth, int alpha, int beta, int ply) 
    {
        nodeCount++;//debug code
        
        pvLength[ply] = 0; //set it to 0 for each recursive call.

        if (depth == 0) 
        {
            leafCount++;
            // pvLength[ply] = 0; //signifies the search function finished searching
            return evaluation.EvaluatePosition(board);
        }
        //Populate moveList with pseudolegal moves
        Move[] moveList = new Move[256];
        int moveCount = 0;
        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);

        int legalMovesPlayed = 0;

        //Populate scores for each move in the moveList array in the same order.
        int[] moveScore = new int[moveCount];
        for(int i = 0; i<moveCount; i++)
        {
            Move newMove = moveList[i];
            moveScore[i] = ScoreMove(newMove, board);
        }


        //Apply selection sort to the moveScore array 

        //run moves in moveList through the legality check
        for (int i = 0; i < moveCount; i++)
        {
            //Selection sort           
            
            int bestMoveIndex = i;

            for(int j = i; j < moveCount; j++)
            {
                if(moveScore[j] > moveScore[bestMoveIndex])
                {
                    bestMoveIndex = j;
                }  
            }

            //swap the best move in moveList
            Move tempMove = moveList[i];
            moveList[i] = moveList[bestMoveIndex];
            moveList[bestMoveIndex] = tempMove;

            //swap their scores in moveScore.
            int temp = moveScore[i];
            moveScore[i] = moveScore[bestMoveIndex];
            moveScore[bestMoveIndex] = temp;
        

            
            // continuation of search
            Move move = moveList[i];
            board.MakeMove(move);

            int colorThatJustMoved = board.colorToMove ^ 1;
            int kingSquare = board.GetKingSquare(colorThatJustMoved);

            // Legality check
            if (kingSquare != -1 && board.IsSquareAttacked(kingSquare, colorThatJustMoved))
            {
                board.UnmakeMove(move);
                continue; 
            }

            legalMovesPlayed++;

            //Recursive call for lower depth.
            int score = -NegaMax(board, moveGenerator, evaluation, depth - 1, -beta, -alpha, ply+1);
        
            board.UnmakeMove(move);

            //Alpha-Beta pruning
            if (score >= beta) 
            {
                return score; //If this branch leads to a worse outcome, do not consider it. Return beta and get out of the path.
                //There is some confusion as to returning beta or score. I don't understand it yet but returning score is better because it gives more information somehow. I don't know.
            }
            if (score > alpha) 
            {
                alpha = score; //We found a better move for ourselves
             
                //debug
                // if (ply == 0)
                // {
                //     Console.WriteLine($"Root updating PV: Move is {move.StartSquare} to {move.TargetSquare}, Score is {score}");
                // } 

                pvTable[ply, 0] = move;

                // 2. Copy the sequence of moves from the deeper ply
                for (int j = 0; j < pvLength[ply + 1]; j++)
                {
                    pvTable[ply, j + 1] = pvTable[ply + 1, j];
                }

                // 3. Update the length of the sequence for this ply
                pvLength[ply] = pvLength[ply + 1] + 1;
            }


        }

        //STALEMATE, CHECKMATE CHECK.
        if(legalMovesPlayed == 0)
        {
            int colorToMove = board.colorToMove;
            int currentKingSquare = board.GetKingSquare(colorToMove);

            if((currentKingSquare != -1) && board.IsSquareAttacked(currentKingSquare, colorToMove))
            {
                return -9999; //checkmate
            }
            else
            {
                return 0; //stalemate
            }
        }

        // if (ply == 0)
        // {
        //     Console.WriteLine($"[DEBUG] Exiting Root: PV Length is {pvLength[0]}, Best move in array is {pvTable[0,0].StartSquare}, {pvTable[0,0].TargetSquare}");
        // }

        return alpha;
    }


    //Helper function
    public void PrintPrincipalVariation()
    {
        Console.Write("Best Line: ");
        for (int i = 0; i < pvLength[0]; i++)
        { 
            // Console.Write((pvTable[0,i].StartSquare) + "," + (pvTable[0,i].TargetSquare));
            Console.Write(BoardUtility.MoveToUci(pvTable[0,i]) + " ");
        }
        Console.WriteLine();
    }



    public int ScoreMove(Move move, Board board)
    {

        // int[] pieceValues = {5, 3, 3, 9, 10000, 1, 5, 3, 3, 9, 10000, 1};
        int[] pieceTypeMap = {0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5}; //rook, knight, bishop, queen, king, pawn

        if((move.Flag >= (int)Move.MoveFlag.promoteToQueen) && (move.Flag <= (int)Move.MoveFlag.promoteToBishop))
        {
            return 500; //Will fine tune this value later.
        }
        if((move.Flag >= (int)Move.MoveFlag.whiteKingSideCastle) && (move.Flag <= (int)Move.MoveFlag.blackQueenSideCastle))
        {
            return 50; //will fine tune this value later.
        }

        int movingPiece = board.pieceOnSquare[move.StartSquare];
        int capturedPieceType = board.pieceOnSquare[move.TargetSquare];
        //pieceOnSquare stores the piece type for every square. If there is a white queen on index 12 that is e2, then the 12th element in this array would be 3 since 3 is the WhiteQueen value on piece enum.

        if(move.Flag == (int)Move.MoveFlag.enPassantCapture)
        {
            capturedPieceType = (int)Board.Piece.WhitePawns + (board.colorToMove^1) * 6;
        }

        if(capturedPieceType != -1)
        {
            int capturedPieceIndex = pieceTypeMap[capturedPieceType];
            int movingPieceIndex = pieceTypeMap[movingPiece];

            // int capturedPieceValue = pieceValues[capturedPieceType];
            // int movingPieceValue = pieceValues[movingPiece];
            
            // //MVV - LVA. Most valuable capture, least valuable attacker.
            // //1000 is a base value
            // // return 1000 + (10 * capturedPieceValue) - movingPieceValue;

            return Evaluation.mvvLva[movingPieceIndex, capturedPieceIndex];
        }
        

        return 0;
    }

    


    /*
    MOVE ORDERING

            Attacker - 
    Victim               Rook  Knight  Bishop  Queen  King
                Rook
                
                Knight

                Bishop

                Queen

                King

    */


}