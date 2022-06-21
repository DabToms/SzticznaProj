using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSI
{
    public class Map
    {
        public int wysokosc { get; set; }
        public int szerokosc { get; set; }

        public Node[][] plansza { get; set; }

        public Node player { get; set; }

        public char playerChar { get; set; }
        public char playerTrail { get; set; }

        public char point { get; set; }
        public char obstacle { get; set; }

        public Map(int x, int y)
        {
            this.wysokosc = y;
            this.szerokosc = x;
            plansza = new Node[this.wysokosc][];
            for (int j = 0; j < this.wysokosc; j++)
            {
                plansza[j] = new Node[this.szerokosc];
                for (int k = 0; k < this.szerokosc; k++)
                {
                    plansza[j][k] = new Node(k, j, ' ');
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
            player.y = y;
            player.x = x;
            plansza[player.y][player.x].znak = playerChar;
        }
        /// <summary>
        /// Przemieść gracza
        /// </summary>
        /// <param name="node"></param>
        public void movePlayer(Node node)
        {
            player.y = node.y;
            player.x = node.x;
            plansza[player.y][player.x].znak = playerChar;
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
                    plansza[0][j].znak = this.obstacle;
                }
                // 1/8 szansy na wygenerowanie punktu
                else if (r.NextDouble() <= punkt)
                {
                    plansza[0][j].znak = this.point;
                }
                else
                {
                    plansza[0][j].znak = ' ';
                }
            }

            for (int i = 1; i < wysokosc; i++)
            {
                for (int j = 0; j < szerokosc; j++)
                {
                    // 1/4 szansy na wygenerowanie ściany jeżeli na górze jest ściana
                    if (plansza[i-1][j].znak == '#' && r.NextDouble() <= przeszkoda/2)
                    {
                        plansza[i][j].znak = this.obstacle;
                    }
                    // 1/8 szansy na wygenerowanie ściany
                    else if (r.NextDouble() <= przeszkoda)
                    {
                        plansza[i][j].znak = this.obstacle;
                    }
                    // 1/8 szansy na wygenerowanie punktu
                    else if (r.NextDouble() <= punkt)
                    {
                        plansza[i][j].znak = this.point;
                    }
                    else
                    {
                        plansza[i][j].znak = ' ';
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
            this.player = new Node(x, y, character);
            this.playerChar = character;
            plansza[y][x] = this.player;
            this.playerTrail = trail;
        }

        /// <summary>
        /// Wypisane mapy
        /// </summary>
        public void printMap()
        {
            for (int i = 0; i < this.wysokosc; i++)
            {
                for (int j = 0; j < this.szerokosc; j++)
                {
                    Console.Write(this.plansza[i][j].znak);
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
        public Node? scanForPoint(int maxPromien)
        {
            // startowe pozycje skanowania, wybrane są rogi
            int startSkanX, startSkanY;
            // skanowanie ze zwiększającym się promieniem
            for (int promienSkanu = 1; promienSkanu <= maxPromien; promienSkanu++)
            {
                // Sprawdzenie wykroczenia poza planszę 
                // TUTAJ SPRAWDZIĆ I POPRAWIĆ
                if (this.player.x + promienSkanu > this.szerokosc || this.player.x - promienSkanu < 0 || this.player.y + promienSkanu > this.wysokosc || this.player.y - promienSkanu < 0)
                {
                    break;
                }

                startSkanX = this.player.x - promienSkanu;
                startSkanY = this.player.y - promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    // czy pole jest punktem
                    if (plansza[startSkanY + i][startSkanX].znak == this.point)
                    {
                        // zwracamy punkt
                        return plansza[startSkanY + i][startSkanX];
                    }
                    if (plansza[startSkanY][startSkanX + i].znak == this.point)
                    {
                        return plansza[startSkanY][startSkanX+i];
                    }
                }
                startSkanX = this.player.x + promienSkanu;
                startSkanY = this.player.y + promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    if (plansza[startSkanY - i][startSkanX].znak == this.point)
                    {
                        return plansza[startSkanY - i][startSkanX];
                    }
                    if (plansza[startSkanY][startSkanX -i].znak == this.point)
                    {
                        return plansza[startSkanY][startSkanX -i];
                    }
                }
            }
            return null;
        }
    }
}
