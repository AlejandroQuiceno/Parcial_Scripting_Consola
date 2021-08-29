using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
namespace Aplicacion_Consola_Algoritmo_Busqueda
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timeMeasure = new Stopwatch();
            List<Node> paths = new List<Node>();
            /*Para la construccion del laberinto sera, " "para los espacios en balcos, 0 para los bloques explorados y O para los no explorados, 1 para el bloque inicial y 2 para el bloque final, & para el camino*/
            string textlocation = "C:\\Users\\usuario\\Desktop\\U 2021-2\\Scripting\\maze.txt";
            if (File.Exists(textlocation))
            {
                string[] lines = File.ReadAllLines(textlocation);
                PrintMaze(lines);
                timeMeasure.Start();
                SearchPath searchPath = new SearchPath(lines);
                paths = searchPath.Path;
                timeMeasure.Stop();

            }
            Console.WriteLine("Tiempo de ejecución :" + timeMeasure.Elapsed.TotalMilliseconds + "ms");
            Console.ReadKey();
        }
        public static void PrintMaze(string[] _lines)
        {
            for (int i = 0; i < _lines.Length; i++) //GetLenght(0) para fila y GetLenght(1) para columna
            {
                for (int j = 0; j < _lines[0].Length; j++)
                {
                        Console.Write(_lines[i][j]);
                        Console.Write(" ");
                    
                }
                Console.WriteLine();
            }
        }
    }
}