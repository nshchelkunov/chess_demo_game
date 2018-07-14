using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class Moves
    {
        FigureMoving fm;
        Board board;

        public Moves (Board board)
        {
            this.board = board;
        }

        public bool CanMove (FigureMoving fm)
        {
            this.fm = fm;
            return
                CanMoveFrom() && // Можно ли пойти с этой клетки
                CanMoveTo() && // Можно ли пойти куда собрались ходить
                CanFigureMove(); // Может ли фигура сделать этот ход
        }

        bool CanMoveFrom()
        {
            return
                fm.from.OnBoard() &&
                fm.figure.GetColor() == board.moveColor;
        }

        bool CanMoveTo()
        {
            return fm.to.OnBoard() &&
                   Square.NoEqualsSquare(fm.from, fm.to)  &&
                   board.GetFigureAt(fm.to).GetColor() != board.moveColor;          
        }

        bool CanFigureMove()
        {
            switch (fm.figure)
            {
                case Figure.none:
                    return false;

                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) &&
                            CanStraightMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) &&
                            CanStraightMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();

                default: return false;
            }
        }
        
        bool CanPawnMove() //Может ли пешка ходить
        {
            if (fm.from.y < 1 || fm.from.y > 6)
                return false;
            int stepY = fm.figure.GetColor() == Color.white ? 1 : -1; //Направление движения вверж или вниз по доске
            return CanPawnGo(stepY) ||   //Может ли идти вперед на 1 клетку
                   CanPawnJump(stepY) || // Может ли идти через 1 клетку
                   CanPawnEat(stepY);    // Может ли есть
        }
        private bool CanPawnGo (int stepY)
        {
            if (board.GetFigureAt(fm.to) == Figure.none) //Если клетка куда указан ход пуста
                if (fm.DeltaX == 0)                      //Если ход по прямой
                    if (fm.DeltaY == stepY)              
                        return true;
            return false;
        }

        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigureAt(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == 2 * stepY) //Если ход через 1 клетку
                        if (fm.from.y == 1 || fm.from.y == 6) // Если ход из 1 или 6 горизонтали
                            if (board.GetFigureAt (new Square (fm.from.x, fm.from.y + stepY)) == Figure.none) // Если между клетками 'от' и 'куда' нет фигуры
                                return true;
            return false;


        }
        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAt(fm.to) != Figure.none)
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }

        private bool CanStraightMove() // Может ли двигаться прямо
        {
            Square at = fm.from;
            do
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY);
                if (Square.EqualsSquare(at, fm.to))
                    return true;
            } while (at.OnBoard() &&
                     board.GetFigureAt(at) == Figure.none);
            return false;
        }

        private bool CanKingMove() 
        {
            if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1)
                return true;
            return false;
        }
        private bool CanKnightMove()
        {
            if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
            if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
            return false;
        }

    }

