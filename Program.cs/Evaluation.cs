using System;
using System.Numerics;
using static Board;
public class Evaluation
{

    static readonly int[] ColorMultiplier = {1, -1};
    public int EvaluatePosition(Board board)
    {
        int score = 
        340 * (BitOperations.PopCount(board.pieceBitboards[(int)Piece.WhiteBishops]) - BitOperations.PopCount(board.pieceBitboards[(int)Piece.BlackBishops])) + 
        320 * (BitOperations.PopCount(board.pieceBitboards[(int)Piece.WhiteKnights]) - BitOperations.PopCount(board.pieceBitboards[(int)Piece.BlackKnights])) + 
        900 * (BitOperations.PopCount(board.pieceBitboards[(int)Piece.WhiteQueens]) - BitOperations.PopCount(board.pieceBitboards[(int)Piece.BlackQueens])) + 
        500 * (BitOperations.PopCount(board.pieceBitboards[(int)Piece.WhiteRooks]) - BitOperations.PopCount(board.pieceBitboards[(int)Piece.BlackRooks])) + 
        100 * (BitOperations.PopCount(board.pieceBitboards[(int)Piece.WhitePawns]) - BitOperations.PopCount(board.pieceBitboards[(int)Piece.BlackPawns]));

        score *= ColorMultiplier[board.colorToMove];


        return score;
    }

    public static readonly int[,] mvvLva = new int[6,6]
    {
        //victim
        // ROOK  BISHOP  KNIGHT  QUEEN  KING  PAWN
                                                        //attacker 
           {500,  380,    260,    620,     0,    140},     // ROOK
           {520,  400,    280,    640,     0,    160},     // BISHOP
           {540,  420,    300,    660,     0,    180},     // KNIGHT
           {480,  360,    240,    600,     0,    120},     // QUEEN
           {460,  340,    220,    580,     0,    100},     // KING
           {560,  440,    320,    680,     0,    200}      // PAWN
    };
}

/*
    mvvLva(3,5) -> Queen capturing pawn. 3-> attacker, 5-> victim. x = attacker. y = victim.
    knight capturing rook should be lower than rook capturing queen
    lowest score for free queen is 375 which is queen capturuing queen. Capturing any other free piece should always be lower than that of queen capturing queen.

    queen - 400, 430, 450, 375, 600 ----- 375 - 600 ----- 580 - 680
    rook - 340, 350, 360, 320, 380 ------ 320 - 380 ----- 460 - 560
    bishop - 280, 300, 310, 265, 310 ----- 265 - 310 ----- 340 - 440
    knight - 275, 280, 305, 260, 320 ----- 260 - 320 ----- 220 - 320
    pawn - 160, 170, 175, 150, 200 ----- 150 - 200 ----- 100-200


    public static readonly int[,] mvvLva = new int[6,6]
    {
        //victim
        // ROOK  BISHOP  KNIGHT  QUEEN  KING  PAWN
                                                        //attacker 
           {500,  380,    260,    620,     0,    140},     // ROOK
           {520,  400,    280,    640,     0,    160},     // BISHOP
           {540,  420,    300,    660,     0,    180},     // KNIGHT
           {480,  360,    240,    600,     0,    120},     // QUEEN
           {460,  340,    220,    580,     0,    100},     // KING
           {560,  440,    320,    680,     0,    200}      // PAWN
    };



*/