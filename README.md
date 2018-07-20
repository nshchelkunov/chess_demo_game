"Chess Demo Game" представляет собой клиент- серверную игру "Шахматы", создаваемую в демострационных целях.
Проект находится в стадии разработки, на данный момент реализованы основные компоненты архитектуры.

КЛИЕНТ - тестируется на платформах Android, Web (WebGL), Windows 10

Интерфейс: Unity 2018
	   Программная генерация доски с фигурами, реакция на Input.
	   chess_demo_game/Chess/Assets/Scripts/
	   Основная логика в Game.cs

Правила игры: библиотека ChessRules.dll
	   Cодержит набор классов определяющих правила игры и проверку ходов.
	   chess_demo_game/Chess_dll/Chess/
	   Консольный клиент, используемый для тестирования находится в chess_demo_game/Chess_dll/DemoChess/

Сетевая часть клиента: библиотека ChessClient.dll 
	   Здесь расположенна клиентская часть сетевого кода на основе HTTP API - ASP.NET.
	   chess_demo_game/ChessClient/ChessClient/
	   ClientChess.cs - формирует запрос, отправляет на сервер, принимает и парсит json, 
	   сохраняет данные в структуру GameInfo.cs
	   Консольный клиент: chess_demo_game/ChessClient/ConsoleChess/


СЕРВЕР - платформа Windows Server 2016

Сервер: ASP.NET Core Web Application 
	   chess_demo_game/ChessAPI/ChessAPI/
	   Реализует HTTP API.

Доступ к данным: ADO.NET Entity Framework (EF)

РСУБД: Microsoft SQL Server

Правила игры (для проверки хода на сервере): ChessRules.dll


