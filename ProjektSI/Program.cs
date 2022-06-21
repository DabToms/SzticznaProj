using ProjektSI;

int x = 50;
int y = 50;
Map mapa = new Map(x, y);
mapa.setCharacters('X','@');
mapa.initialize(0.10,0.05);
mapa.setPlayer('G','*', x/2, y/2);
mapa.printMap();
Console.Clear();

int max_Promienskanu = 9;
while (true)
{
    // skanowanie w poszukiwaniu najbliższego PUNKTU
    Node? punkt = mapa.scanForPoint(max_Promienskanu);

    if (punkt == null)
    {
        Console.WriteLine("Umarłeś");
        break;
    }
    else
    {
        // tutaj czasami wywala error bo zwraca null
        matrixNode endNode = Astar.FindPath(mapa.plansza, mapa.player.x, mapa.player.y, punkt.x, punkt.y, mapa.obstacle);
        Stack<matrixNode> path = new Stack<matrixNode>();

        while (endNode.x != mapa.player.x || endNode.y != mapa.player.y)
        {
            path.Push(endNode);
            endNode = endNode.parent;
        }
        path.Push(endNode);

        Console.WriteLine("The shortest path from  " +
                          "(" + mapa.player.x + "," + mapa.player.y + ")  to " +
                          "(" + punkt.x + "," + punkt.y + ")  is:  \n");

        while (path.Count > 0)
        {
            matrixNode node = path.Pop();
            Console.WriteLine("(" + node.x + "," + node.y + ")");
            mapa.movePlayer(node.x,node.y);
            mapa.printMap();
            Console.Clear();
            mapa.plansza[node.y][node.x].znak = mapa.playerTrail;

        }
        mapa.movePlayer(punkt);
        mapa.printMap();
        Console.Clear();
    }

    mapa.printMap();
    Console.Clear();
}
