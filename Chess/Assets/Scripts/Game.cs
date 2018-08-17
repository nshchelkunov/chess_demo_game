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
    const string HOST = "http://localhost:50000/api/Games/";
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
        // GameInfo Возвращает ответ сервера в виде структуры с данными по игре
        GameInfo game = client.GetCurrentGame();
        // Создаем доску 
        CreateBoard();
        // Отображаем фигуры
        ShowFigures();
	}
	
	void Update () 
	{   
        // Если фигура захваченна мышью
		if(dad.Action())
        {
            // С какой клетки
            string from = GetSquare (dad.pickPosition);
            // На какой клете отпущенна
            string to = GetSquare (dad.dropPosition);
            // Какая фигура
            string figure = chess.GetFigureAt(from).ToString();
            // Запись хода в строковом формате (Pe2e4)
            string move = figure + from + to;
            Debug.Log(move);
            // Сделать ход
            chess = new Chess(client.SendMove(move).FEN);
            // Отобразить новую позицию
            ShowFigures();
        }
    }
    // Создает шахматную доску
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

    //Принимает координаты нажатия, возвращает шахматную позицию: e2, h7..
    string GetSquare (Vector2 position) 
    {
        int x = Convert.ToInt32(position.x / 2.0);        
        int y = Convert.ToInt32(position.y / 2.0);
        return ((char)('a' + x)).ToString() + (y + 1).ToString();
    }

    // Размещает фигуры на доске
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

    // Создает фигуру на заданных координатах
    void PlaceFigure (string box, string figure, int x, int y)
    {
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure);
        GameObject goSquare = GameObject.Find("" + y + x);

        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;
    }
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
                // Если мышь нажата в статусе none
                if (IsMouseButtonPressed())
                    // То захватываем обьект, меняем состояние
                    PickUp();                
				break;
			case State.drag:
                // Если мышь нажата в статусе drag (т.е. обьект перетаскивается)
                if (IsMouseButtonPressed())
                    // То продолжаем его перетаскивать
                    Drag();
                // Если клавиша мыши (обьект) отпущена
                else
                {
                    // Отпустить обьект
                    Drop(); 			     
					return true; 			 
				}						   				
				break;
		}
		return false;
	}

    // Возвращает true, если нажата левая кнопка мыши
    bool IsMouseButtonPressed() 
	{
		return Input.GetMouseButton(0);
	}

	void PickUp()
	{
        // Сохраняем координаты нажатия мыши
        Vector2 clickPosition = GetClickPosition();
        // Сохраняем обьект на который было нажатие
        Transform clickedItem = GetItemAt (clickPosition); 
		if (clickedItem == null) return;
        // Позиция обьекта, по которому было нажатие
        pickPosition = clickedItem.position;
        // Записывает в item обьект на который было нажатие
        item = clickedItem.gameObject; 
		state = State.drag;
		offset = pickPosition - clickPosition;
    }

	Vector2 GetClickPosition()
	{
        // Возвращает координаты мыши
        return Camera.main.ScreenToWorldPoint (Input.mousePosition); 
	}

	Transform GetItemAt (Vector2 position)
	{
        // Возвращает массив обьектов на которые было нажатие.
        RaycastHit2D[] figures = Physics2D.RaycastAll (position, position, 0.5f); 
		if (figures.Length == 0)
			return null;
        // Возвращает первый обьект из массива
        return figures[0].transform; 

	}

    // Меняем позицию обьекта на текущюю
    void Drag() 
	{
		item.transform.position = GetClickPosition() + offset;
	}

    // Если фигура отпущена, то меняем состояние и обнуляем обьект.
    void Drop()
	{
        dropPosition = item.transform.position;
        state = State.none;
		item = null;
	}
}

