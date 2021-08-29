using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Consola_Algoritmo_Busqueda
{
    public class SearchPath
    {

        Node _startingPoint;
        Node _endingPoint;
        string _startingPointString = "1";
        string _endingPointString = "2";
        string _pathString = "&";
        string _exploredNodeColor = "0";
        private string[] _lines; // aqui guardo las lineas del archivo de texto leido
        public SearchPath(string[] lines)
        {
            _lines = lines;
        }


        private Dictionary<string, Node> _block = new Dictionary<string, Node>();    // aqui la key va a ser "posx,posy" en string Diccionario de nodos que nohan sido explorados, contiene todos los nodos del laberinto
        private int[,] _directions = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };    // aqui guardo las direcciones como me voy a mover en el archivo de texto
        private Queue<Node> _queue = new Queue<Node>();         // Cola de nodos que ya han sido explorados, pero voy a buscar en sus vecinos 
        private Node _searchingPoint;                           // Current node we are searching
        private bool _isExploring = true;                       // If we are end then it is set to false

        private List<Node> _path = new List<Node>();            // For storing the path traversed


        public List<Node> Path
        {
            get
            {
                if (_path.Count == 0)                           // If we've already found path, no need to check it again
                {
                    LoadAllBlocks();
                    BFS();
                    CreatePath();
                }
                return _path;
            }
        }

        // For getting all nodes with Node.cs and storing them in the dictionary
        private void LoadAllBlocks()
        {
            for (int i = 0; i < _lines.Length; i++) //GetLenght(0) para fila y GetLenght(1) para columna
            {
                for (int j = 0; j < _lines[0].Length; j++)
                {
                    string key = i + "," + j;
                    Node node = new Node(i, j, _lines[i][j]);
                    if (_lines[i][j] != ' ')
                    {
                        if (_block.ContainsKey(key))
                        {
                            Console.WriteLine("2 Nodes present in same position. i.e nodes overlapped.");
                        }
                        else
                        {
                            if (node.Value == '1')
                            {
                                _startingPoint = node;
                            }
                            if (node.Value == '2')
                            {
                                _endingPoint = node;
                            }
                            _block.Add(key, node);// Add the position of each node as key and the Node as the value


                        }
                    }
                }
            }
        }


        // BFS; For finding the shortest path
        private void BFS()
        {
            _queue.Enqueue(_startingPoint); // Pongo en la cola el nodo inicial.

            while (_queue.Count > 0 && _isExploring)
            {// empiezo un ciclo infinito que caba cuando encuentro el nodo deseado.

                _searchingPoint = _queue.Dequeue(); // cojo el nodo en la cabeza de la cola y lo pongo en el nodo _SearchingPoint
                OnReachingEnd();//verifico si ya encontre el nodo final, por si debo de hacer el bool _isExploring falso
                ExploreNeighbourNodes();//exploro los vecinos en sus cuatro posibles direcciones 
            }
        }


        // To check if we've reached the Ending point
        private void OnReachingEnd()
        {
            if (_searchingPoint == _endingPoint)
            {// verifico si este nodo antes de buscarle los vecinos es el nodo final
                _isExploring = false;
                //Console.Write(_searchingPoint.Value+" = "+_endingPoint.Value);
            }
            else
            {
                _isExploring = true;
            }
        }


        // Searching the neighbouring nodes
        private void ExploreNeighbourNodes()
        {
            if (!_isExploring) { return; }//si el bool is_exploring es falso eso significa que esta funcion regresa sin hacer nada;
            for (int i = 0; i < _directions.GetLength(0); i++)
            {
                int neighbourPosx = _searchingPoint.Positionx + _directions[i, 0];
                int neighbourPosy = _searchingPoint.Positiony + _directions[i, 1];
                string key = neighbourPosx + "," + neighbourPosy;
                if (_block.ContainsKey(key))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
                {

                    Node node = _block[key];               // aqui tengo el vecino que estoy buscando en este ciclo
                    if (!node.isExplored)                           //verifico que no halla sido explorado
                    {
                        _queue.Enqueue(node);                       // Enqueueing the node at this position, lo meto a la cola, donde tengo los nodos esperando para explorar sus vecinos
                        node.isExplored = true;                      //ya fue explorado   
                        node.Value = '0';
                        node.isExploredFrom = _searchingPoint;      // le agrego desde que nodo le llegue, de esta manera puedo construir el camino
                        //Console.WriteLine("Node, Pos({0},{1}), value: {2}, previousNode: {3}", node.Positionx, node.Positiony, node.Value, node.isExploredFrom.Value);
                    }
                }
            }
            /*
            foreach (var direction in _directions)
            {// para cada direccion x,y hago un ciclo verificando los vecinos del nodo que voy a buscar
                int neighbourPosx = _searchingPoint.Positiony + direction;// aqui tengo almacenada la key del vector que estoy buscando.


                if (_block.ContainsKey(neighbourPos))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
                {
                    Node node = _block[neighbourPos];               // aqui tengo el vecino que estoy buscando en este ciclo

                    if (!node.isExplored)                           //verifico que no halla sido explorado
                    {
                        _queue.Enqueue(node);                       // Enqueueing the node at this position, lo meto a la cola, donde tengo los nodos esperando para explorar sus vecinos
                        node.isExplored = true;                      //ya fue explorado   
                        node.GetComponentInChildren<Renderer>().material.color = _exploredNodeColor;
                        node.isExploredFrom = _searchingPoint;      // le agrego desde que nodo le llegue, de esta manera puedo construir el camino
                    }
                }
            }
            */
        }

        // Aqui creo a partir del nodo final. Como todos los nodos tienen un atada la direccion de un nodo desde donde fueron explorados, puedo reconstruir el camino creando una lista _path que empieza desde el nodo final

        public void CreatePath()
        {
            SetPath(_endingPoint);
            Node previousNode = _endingPoint.isExploredFrom;// tengo el nodo previo al nodo final, esta estructura de datos que estan formando todos estos nodos se llama listas entrelazadas

            while (previousNode != _startingPoint)
            {//no termina hasta que no encuentre el nodo inicial
                SetPath(previousNode);// agrego el nodo a la lista
                previousNode = previousNode.isExploredFrom;// voy modificando este apuntador y le voy agregando todos los nodos previos a este
            }

            SetPath(_startingPoint);//el ultimo nodo del path va a ser obviamente el starting point, asi solamente vouy modificando las cordenadas del Player con esta lista.
            _path.Reverse();// reverso todos los elementos del path, debido a que no empiezo desde el final hasta el principio si no del principio hasta el final
            SetPathColor();
            PrintPath();

        }

        // For adding nodes to the path
        private void SetPath(Node node)
        {// simplemente para agruegar nodos al path
            _path.Add(node);
        }

        // Setting color to nodes
        private void SetPathColor()
        {
            for (int i = 1; i < Path.Count - 1; i++)
            {
                _path[i].Value = '&';
            }
            /*
            foreach (Node node in _path)
            {
                node.GetComponentInChildren<Renderer>().material.color = _pathColor; // cojo cada renderer de cada bloque que me hace parte de mi _path, y le cambio su color
            }
            //SetColor();
            */
        }
        private void PrintPath()
        {
            _startingPoint.Value = '1';
            _endingPoint.Value = '2';
            Console.WriteLine("Camino");
            foreach (var item in _path)
            {
                Console.WriteLine("Node, Pos({0},{1}), value: {2}", item.Positionx + 1, item.Positiony + 1, item.Value);
            }
        }
        // Setting color to start and end position
        /*
        private void SetColor()//simplemente para cambiarle el color al starting point y al endung point
        {
            _startingPoint.GetComponentInChildren<Renderer>().material.color = _startingPointString;
            _endingPoint.GetComponentInChildren<Renderer>().material.color = _endingPointString;
        }
        */

    }
}
