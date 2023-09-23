using ChessChallenge.API;
using System;

public class MyBot : IChessBot
{
    // Piece values: null, pawn, knight, bishop, rook, queen, king
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 10 };
    int numberOfMovesPlayed = 0;
    int highestMoveValue = -10;

    string[] whiteOpening1 = { "e2e4", "b1c3", "f1c4", "d2d3", "g1f3"};
    string[] blackOpening1 = { "f7f6", "b8c6", "e7e5", "f8c5", "g8e7"};
    
    public Move Think(Board board, Timer timer)
    {
        Move[] allMoves = board.GetLegalMoves();

        Move moveToPlay = Move.NullMove;
        
        string[] opening = GetOpening(board);

        moveToPlay = GetCheckmateMove(board, allMoves);
        
        if (!MoveFound(moveToPlay))
        {
            foreach (Move move in allMoves)
            {
                if(move.IsCapture)
                {
                    // Find highest value capture
                    Piece capturedPiece = board.GetPiece(move.TargetSquare);
                    Piece capturingPiece = board.GetPiece(move.StartSquare);
                    int moveValue = pieceValues[(int)capturedPiece.PieceType] - pieceValues[(int)capturingPiece.PieceType];

                    Console.WriteLine(move + " value : " + moveValue);

                    if (moveValue > highestMoveValue)
                    {
                        moveToPlay = move;
                        highestMoveValue = moveValue;
                    }
                }
            }

            highestMoveValue = -10;
        }

        if (!MoveFound(moveToPlay))
        {
            if (numberOfMovesPlayed < opening.Length)
            {
                // Get opening move based on color and moves played
                Move openingMove = new Move(opening[numberOfMovesPlayed], board);
                if(isMoveLegal(openingMove, allMoves)) moveToPlay = openingMove;
            }
        }

        if (!MoveFound(moveToPlay))
            moveToPlay = GetRandomLegalMove(allMoves);

        numberOfMovesPlayed++;
        Console.WriteLine(moveToPlay);
        return moveToPlay;
    }

    // Return true if moveToPlay is not null
    bool MoveFound(Move moveToPlay)
    {
        return !Move.NullMove.Equals(moveToPlay);
    }

    // Pick a random move to play among legal moves
    Move GetRandomLegalMove(Move[] allMoves)
    {
        Random rng = new();
        return allMoves[rng.Next(allMoves.Length)];
    }

    // Test if this move gives checkmate
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    // Test if this move gives check
    bool MoveIsCheck(Board board, Move move)
    {
        board.MakeMove(move);
        bool isCheck = board.IsInCheck();
        board.UndoMove(move);
        return isCheck;
    }

    // if a checkmate is possible, return the move, else return Move.NullMove
    Move GetCheckmateMove(Board board, Move[] allMoves)
    {
        foreach (Move move in allMoves)
        {
            if (MoveIsCheckmate(board, move)) return move;
        }
        return Move.NullMove;
    }

    // return true if the move is legal
    bool isMoveLegal(Move move, Move[] allMoves)
    {
        foreach (Move legalMove in allMoves)
        {
            if (move.Equals(legalMove))
            {
                return true; break;
            }
        }
        return false;
    }

    string[] GetOpening(Board board)
    {
        if (board.IsWhiteToMove) return whiteOpening1;
        else return blackOpening1;
    }
}