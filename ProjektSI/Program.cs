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

int max_Promienskanu = 9;
while (true)
{
    // skanowanie w poszukiwaniu najbliższego PUNKTU
    // zwraca obiekt z jego położeniem
    Node? punkt = mapa.scanForPoint(max_Promienskanu);

    // jeżeli nie znaleciopno punktu umierasz
    if (punkt == null)
    {
        Console.WriteLine("Umarłeś");
        break;
    }
    else
    {
        // przekazanie mapy, współrzędnch gracza i wspórzędnych punktu i znak przezszkody
        // zwrucenie obiektu do przejścia przez a*(nie mój kod)
        // tutaj czasami wywala error bo zwraca null
        matrixNode endNode = Astar.FindPath(mapa.plansza, mapa.player.x, mapa.player.y, punkt.x, punkt.y, mapa.obstacle);
        Stack<matrixNode> path = new Stack<matrixNode>();

        // nie mój kod
        while (endNode.x != mapa.player.x || endNode.y != mapa.player.y)
        {
            path.Push(endNode);
            endNode = endNode.parent;
        }
        // nie mój kod
        path.Push(endNode);

        Console.WriteLine("The shortest path from  " +
                          "(" + mapa.player.x + "," + mapa.player.y + ")  to " +
                          "(" + punkt.x + "," + punkt.y + ")  is:  \n");

        while (path.Count > 0)
        {
            matrixNode node = path.Pop();
            Console.WriteLine("(" + node.x + "," + node.y + ")");
            // zktualizacja położenia gracza
            mapa.movePlayer(node.x,node.y);
            // wypisanie map
            mapa.printMap();
            Console.Clear();
            // miejsce w którym byliśmy zmenia sie w gwiazdkę
            mapa.plansza[node.y][node.x].znak = mapa.playerTrail;

        }
        // przesunięcie znaku gracza i aktualizaja jego pozycji
        mapa.movePlayer(punkt);
    }

    mapa.printMap();
    Console.Clear();
}
