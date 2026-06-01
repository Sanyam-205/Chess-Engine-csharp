using System;
using System.Numerics;
using static Board;

public class Search
{
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
    public int NegaMax(Board board, MoveGenerator moveGenerator, Evaluation evaluation, int depth, int alpha, int beta) 
    {

        if (depth == 0) return evaluation.EvaluatePosition(board);

        //Populate moveList with pseudolegal moves
        Move[] moveList = new Move[256];
        int moveCount = 0;
        moveGenerator.GenerateAllPseudoLegalMoves(board, moveList, ref moveCount);

        int legalMovesPlayed = 0;

        //run moves in moveList through the legality check
        for (int i = 0; i < moveCount; i++)
        {
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
            int score = -NegaMax(board, moveGenerator, evaluation, depth - 1, -beta, -alpha);
        
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

        return alpha;
    }




}