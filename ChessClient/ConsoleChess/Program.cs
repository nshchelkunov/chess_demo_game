using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessClient;

using static System.Console;


namespace ConsoleChess
{
    class Program
    {
        public const string HOST = "http://localhost:51278/api/Games/"; 
        public const string USER = "2";
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
        }

        ClientChess client;

        void Start()
        {
            client = new ClientChess(HOST, USER);
            WriteLine(client.host);
            WriteLine((client.GetCurrentGame()).FEN);
            while(true)
            {  
                Write("Your move:  ");
                string move = ReadLine();
                if (move == "q") return;
                //Clear();
                WriteLine(client.SendMove(move).FEN);
            }
        }
    }
}
