using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClient
{
    public struct GameInfo
    {
        public int GameID; // Индификатор игры
        public string FEN; // Позиция в нотации Форсайта
        public string Status; //Игра идет, ожидание, завершена
        //public string White; // Кто играет белыми
        //public string Black; // Кто играет черными
        //public string LastMove; // Какой ход был последним
        //public string YourColor; // Каким цветом вы играете
        //public bool IsYourMove; // Ваш текуший ход
        //public string OfferDraw; // Была ли ничья - код Draw 
        //public string Winner; // Имя победителя, если партия закончилась.

        public GameInfo (NameValueCollection list)
        {
            GameID = int.Parse(list["ID"]);
            FEN = list["FEN"];
            Status = list["Status"];
            //White = list["White"];
            //Black = list["Black"];
            //LastMove = list["LastMove"];
            //YourColor = list["YourColor"];
            //IsYourMove = bool.Parse(list["IsYourMove"]);
           // OfferDraw = list["OfferDraw"];
            //Winner = list["Winner"];
        }
    }
}
