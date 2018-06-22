using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;

namespace DemoChess
{
    class Program
    {
        static void Main(string[] args)
        {
            Chess.Chess chess = new Chess.Chess();
            
            while (true) // Выводит игру на консоль
            {
                Console.WriteLine(chess.fen);
                Print(ChessToAscii(chess));
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii (Chess.Chess chess) //Формирует текст игрового поля для консоли
        {
            string text = "  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += chess.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += "  +-----------------+\n";
            text += "    a b c d e f g h\n";
            return text;
        }

        static void Print (string text) // Подсветка фигур в консоли
        {
            ConsoleColor oldForeColor = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' &&  x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = oldForeColor;
        }
    }
}
