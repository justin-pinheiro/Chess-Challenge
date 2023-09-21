using ChessChallenge.API;

public class MyBot : IChessBot
{
    // test ;)
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        return moves[0];
    }
}