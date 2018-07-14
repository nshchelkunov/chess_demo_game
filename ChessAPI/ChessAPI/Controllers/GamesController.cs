using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChessAPI.Controllers.Models;

namespace ChessAPI.Controllers
{
    public class GamesController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: api/Game
        public Games GetGame()
        {
            Logic logic = new Logic();
            Games game = logic.GetCurrentGame();
            return game;
        }

        // GET: api/Games/5
        public Games GetGame(int id)
        {
            Logic logic = new Logic();
            Games game = logic.GetGame(id);
            return game;
        }

        public Games GetMove (int id, string move)
        {
            Logic logic = new Logic();
            Games game = logic.MakeMove(id, move);
            return game;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GamesExists(int id)
        {
            return db.Games.Count(e => e.ID == id) > 0;
        }
    }
}