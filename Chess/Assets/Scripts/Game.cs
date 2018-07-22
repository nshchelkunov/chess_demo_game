using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessClient;
using ChessRules;

public class Game : MonoBehaviour {

	DragAndDrop dad;
    Chess chess;
    public GameObject BlackSquare;
    public GameObject WhiteSquare;
    const string HOST = "http://localhost:51278/api/Games/";
    string USER = "2";

    ClientChess client;

    public Game()
	{
		dad = new DragAndDrop();
        chess = new Chess();
	}

	public void Start () 
	{
        client = new ClientChess(HOST, USER);
        GameInfo game = client.GetCurrentGame();
        Debug.Log(game.FEN);

        CreateBoard();
        ShowFigures();
	}
	
	void Update () 
	{
		if(dad.Action())
        {
            string from = GetSquare (dad.pickPosition);
            string to = GetSquare (dad.dropPosition);
            string figure = chess.GetFigureAt(from).ToString();
            string move = figure + from + to;
            Debug.Log(move);
            chess = new Chess(client.SendMove(move).FEN);
            ShowFigures();
        }
    }

    void CreateBoard()
    {
        GameObject cell;
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                if ((y + x) % 2 == 0)
                {
                    cell = (GameObject)Instantiate(BlackSquare, new Vector2(y * 2, x * 2), Quaternion.identity);
                    cell.name = "" + x + y;
                }
                else
                {
                    cell = (GameObject)Instantiate(WhiteSquare, new Vector2(y * 2, x * 2), Quaternion.identity);
                    cell.name = "" + x + y;
                } 
            }
        cell = (GameObject)Instantiate(WhiteSquare, new Vector2(17, 2), Quaternion.identity);
        cell.name = "99";
    }

    string GetSquare (Vector2 position) //Принимает координаты нажатия, возвращает шахматную позицию: e2, h7..
    {
        int x = Convert.ToInt32(position.x / 2.0);        
        int y = Convert.ToInt32(position.y / 2.0);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

    void ShowFigures()
    {
        int nr = 0;
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();
                if (figure == ".") continue;
                PlaceFigure ("Box" + nr, figure, x, y);
                nr++;
            }
        for (; nr < 32; nr++)
            PlaceFigure("Box" + nr, "q", 9, 9);
    }

    void PlaceFigure (string box, string figure, int x, int y)
    {
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure); // K R P ..
        GameObject goSquare = GameObject.Find("" + y + x);

        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;
    }

    /*void MarkValidFigures() // Подсвечивает фигуры которые могут ходить
    {
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
                MarkSquare(x, y, false);
        foreach (string moves in chess.YieldValidMoves())
        {
            int x, y;
            GetCoord(moves.Substring(1, 2), out x, out y);
            MarkSquare(x, y, true);
        }     
    }

    public void GetCoord (string name, out int x, out int y)
    {
        x = 9;
        y = 9;
        if (name.Length == 2 &&
            name[0] >= 'a' && name[0] <= 'h' &&
            name[1] >= '1' && name[1] <= '8')
        {
            x = name[0] - 'a'; // Создаем координаты от 0 до 7, 'a' - 'a' = 0, 'b' - 'a' = 1
            y = name[1] - '1';
        }
    }

    void MarkSquare (int x, int y, bool isMarket)
    {
        GameObject cell;
        string color = (x + y) % 2 == 0 ? "Black" : "White";
        GameObject goSquare = GameObject.Find("" + y + x);
        if (isMarket)
            cell = GameObject.Find(color + "SquareMarked"); // Подсвечивает клетку
        else
            cell = GameObject.Find(color + "Square"); // Убирает подсветку
        var spriteSquare = goSquare.GetComponent<SpriteRenderer>();
        var spriteCell = cell.GetComponent<SpriteRenderer>();
        spriteSquare.sprite = spriteCell.sprite;
    }
    */
}

class DragAndDrop
{
	enum State
	{
		none,
		drag
	}

    public Vector2 pickPosition { get; private set; }
    public Vector2 dropPosition { get; private set; }

    Vector2 offset;
	State state;
	GameObject item;

	public DragAndDrop ()
	{
		state = State.none;
		item = null;
	}

	public bool Action ()
	{
		switch (state)
		{
			case State.none:
				if (IsMouseButtonPressed())  //Если мышь нажата в статусе none
					PickUp();               //То захватываем обьект, меняем состояние
				break;
			case State.drag:
				if (IsMouseButtonPressed()) //Если мышь нажата в статусе drag (т.е. обьект перетаскивается)
					Drag();                //То продолжаем его перетаскивать
				else						   //Если клавиша мыши (обьект) отпущена
				{
					Drop(); 			    //Отпустить обьект
					return true; 			//
				}						   				
				break;
		}
		return false;
	}

	bool IsMouseButtonPressed() // Возвращает true, если нажата левая кнопка мыши
	{
		return Input.GetMouseButton(0);
	}

	void PickUp()
	{
		Vector2 clickPosition = GetClickPosition(); // Сохраняем координаты нажатия мыши
		Transform clickedItem = GetItemAt (clickPosition); // Сохраняем обьект на который было нажатие
		if (clickedItem == null) return;
        pickPosition = clickedItem.position; // Позиция обьекта, по которому было нажатие
		item = clickedItem.gameObject; // Записывает в item обьект на который было нажатие
		state = State.drag;
		offset = pickPosition - clickPosition;
        //Debug.Log(pickPosition.x + "  " + pickPosition.y);
    }

	Vector2 GetClickPosition()
	{
		return Camera.main.ScreenToWorldPoint (Input.mousePosition); //Возвращает координаты мыши
	}

	Transform GetItemAt (Vector2 position)
	{
		RaycastHit2D[] figures = Physics2D.RaycastAll (position, position, 0.5f); //Возвращает массив обьектов на которые было нажатие.
		if (figures.Length == 0)
			return null;
		return figures[0].transform; //Возвращает первый обьект из массива

	}

	void Drag() // Меняем позицию обьекта на текущюю
	{
		item.transform.position = GetClickPosition() + offset;
	}

	void Drop() // Если фигура отпущена, то меняем состояние и обнуляем обьект.
	{
        dropPosition = item.transform.position;
        state = State.none;
		item = null;
	}
}

