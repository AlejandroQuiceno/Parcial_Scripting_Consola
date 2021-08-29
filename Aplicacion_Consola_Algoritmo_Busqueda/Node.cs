using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion_Consola_Algoritmo_Busqueda
{
        public class Node
        {
            public bool isExplored = false;
            public Node isExploredFrom;
            private int positionx;
            private int positiony;
            private char value = ' ';
            public int Positiony { get => positiony; }
            public int Positionx { get => positionx; }
            public char Value { get => value; set => this.value = value; }

            public Node(int positionx, int positiony, char value)
            {
                this.positiony = positiony;
                this.positionx = positionx;
                this.value = value;
            }
        }
    
}
