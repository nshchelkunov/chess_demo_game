using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ChessClient
{
    public class ClientChess
    {
        public string host { get; private set; }
        public string user { get; private set; }

        int CurrentGameID;

        public ClientChess (string host, string user)
        {
            this.host = host;
            this.user = user;
        }

        // Возвращает ответ сервера в виде структуры с данными по игре
        public GameInfo GetCurrentGame() 
        {
            GameInfo game = new GameInfo(ParseResponse(CallServer()));
            CurrentGameID = game.GameID;
            return game;
        }
        // Отправляет на сервер запрос с ходом, возвращает контейнер с новыми данными по игре
        public GameInfo SendMove (string move)
        {
            string json = CallServer(move);
            var list = ParseResponse(json);
            GameInfo game = new GameInfo(list);
            return game;
        }

        // Делает запрос, возвращает ответ сервера
        private string CallServer(string param = "") 
        {
            Console.WriteLine(host + user + "/" + param);
            // Запрос ресурсов с сервера
            WebRequest request = WebRequest.Create(host + user + "/" + param);
            // Сохраняем ответ от сервера
            WebResponse response = request.GetResponse();
            // using указывает, когда объекты, использующие ресурсы, должны их освобождать.
            using (Stream stream = response.GetResponseStream()) 
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        // {"ID":2,"FEN":"rnbqkbnr/pppppppp/8/8/8/5P2/PPPPP1PP/RNBQKBNR b - - 0 1","Status":"play"}
        // Парсит ответ сервера в ассоциативный массив
        private NameValueCollection ParseResponse (string Response) 
        {
            NameValueCollection list = new NameValueCollection();
            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";
            foreach (Match m in Regex.Matches(Response, pattern))
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
            return list;
        }
    }
}
