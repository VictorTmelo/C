using System;
using System.Collections;
using System.Collections.Generic;
using MPI;

namespace AulaMPI2
{
    public class AppMain
    {
        // Gerador de matriz:
        public static readonly int N = 16; // Nro de matrizes para multiplicar
        public static int MIN = 5;
        public static int MAX = 10;
        public static string file = "C://Users//Player Ghost//Desktop//AulaMPI2//AulaMPI2//Matrix.txt";
        public static RequestList bag = new RequestList();

        public static void Main(string[] args)
        {
            MPIEnv.mpi_start();
            //test1();
            //test2();
            multi();
            //sort();
            MPIEnv.mpi_stop();
        }
        public static void test2()
        {
            List<object> lista = new List<object>();
            Splitter splitter = new Splitter();
            List<ulong[][]> ms = Generator.MatrixList(file);


            if (MPIEnv.Rank == MPIEnv.Root)
            {
                for (int i = 0; i < ms.Count; i++)
                    lista.Add(ms[i]);
            }
            List<object> bloco = splitter.Splitting(lista);
            if (bloco != null)
            {
                Console.WriteLine("Rank" + MPIEnv.Rank + ":\n" + printMatrix(bloco));
            }
        }
        public static string printMatrix(List<object> lista)
        {
            string s = "";
            foreach (object o in lista)
            {
                ulong[][] ms = (ulong[][])o;
                s += ms.Length + " x " + ms[0].Length + "\n";
                foreach (var a in ms)
                {
                    foreach (var b in a)
                    {
                        s = s + b + " ";
                    }
                    s = s + "\n";
                }
            }
            return s;
        }
        /* // Aula anterior
        */
        public static void test1()
        {
            var lc = Generator.Gen(file, N, MIN, MAX);
            //MPIEnv.reduce();
            //MPIEnv.allReduce();
            //MPIEnv.immediateSendReceive();
            //MPIEnv.broadcast(10);
            //MPIEnv.scatter();
            //MPIEnv.allGather();
            //MPIEnv.allToAll();
        }

        public static void multi()
        {
            List<object> lista = new List<object>();
            Splitter splitter = new Splitter();
            List<ulong[][]> ms = Generator.MatrixList(file);

            int tag = 0;
            RequestList bag_requests = new RequestList();

            if (MPIEnv.Rank == MPIEnv.Root)
            {
                for (int i = 0; i < ms.Count; i++)
                    lista.Add(ms[i]);
            }

            List<object> bloco = splitter.Splitting(lista);

            if (bloco != null)
            {
                ulong[][] aux = (ulong[][])bloco[0];

                for (int i = 1; i < bloco.Count; i++)
                {
                    aux = Util.multiply(aux, (ulong[][])bloco[i]);
                }

                if (MPIEnv.Rank != MPIEnv.Root)
                {
                    bag_requests.Add(MPIEnv.Comm_world.ImmediateSend<ulong[][]>(aux, 0, tag));
                }

                bag_requests.WaitAll();

                if (MPIEnv.Rank == MPIEnv.Root)
                {
                    List<object> Resultados = new List<object>();
                    Resultados.Add(aux);

                    for (int i = 1; i < MPIEnv.Size; i++)
                    {
                        Resultados.Add(MPIEnv.Comm_world.Receive<object>(i, tag));
                    }

                    bag_requests.WaitAll();

                    ulong[][] aux2 = (ulong[][])Resultados[0];

                    for (int i = 1; i < MPIEnv.Size; i++)
                    {
                        aux2 = Util.multiply(aux2, (ulong[][])Resultados[i]);
                    }

                    List<object> Resultado = new List<object>();
                    Resultado.Add(aux2);


                    ulong check_sum = 0;
                    for (int i = 0; i < aux2.GetLength(0); i++)
                        for (int j = 0; j < aux2[i].GetLength(0); j++)
                            check_sum += aux2[i][j];

                    Console.WriteLine($"check_sum: " + check_sum);


                    Console.WriteLine("Resultados: " + MPIEnv.Rank + ":\n" + printMatrix(Resultados));

                    Console.WriteLine("Resultado: " + MPIEnv.Rank + ":\n" + printMatrix(Resultado));

                    sendToAll(aux2);
                }

                sort();
            }
        }

        public static void sendToAll(ulong[][] matrix)
        {
            Splitter.Splitting2(matrix, bag);
        }

