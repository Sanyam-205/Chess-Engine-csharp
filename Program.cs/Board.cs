//pure c# script. No unity element. 
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
public class Board
{
    
    //combined 64 bit integers for black and white pieces
    public ulong AllPieces;

    public int colorToMove, castlingRights;
    public ulong enPassantSquare;

    

    //list containing 12 ulong integers. It is used to store separate ulong values for each peiece type. We store them in a list because it is much faster to get to a specific index in a list compared to 12 different if, else if branches.
    public ulong[] pieceBitboards = new ulong[12];
    
    //We create a enum Piece because in our list, each index, stores a value for a piece. Since remembering index 0 stores white rook values, 1 stores white knights values isn't exactly human friendly, we create a enum and call the list using (int)Piece.WhiteRooks.

    //-------------------------------------------------------------------------------------------------------------------------------------------//
    /// White rook, knight, bishop and queen being exactly 6 places apart from their black piece counterparts is what allows the promotion code 
    // (int finalPieceIndex = baseIndex + (colorToMove * 6);) to work. It's fragile but it works. SO DONT CHANGE
    //-------------------------------------------------------------------------------------------------------------------------------------------//

    public ulong[] colorBitboard = new ulong[2];

    public enum PieceTeam
    {
        WhitePieces, BlackPieces
    }

    public enum Piece
    {
        WhiteRooks, WhiteKnights, WhiteBishops, WhiteQueens, WhiteKing, WhitePawns, BlackRooks, BlackKnights, BlackBishops, BlackQueens, BlackKing, BlackPawns
    }

    /*
        0   WhiteRooks, 
        1   WhiteKnights, 
        2   WhiteBishops, 
        3   WhiteQueens, 
        4   WhiteKing, 
        5   WhitePawns, 
        6   BlackRooks, 
        7   BlackKnights, 
        8   BlackBishops, 
        9   BlackQueens, 
        10  BlackKing, 
        11  BlackPawns
    */

    

    public void MovePiece (Move move)
    {
        enPassantSquare = 0;

        // find out which piece is moving. 
        // since we're not using an array but bitboards, we can't update the board without updating the actual bitboard of that specific piece. 
        // we will use a XOR operator. As the name suggests, XOR returns 0 when value is 1 and 1 when value is 0. For our bitboard, it would simply turn the bit on start square to 0 and will turn on the bit on target square essentially moving the piece.

        
        ulong startMask = 1UL << move.StartSquare; //create a ulong variable where every bit is 0 other than the one on StartSquare
        ulong targetMask = 1UL << move.TargetSquare; //create a ulong variable where every bit is 0 other than the one on TargetSquare

        ulong moveMask = (1UL << move.StartSquare) | (1UL << move.TargetSquare); ////create another ulong variable where every bit is 0 other than those two bits where startMask and targetMask are 1


        for (int i = 0; i<12; i++) // loop through pieceBitboards list
        {
            if((startMask & pieceBitboards[i])!= 0) //Verify if a non empty square is selected. It means if starting square bitboard and piecebitboard both store 1. In essense, the square isn't empty.
            {
                if (colorToMove == 0) //capture logic if white's turn
                {
                    for (int j = 6; j<12; j++) //loop through 6-12 of pieceBitboards because 6-11 index contains black pieces.
                    {
                        pieceBitboards[j] &= ~targetMask;
                    }
                }
                else //capture logic for black's turn
                {
                    for (int j = 0; j<6; j++) //loop through 0-6 of pieceBitboards becasue 0-5 indexes contain white pieces.
                    {
                        pieceBitboards[j] &= ~targetMask;
                    }
                }
                
                //promotion logic
                if (move.Flag >= (int)Move.MoveFlag.promoteToQueen && move.Flag <= (int)Move.MoveFlag.promoteToBishop) 
                {
                   HandlePromotion(move, i, startMask, targetMask); 
                }

                //en passant logic
                // else if (move.Flag == (int)Move.MoveFlag.enPassantCapture)
                // {
                //     HandleEnPassant(move, i, startMask, targetMask);
                // }
                else 
                {
                    pieceBitboards[i] ^= moveMask; // Normal teleport. XOR operator. Since pieceBitboard and moveMask will be 1 only at start square, it will turn start square to 0. Since moveMask will be 0 for pieceBitboard, it will turn it 1, effectively moving the piece.
                }

                break;
                
                
            }
        }
        colorToMove ^= 1; //reverse the turn. Again, using XOR operator with 1 because 0 ^ 1 = 1, 1 ^ 1 = 0


    }

    private void HandlePromotion(Move move, int pawnIndex, ulong startMask, ulong targetMask)
    {
        pieceBitboards[pawnIndex] ^= startMask;//delete the pawn
        int baseIndex = Move.flagToBaseIndex[move.Flag];
        int finalPieceIndex = baseIndex + (colorToMove * 6);
        pieceBitboards[finalPieceIndex] |= targetMask;

    }   
    private void HandleEnPassant (Move move, int pawnIndex, ulong startMask, ulong targetMask)
    {
        pieceBitboards[pawnIndex] ^= (startMask | targetMask);
        int captureSquare = (colorToMove == 0) ? move.TargetSquare - 8 : move.TargetSquare + 8;

        ulong captureMask = 1UL <<captureSquare;

        int enemyPawnIndex = (colorToMove == 0) ? 6 : 0; // Black pawn is 6, White pawn is 0
        pieceBitboards[enemyPawnIndex] ^= captureMask;

    }

     




}
