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
        public void consumePoint(Node point)
        {
            plansza[point.x][point.y].znak = ' ';
        }
        public void movePlayer(int x, int y)
        {
            player.y = y;
            player.x = x;
            plansza[player.y][player.x].znak = playerChar;
        }
        public void movePlayer(Node node)
        {
            player.y = node.y;
            player.x = node.x;
            plansza[player.y][player.x].znak = playerChar;
        }
        public void setCharacters(char obstacle, char point)
        {
            this.obstacle = obstacle;
            this.point = point;
        }

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
        public void setPlayer(char character, char trail, int x, int y)
        {
            this.player = new Node(x, y, character);
            this.playerChar = character;
            plansza[y][x] = this.player;
            this.playerTrail = trail;
        }
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

        public Node? scanForPoint(int promien)
        {
            int aktualX, aktualY;
            for (int promienSkanu = 1; promienSkanu <= promien; promienSkanu++)
            {
                // Sprawdzenie wykroczenia poza planszę 
                if (this.player.x + promienSkanu > this.szerokosc || this.player.x - promienSkanu < 0 || this.player.y + promienSkanu > this.wysokosc || this.player.y - promienSkanu < 0)
                {
                    break;
                }

                aktualX = this.player.x - promienSkanu;
                aktualY = this.player.y - promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    if (plansza[aktualY + i][aktualX].znak == this.point)
                    {
                        return new Node(aktualX, aktualY+i, this.point);
                    }
                    if (plansza[aktualY][aktualX + i].znak == this.point)
                    {
                        return new Node(aktualX+i, aktualY, this.point);
                    }
                }
                aktualX = this.player.x + promienSkanu;
                aktualY = this.player.y + promienSkanu;
                for (int i = 0; i < promienSkanu * 2 + 1; i++)
                {
                    if (plansza[aktualY - i][aktualX].znak == this.point)
                    {
                        return new Node(aktualX, aktualY-i, this.point);
                    }
                    if (plansza[aktualY][aktualX -i].znak == this.point)
                    {
                        return new Node(aktualX-i, aktualY, this.point);
                    }
                }
            }
            return null;
        }
    }
}