        public static void sort()
        {
            /*ulong[][] a = new ulong[2][];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = new ulong[2];
            }
            a[0][0] = 3;
            a[0][1] = 2;
            a[1][0] = 4;
            a[1][1] = 1;*/
            //Console.WriteLine("a");

            RequestList bag_requests = new RequestList();

            List<object> messageReceived = (List<object>)MPIEnv.Comm_world.Receive<object>(MPIEnv.Root, TAGs.DIST_GERAL);

            bag.WaitAll();

            MPIEnv.Comm_world.Barrier();

            List<ulong[]> minhasLinhas = new List<ulong[]>();

            for (int i = 0; i < messageReceived.Count; i++)
            {
                minhasLinhas.Add((ulong[])messageReceived[i]);
            }

            Console.WriteLine("received: " + MPIEnv.Rank + ": " + "\n");


            for (int i = 0; i < minhasLinhas.Count; i++)
            {
                for (int j = 0; j < minhasLinhas[i].Length; j++)
                {
                    Console.Write(minhasLinhas[i][j] + " ");
                }

                Console.WriteLine();
            }

            if (minhasLinhas != null)
            {
                for (int i = 0; i < minhasLinhas.Count; i++)
                {
                    Util.insertionSort(minhasLinhas[i]);
                }

                if (MPIEnv.Rank != MPIEnv.Root)
                {
                    bag_requests.Add(MPIEnv.Comm_world.ImmediateSend<List<ulong[]>>(minhasLinhas, 0, TAGs.DIST_GERAL));
                }

                MPIEnv.Comm_world.Barrier();

                if (MPIEnv.Rank == MPIEnv.Root)
                {
                    List<ulong[]> linhasPreOrdenadas = new List<ulong[]>();

                    for (int i = 0; i < minhasLinhas.Count; i++)
                    {
                        linhasPreOrdenadas.Add(minhasLinhas[i]);
                    }

                    for (int i = 1; i < MPIEnv.Size; i++)
                    {
                        List<ulong[]> receivedLines = MPIEnv.Comm_world.Receive<List<ulong[]>>(i, TAGs.DIST_GERAL);

                        for (int j = 0; j < receivedLines.Count; j++)
                        {
                            linhasPreOrdenadas.Add(receivedLines[j]);
                        }
                    }
                    Console.WriteLine(linhasPreOrdenadas.Count + " Rankk:" + MPIEnv.Rank);

                    bag_requests.WaitAll();

                    /*Console.WriteLine("t1: ");

                    for (int i = 0; i < linhasOrdenadas.Count; i++)
                    {
                        for (int j = 0; j < linhasOrdenadas[i].Length; j++)
                        {
                            Console.Write(linhasOrdenadas[i][j]);
                        }

                        Console.WriteLine();
                    }*/

                    int colunas = linhasPreOrdenadas[0].Length;

                    ulong[] linhasOrdenadas = new ulong[linhasPreOrdenadas.Count * linhasPreOrdenadas[0].Length];

                    int cont = 0;

                    for (int i = 0; i < linhasPreOrdenadas.Count; i++)
                    {
                        for (int j = 0; j < linhasPreOrdenadas[i].Length; j++)
                        {
                            linhasOrdenadas[cont] = linhasPreOrdenadas[i][j];
                            cont++;
                        }
                    }

                    Console.WriteLine("Antes de ordenar as linhas recebidas: " + MPIEnv.Rank + ": " + "\n");

                    cont = 1;

                    for (int i = 0; i < linhasOrdenadas.Length; i++)
                    {
                        Console.Write(linhasOrdenadas[i] + " ");

                        if (cont == colunas)
                        {
                            Console.Write("\n");
                            cont = 0;
                        }

                        cont++;
                    }

                    Console.WriteLine("\n");

                    Util.insertionSort(linhasOrdenadas);

                    Console.WriteLine("Depois de ordenar as linhas recebidas: " + MPIEnv.Rank + ": " + "\n");

                    cont = 1;

                    for (int i = 0; i < linhasOrdenadas.Length; i++)
                    {
                        Console.Write(linhasOrdenadas[i] + " ");

                        if (cont == colunas)
                        {
                            Console.Write("\n");
                            cont = 0;
                        }

                        cont++;
                    }
                }
            }
        }
    }
}
