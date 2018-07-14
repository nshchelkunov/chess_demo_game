using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessRules;

namespace ChessAPI.Controllers.Models
{
    public class Logic
    {
        private ModelChessDB db;

        public Logic()
        {
            db = new ModelChessDB();
        }
        public Games GetCurrentGame()
        {
            Games game = db
                .Games
                .Where(g => g.Status == "play")
                .OrderBy(g => g.ID)
                .FirstOrDefault();
            if (game == null)
                game = CreateNewGame();
            return game;
        }

        public Games GetGame (int id)
        {
            return db.Games.Find(id);
        }

        public Games MakeMove(int id, string move)
        {
            Games game = GetGame(id);
            if (game == null) return game;

            if (game.Status != "play")
                return game;

            Chess chess = new Chess(game.FEN);
            Chess chessNext = chess.Move(move);

            if (chessNext.fen == game.FEN)
                return game;
            game.FEN = chessNext.fen;
            //if (chessNext.IsCheckmate || chess.IsStalemate)
            //    game.Status = "done";
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }

        private Games CreateNewGame()
        {
            Games game = new Games();

            Chess chess = new Chess();

            game.FEN = chess.fen;
            game.Status = "play";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }


    }
}