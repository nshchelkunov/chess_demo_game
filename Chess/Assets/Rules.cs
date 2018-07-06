using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour {

	DragAndDrop dad;

	public Rules()
	{
		dad = new DragAndDrop();
	}

	void Start () 
	{
		
	}
	
	void Update () 
	{
		dad.Action();
	}
}

class DragAndDrop
{
	enum State
	{
		none,
		drag
	}

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
				// if (IsMouseButtonPressed()) //Если мышь нажата в статусе drag (т.е. обьект перетаскивается)
				//		Drag();                //То продолжаем его перетаскивать
				//else						   //Если обьект отпущен
				//{
				//	 	Drop(); 			    //Скинуть обьект
				//	 	return true; 			//
				//}						   
				
				break;
		}
		return false;
	}

	bool IsMouseButtonPressed()
	{
		return Input.GetMouseButton(0);
	}

	void PickUp()
	{
		Vector2 clickPosition = GetClickPosition(); // Сохраняем координаты нажатия мыши
		Transform clickedItem = GetItemAt (clickPosition); // Сохраняем обьект на который было нажатие
		if (clickedItem == null) return;
		item = clickedItem.gameObject; // Записывает в item обьект на который было нажатие
		state = State.drag;
		Debug.Log ("Picked up: " + item.name);
	}

	Vector2 GetClickPosition()
	{
		return Camera.main.ScreenToWorldPoint (Input.mousePosition); //Возвращает координаты
	}

	Transform GetItemAt (Vector2 position)
	{
		RaycastHit2D[] figures = Physics2D.RaycastAll (position, position, 0.5f); //Возвращает массив обьектов на которые было нажатие.
		if (figures.Length == 0)
			return null;
		return figures[0].transform; //Возвращает первый обьект из массива

	}
}

