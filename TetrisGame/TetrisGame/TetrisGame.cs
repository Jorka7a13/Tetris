using System;
using System.Collections.Generic;
using System.Threading;

class TetrisGame
{
    public static int fieldWidht = 9;
    public static List<Square> squareList = new List<Square>();
    public static int points = 0;

    static void PrintOnPosition(int x, int y, char c, ConsoleColor color)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }

    static void DrawField()
    {
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < Console.BufferHeight; i++)
        {
            Console.SetCursorPosition(fieldWidht + 1, i);
            Console.WriteLine("|"); 
        }

        Console.SetCursorPosition(fieldWidht + 5, 10);
        Console.Write("Score: " + points);

        for (int i = 0; i < squareList.Count; i++)
        {
            PrintOnPosition(squareList[i].x, squareList[i].y, squareList[i].character, squareList[i].color);
        }
    }

    static bool LowerSquareIsEmpty(Square square) 
    {
        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == square.y + 1 && currSquare.x == square.x)
            {
                return false;
            }
        }
        return true;
    }

    static bool LowerSquaresAreEmpty(Square square1, Square square2, Square square3, Square square4)
    {
        foreach (Square currSquare in squareList)
        {
            if (currSquare != square1 && currSquare != square2 && currSquare != square3 && currSquare != square4)
            {
                if (currSquare.y == square1.y + 1 && currSquare.x == square1.x)
                {
                    return false;
                }
                else if (currSquare.y == square2.y + 1 && currSquare.x == square2.x)
                {
                    return false;
                }
                else if (currSquare.y == square3.y + 1 && currSquare.x == square3.x)
                {
                    return false;
                }
                else if (currSquare.y == square4.y + 1 && currSquare.x == square4.x)
                {
                    return false;
                }
            }
        }
        return true;
    }

    static bool LeftSquareIsEmpty(Square square1, Square square2, Square square3, Square square4)
    {
        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == GetLeftSquare(square1, square2, square3, square4).y && currSquare.x == GetLeftSquare(square1, square2, square3, square4).x - 1)
            {
                return false;
            }
        }
        return true; 
    }

    static bool RightSquareIsEmpty(Square square1, Square square2, Square square3, Square square4)
    {
        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == GetRightSquare(square1, square2, square3, square4).y && currSquare.x == GetRightSquare(square1, square2, square3, square4).x + 1)
            {
                return false;
            }
        }
        return true;
    }

    static bool SquareShouldStop(Square square)
    {
        if (square.y + 1 < Console.BufferHeight && LowerSquareIsEmpty(square))
        {
            return false;
        }
        return true;
    }

    static bool SquaresShouldStop(Square square1, Square square2, Square square3, Square square4)
    {
        if (LowerSquaresAreEmpty(square1, square2, square3, square4))
        {
            if (square1.y + 1 < Console.BufferHeight && square2.y + 1 < Console.BufferHeight && square3.y + 1 < Console.BufferHeight && square4.y + 1 < Console.BufferHeight)
            {
                return false;
            }
        }
        return true;
    }
    
    static Square GetLeftSquare(Square square1, Square square2, Square square3, Square square4) // What if there are two left squares?
    {
        if (square1.x <= square2.x && square1.x <= square3.x && square1.x <= square4.x)
	    {
		    return square1;
	    }
        else if (square2.x <= square1.x && square2.x <= square3.x && square2.x <= square4.x)
        {
            return square2;
        }
        else if (square3.x <= square1.x && square3.x <= square2.x && square3.x <= square4.x)
        {
            return square3;
        }
        else if (square4.x <= square1.x && square4.x <= square2.x && square4.x <= square3.x)
        {
            return square4;
        }

        return null;
    }

    static Square GetRightSquare(Square square1, Square square2, Square square3, Square square4) // What if there are two right squares?
    {
        if (square1.x >= square2.x && square1.x >= square3.x && square1.x >= square4.x)
        {
            return square1;
        }
        else if (square2.x >= square1.x && square2.x >= square3.x && square2.x >= square4.x)
        {
            return square2;
        }
        else if (square3.x >= square1.x && square3.x >= square2.x && square3.x >= square4.x)
        {
            return square3;
        }
        else if (square4.x >= square1.x && square4.x >= square2.x && square4.x >= square3.x)
        {
            return square4;
        }

        return null;
    } 

    static void MoveSquaresDown(int deletedLine)
    {
        foreach (Square currSquare in squareList)
        {
            if (currSquare.y < deletedLine)
            {
                    currSquare.y++;
            }
        }
    }

    static void MoveSquaresToBottom(Square square1, Square square2, Square square3, Square square4)
    {
        while (!SquaresShouldStop(square1, square2, square3, square4))
        {
            square1.y++;
            square2.y++;
            square3.y++;
            square4.y++;
        }
    }

    static void MovePlayerLeftOrRight(Square square1, Square square2, Square square3, Square square4)
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                while (Console.KeyAvailable) Console.ReadKey(true); //Cleares the buffer
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (GetLeftSquare(square1, square2, square3, square4).x > 0 && LeftSquareIsEmpty(square1, square2, square3, square4))
                    {
                        square1.x--;
                        square2.x--;
                        square3.x--;
                        square4.x--;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow) 
                {
                    if (GetRightSquare(square1, square2, square3, square4).x < fieldWidht && RightSquareIsEmpty(square1, square2, square3, square4))
                    {
                        square1.x++;
                        square2.x++;
                        square3.x++;
                        square4.x++;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    MoveSquaresToBottom(square1, square2, square3, square4);
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    RotateBlock(square1, square2, square3, square4);
                }

                Console.Clear();
                DrawField();
            }
        }
    }

    static void MovePlayerDown(Square square1, Square square2, Square square3, Square square4)
    {
        if (LowerSquaresAreEmpty(square1, square2, square3, square4) && !SquaresShouldStop(square1, square2, square3, square4))
        {
            square1.y++;
            square2.y++;
            square3.y++;
            square4.y++;
        }
    }

    static void DeleteRow(int row)
    {
        for (int i = squareList.Count - 1; i > -1; i--)
        {
            if (squareList[i].y == row)
            {
                squareList.RemoveAt(i);
            }
        }
        points += 10;
    }

    static void RemoveRowsIfNecessary(Square square1, Square square2, Square square3, Square square4) 
    {
        int count = 0;
        int row1 = 0, row2 = 0, row3 = 0, row4 = 0;
        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == square1.y) 
            {
                count++;
            }
        }
        if (count == fieldWidht + 1)
        {
            DeleteRow(square1.y);
            row1 = square1.y;
        }
        count = 0;

        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == square2.y)
            {
                count++;
            }
        }
        if (count == fieldWidht + 1)
        {
            DeleteRow(square2.y);
            row2 = square2.y;
        }
        count = 0;

        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == square3.y)
            {
                count++;
            }
        }
        if (count == fieldWidht + 1)
        {
            DeleteRow(square3.y);
            row3 = square3.y;
        }
        count = 0;

        foreach (Square currSquare in squareList)
        {
            if (currSquare.y == square4.y)
            {
                count++;
            }
        }
        if (count == fieldWidht + 1)
        {
            DeleteRow(square4.y);
            row4 = square4.y;
        }
        count = 0;

        MoveSquaresDown(row1);
        MoveSquaresDown(row2);
        MoveSquaresDown(row3);
        MoveSquaresDown(row4);
    }

    static bool YouLose()
    {
        foreach (Square square in squareList)
        {
            if (square.y == 0 && square.x == fieldWidht / 2)
            {
                return true;
            }
        }
        return false;
    }

    static void DrawEndingScreen()
    {
        string message = "You Lose!";
        string score = "Your score: " + points.ToString();
        Console.Clear();
        Console.SetCursorPosition(Console.BufferWidth / 2 - message.Length / 2, 6);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(message);
        Console.SetCursorPosition(Console.BufferWidth / 2 - score.Length / 2, 10);
        Console.Write(score);
        Console.ReadKey(true);
    }

    static void GenerateBlock(int rand, Square square1, Square square2, Square square3, Square square4)
    {
        switch (rand)
        {
            case 1:
                square1.x = fieldWidht / 2 - 1;
                square1.y = 0;
                square1.color = ConsoleColor.Yellow;
                square2.x = fieldWidht / 2;
                square2.y = 0;
                square2.color = ConsoleColor.Yellow;
                square3.x = fieldWidht / 2 - 1;
                square3.y = 1;
                square3.color = ConsoleColor.Yellow;
                square4.x = fieldWidht / 2;
                square4.y = 1;
                square4.color = ConsoleColor.Yellow;
                break;
            case 2:
                square1.x = fieldWidht / 2 - 2;
                square1.y = 0;
                square1.color = ConsoleColor.Cyan;
                square2.x = fieldWidht / 2 - 1;
                square2.y = 0;
                square2.color = ConsoleColor.Cyan;
                square3.x = fieldWidht / 2;
                square3.y = 0;
                square3.color = ConsoleColor.Cyan;
                square4.x = fieldWidht / 2 + 1;
                square4.y = 0;
                square4.color = ConsoleColor.Cyan;
                break;
            case 3:
                square1.x = fieldWidht / 2;
                square1.y = 0;
                square1.color = ConsoleColor.Magenta;
                square2.x = fieldWidht / 2 - 1;
                square2.y = 1;
                square2.color = ConsoleColor.Magenta;
                square3.x = fieldWidht / 2;
                square3.y = 1;
                square3.color = ConsoleColor.Magenta;
                square4.x = fieldWidht / 2 + 1;
                square4.y = 1;
                square4.color = ConsoleColor.Magenta;
                break;
            case 4:
                square1.x = fieldWidht / 2;
                square1.y = 0;
                square1.color = ConsoleColor.Green;
                square2.x = fieldWidht / 2 + 1;
                square2.y = 0;
                square2.color = ConsoleColor.Green;
                square3.x = fieldWidht / 2 - 1;
                square3.y = 1;
                square3.color = ConsoleColor.Green;
                square4.x = fieldWidht / 2;
                square4.y = 1;
                square4.color = ConsoleColor.Green;
                break;
            case 5:
                square1.x = fieldWidht / 2 - 1;
                square1.y = 0;
                square1.color = ConsoleColor.Red;
                square2.x = fieldWidht / 2;
                square2.y = 0;
                square2.color = ConsoleColor.Red;
                square3.x = fieldWidht / 2;
                square3.y = 1;
                square3.color = ConsoleColor.Red;
                square4.x = fieldWidht / 2 + 1;
                square4.y = 1;
                square4.color = ConsoleColor.Red;
                break;
            case 6:
                square1.x = fieldWidht / 2 - 1;
                square1.y = 0;
                square1.color = ConsoleColor.Blue;
                square2.x = fieldWidht / 2 - 1;
                square2.y = 1;
                square2.color = ConsoleColor.Blue;
                square3.x = fieldWidht / 2;
                square3.y = 1;
                square3.color = ConsoleColor.Blue;
                square4.x = fieldWidht / 2 + 1;
                square4.y = 1;
                square4.color = ConsoleColor.Blue;
                break;
            case 7:
                square1.x = fieldWidht / 2 + 1;
                square1.y = 0;
                square1.color = ConsoleColor.DarkYellow;
                square2.x = fieldWidht / 2 - 1;
                square2.y = 1;
                square2.color = ConsoleColor.DarkYellow;
                square3.x = fieldWidht / 2;
                square3.y = 1;
                square3.color = ConsoleColor.DarkYellow;
                square4.x = fieldWidht / 2 + 1;
                square4.y = 1;
                square4.color = ConsoleColor.DarkYellow;
                break;
        }
    }

    static void RotateBlock(Square square1, Square square2, Square square3, Square square4) //TO FIX POSITIONING
    {
        int originX, originY; //Top Left
        originX = square1.x - 1;
        originY = square1.y - 1;

        if (originX > fieldWidht - 3) // IF CERTAIN BLOCK
        {
            originX--;
        }
        else if (originX < 2) // IF CERTAIN BLOCK
        {
            originX++;
        }

        int oldX, oldY;

        oldX = square1.x - originX;
        oldY = square1.y - originY;
        square1.x = 2 - oldY;
        square1.y = oldX;
        square1.x += originX;
        square1.y += originY;

        oldX = square2.x - originX;
        oldY = square2.y - originY;
        square2.x = 2 - oldY;
        square2.y = oldX;
        square2.x += originX;
        square2.y += originY;

        oldX = square3.x - originX;
        oldY = square3.y - originY;
        square3.x = 2 - oldY;
        square3.y = oldX;
        square3.x += originX;
        square3.y += originY;

        oldX = square4.x - originX;
        oldY = square4.y - originY;
        square4.x = 2 - oldY;
        square4.y = oldX;
        square4.x += originX;
        square4.y += originY;
    }

    static void Main(string[] args)
    {
        Console.BufferHeight = Console.WindowHeight = 20;
        Console.BufferWidth = Console.WindowWidth = 30;

        Random rand = new Random();

        Square square1 = new Square();
        Square square2 = new Square();
        Square square3 = new Square();
        Square square4 = new Square();

        GenerateBlock(rand.Next(1, 8), square1, square2, square3, square4);

        squareList.Add(square1);
        squareList.Add(square2);
        squareList.Add(square3);
        squareList.Add(square4);

        Thread thread = new Thread(new ThreadStart(() => MovePlayerLeftOrRight(square1, square2, square3, square4)));
        thread.Start();

        while (true)
        {
            Console.Clear();
            DrawField();
            MovePlayerDown(square1, square2, square3, square4);

            if (SquaresShouldStop(square1, square2, square3, square4))
            {
                thread.Abort();
                RemoveRowsIfNecessary(square1, square2, square3, square4);

                if (YouLose())
                {
                    break;
                }
                else
                {
                    square1 = new Square();
                    square2 = new Square();
                    square3 = new Square();
                    square4 = new Square();

                    GenerateBlock(rand.Next(1, 8), square1, square2, square3, square4);

                    squareList.Add(square1);
                    squareList.Add(square2);
                    squareList.Add(square3);
                    squareList.Add(square4);

                    thread = new Thread(new ThreadStart(() => MovePlayerLeftOrRight(square1, square2, square3, square4)));
                    thread.Start();
                }
            }
            System.Threading.Thread.Sleep(300);
        }
        DrawEndingScreen();
    }
}