using AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSI
{
    public class Map
    {
        public int wynik = 0;
        public int wysokosc { get; set; }
        public int szerokosc { get; set; }

        public short[,] plansza { get; set; }

        public Position playerPos { get; set; }

        public char playerChar { get; set; }
        private short playerCharid = 4;

        public char playerTrail { get; set; }
        public short playerTrailid = 5;

        public char point { get; set; }
        private short pointid = 2;

        public char obstacle { get; set; }
        private short obstacleid = 0;

        public Map(int x, int y)
        {
            this.wysokosc = y;
            this.szerokosc = x;
            this.plansza = new short[x, y];
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    plansza[i, j] = 1;
                }
            }
        }

        /// <summary>
        /// Przemieść gracza
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void movePlayer(int x, int y)
        {
            playerPos = new Position(x,y);
            plansza[playerPos.Row, playerPos.Column] = playerCharid;
        }
        /// <summary>
        /// Przemieść gracza
        /// </summary>
        /// <param name="node"></param>
        public void movePlayer(Position node)
        {
            playerPos = node;
            plansza[playerPos.Row, playerPos.Column] = playerCharid;
        }
        /// <summary>
        /// Ustaw znaki przeszkód i punktów
        /// </summary>
        /// <param name="obstacle"></param>
        /// <param name="point"></param>
        public void setCharacters(char obstacle, char point)
        {
            this.obstacle = obstacle;
            this.point = point;
        }

        /// <summary>
        /// Inicjalizacja mapy z prawdopodobnieństwami na generację obiektów
        /// </summary>
        /// <param name="przeszkoda"></param>
        /// <param name="punkt"></param>
        public void initialize(double przeszkoda, double punkt)
        {
            Random r = new Random();

            for (int j = 0; j < szerokosc; j++)
            {
                // 1/4 szansy na wygenerowanie ściany
                if (r.NextDouble() <= przeszkoda)
                {
                    plansza[0, j] = this.obstacleid;
                }
                // 1/8 szansy na wygenerowanie punktu
                else if (r.NextDouble() <= punkt)
                {
                    plansza[0, j] = this.pointid;
                }
            }

            for (int i = 1; i < szerokosc; i++)
            {
                for (int j = 0; j < wysokosc; j++)
                {
                    // 1/4 szansy na wygenerowanie ściany jeżeli na górze jest ściana
                    if (plansza[i-1, j] == this.obstacleid && r.NextDouble() <= przeszkoda/2)
                    {
                        plansza[i, j] = this.obstacleid;
                    }
                    // 1/8 szansy na wygenerowanie ściany
                    else if (r.NextDouble() <= przeszkoda)
                    {
                        plansza[i, j] = this.obstacleid;
                    }
                    // 1/8 szansy na wygenerowanie punktu
                    else if (r.NextDouble() <= punkt)
                    {
                        plansza[i, j] = this.pointid;
                    }
                }
            }
        }
        /// <summary>
        /// Inicjalizacja gracza
        /// </summary>
        /// <param name="character"></param>
        /// <param name="trail"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void setPlayer(char character, char trail, int x, int y)
        {
            this.playerChar = character;
            plansza[x, y] = this.playerCharid;
            playerPos = new Position(x, y);
            this.playerTrail = trail;
        }

        /// <summary>
        /// Wypisane mapy
        /// </summary>
        public void printMap()
        {
            for (int i = 0; i < this.szerokosc; i++)
            {
                for (int j = 0; j < this.wysokosc; j++)
                {
                    if (this.plansza[i, j] == this.obstacleid)
                    {
                        Console.ForegroundColor = System.ConsoleColor.Red;
                        Console.Write(obstacle);
                    }
                    else if (this.plansza[i, j] == this.playerCharid)
                    {
                        Console.ForegroundColor = System.ConsoleColor.Green;
                        Console.Write(playerChar);
                    }
                    else if (this.plansza[i, j] == this.playerTrailid)
                    {
                        Console.ForegroundColor = System.ConsoleColor.Blue;
                        Console.Write(playerTrail);
                    }
                    else if (this.plansza[i, j] == this.pointid)
                    {
                        Console.ForegroundColor = System.ConsoleColor.Yellow;
                        Console.Write(point);
                    }
                    else
                    {
                        Console.Write(' ');
                    }

                }
                Console.WriteLine();
            }
            Thread.Sleep(500);

        }

        /// <summary>
        /// Skanowanie punktów z coraz większym promieniem
        /// </summary>
        /// <param name="maxPromien">Maksmalny promień skanowania</param>
        /// <returns></returns>
        public Position? scanForPoint(int maxPromien)
        {
            // startowe pozycje skanowania, wybrane są rogi
            int startSkanX, startSkanY;
            // skanowanie ze zwiększającym się promieniem
            for (int promienSkanu = 1; promienSkanu <= maxPromien; promienSkanu++)
            {
                // Sprawdzenie wykroczenia poza planszę 
                // TUTAJ SPRAWDZIĆ I POPRAWIĆ
                if (this.playerPos.Row + promienSkanu > this.szerokosc || this.playerPos.Row - promienSkanu < 0 || this.playerPos.Column + promienSkanu > this.wysokosc || this.playerPos.Column - promienSkanu < 0)
                {
                    break;
                }

                startSkanX = this.playerPos.Row - promienSkanu;
                startSkanY = this.playerPos.Column - promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    // czy pole jest punktem
                    if (plansza[startSkanX + i, startSkanY] == this.pointid)
                    {
                        // zwracamy punkt
                        wynik++;
                        return new Position(startSkanX + i, startSkanY);
                    }
                    if (plansza[startSkanX, startSkanY + i] == this.pointid)
                    {
                        wynik++;
                        return new Position(startSkanX, startSkanY+i);
                    }
                }
                startSkanX = this.playerPos.Row + promienSkanu;
                startSkanY = this.playerPos.Column + promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    if (plansza[startSkanX - i, startSkanY] == this.pointid)
                    {
                        wynik++;
                        return new Position(startSkanX - i, startSkanY);
                    }
                    if (plansza[startSkanX, startSkanY - i] == this.pointid)
                    {
                        wynik++;
                        return new Position(startSkanX, startSkanY -i);
                    }
                }
            }
            return null;
        }
    }
}
