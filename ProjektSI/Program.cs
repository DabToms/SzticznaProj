using AStar;
using AStar.Options;
using ProjektSI;

// podanie szerokości i wysokości map
int x = 50;
int y = 50;
// inicjalizacja mapy
Map mapa = new Map(x, y);

// ustawienie znaków przeszkód(X) i punktów(@)
mapa.setCharacters('X','@');

// ustawienie prawdopodownieństwa respienia przeszkód(10%) i punktów(5%)
mapa.initialize(0.10,0.05);

// Ustawienie znaku gracza(g), znaku jego śladu(*) i jego początkowego położenia(x/2,y/2)
mapa.setPlayer('G','*', x/2, y/2);

// wypisanie mapy
mapa.printMap();
Console.Clear();

// Inicjalizacja Algorytmu A*
var pathfinderOptions = new PathFinderOptions
{
    PunishChangeDirection = true,
    UseDiagonals = false,
};

var worldGrid = new WorldGrid(mapa.plansza);
var pathfinder = new PathFinder(worldGrid, pathfinderOptions);

int max_Promienskanu = 9;
while (true)
{
    // skanowanie w poszukiwaniu najbliższego PUNKTU
    // zwraca obiekt z jego położeniem
    Position? punkt = mapa.scanForPoint(max_Promienskanu);

    // jeżeli nie znaleciopno punktu umierasz
    if (punkt == null)
    {
        Console.WriteLine("Umarłeś");
        Console.WriteLine($"Uzbierałeś: {mapa.wynik} Punktów");
        break;
    }
    else
    {
        Position[] path = pathfinder.FindPath(mapa.playerPos, (Position)punkt);

        foreach(var i in path)
        {
            mapa.movePlayer(i);

            mapa.printMap();
            Console.Clear();

            mapa.plansza[i.Row,i.Column] = mapa.playerTrailid;

        }

        mapa.movePlayer((Position)punkt);
    }

    mapa.printMap();
    Console.Clear();
}
