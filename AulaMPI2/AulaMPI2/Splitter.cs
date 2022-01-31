using System;
using System.Collections.Generic;
using System.Text;
using MPI;

namespace AulaMPI2 {
    public class Splitter {
        /*public List<object> Splitting(List<object> lista) {
            int num_nodes = MPIEnv.Size;
            int len_partition = lista.Count / num_nodes;
            RequestList bag = new RequestList();
            if (MPIEnv.Rank == MPIEnv.Root) { // Quem entra aqui é o nó gerente!!!
                if (lista.Count == 0 || (lista.Count % num_nodes != 0)) {
                    string msn = "Lista vazia, ou lista.Count nao é divisível perfeitamente por N processos!!!";
                    Console.WriteLine(msn);
                    throw new Exception(msn);
                }

                List<object> tmp = new List<object>(len_partition + 2);
                tmp.Add(lista[0]);

                int id_processo = 0;
                for (int i = 1; i <= lista.Count; i++) {

                    Console.WriteLine("i%len_partition: " + i % len_partition);
                    if (i % len_partition != 0) {
                        tmp.Add(lista[i]);
                    }
                    else {
                        int dest = id_processo++;
                        Console.WriteLine("Send to rank " + dest);
                        bag.Add(MPIEnv.Comm_world.ImmediateSend<object>(tmp, dest, TAGs.DIST_GERAL));
                        tmp = new List<object>(len_partition + 2);
                        if (i == lista.Count) break;
                        tmp.Add(lista[i]);
                    }
                }
            }

            object r = MPIEnv.Comm_world.Receive<object>(MPIEnv.Root, TAGs.DIST_GERAL);
            bag.WaitAll();
            return (List<object>)r;
        }*/

        public List<object> Splitting(List<object> lista)
        {
            int num_nodes = MPIEnv.Size;
            int count = 1;
            int j = 0;
            int c = 0;
            int aux = lista.Count;
            RequestList bag = new RequestList();
            if (MPIEnv.Rank == MPIEnv.Root)
            { // Quem entra aqui é o nó gerente!!!
                if (lista.Count == 0)
                {
                    string msn = "Lista vazia!";
                    Console.WriteLine(msn);
                    throw new Exception(msn);
                }

                if (lista.Count % num_nodes != 0)
                {

                    while ((aux - 1) % (num_nodes - 1) != 0)
                    {
                        count++;
                        aux--;
                    }
                }

                int len_partition = lista.Count / num_nodes;

                List<object> tmp = new List<object>(len_partition + 2);
                tmp.Add(lista[0]);

                int id_processo = 0;
                for (int i = 1; i <= lista.Count; i++)
                {
                    j += 1;
                    c += 1;
                    Console.WriteLine("i%len_partition: " + i % len_partition);
                    if (lista.Count % num_nodes != 0)
                    {
                        if (id_processo == 0) len_partition = count;
                        if (id_processo != 0) len_partition = (lista.Count - count) / (num_nodes - 1);
                        if (j == count + 1 && c == count + 1) j = 1;
                    }
                    if (j % len_partition != 0)
                    {
                        tmp.Add(lista[i]);
                    }
                    else
                    {

                        int dest = id_processo++;
                        Console.WriteLine("Send to rank " + dest);
                        bag.Add(MPIEnv.Comm_world.ImmediateSend<object>(tmp, dest, TAGs.DIST_GERAL));
                        tmp = new List<object>(len_partition + 2);
                        if (i == lista.Count) break;
                        tmp.Add(lista[i]);
                    }

                }
            }

            object r = MPIEnv.Comm_world.Receive<object>(MPIEnv.Root, TAGs.DIST_GERAL);
            bag.WaitAll();
            return (List<object>)r;
        }

        public static void Splitting2(ulong[][] matrix, RequestList bag)
        {
            int num_nodes = MPIEnv.Size;
            int count = 1;
            int j = 0;
            int c = 0;
            int aux = matrix.Length;

            if (MPIEnv.Rank == MPIEnv.Root)
            { // Quem entra aqui é o nó gerente!!!
                if (matrix.Length == 0)
                {
                    string msn = "Lista vazia!";
                    Console.WriteLine(msn);
                    throw new Exception(msn);
                }

                if (matrix.Length % num_nodes != 0)
                {

                    while ((aux - 1) % (num_nodes - 1) != 0)
                    {
                        count++;
                        aux--;
                    }
                }

                int len_partition = matrix.Length / num_nodes;

                List<object> tmp = new List<object>(len_partition + 2);
                tmp.Add(matrix[0]);

                int id_processo = 0;
                for (int i = 1; i <= matrix.Length; i++)
                {
                    j += 1;
                    c += 1;
                    Console.WriteLine("i%len_partition: " + i % len_partition);
                    if (matrix.Length % num_nodes != 0)
                    {
                        if (id_processo == 0) len_partition = count;
                        if (id_processo != 0) len_partition = (matrix.Length - count) / (num_nodes - 1);
                        if (j == count + 1 && c == count + 1) j = 1;
                    }
                    if (j % len_partition != 0)
                    {
                        tmp.Add(matrix[i]);
                    }
                    else
                    {

                        int dest = id_processo++;
                        Console.WriteLine("Send to rank " + dest);
                        bag.Add(MPIEnv.Comm_world.ImmediateSend<object>(tmp, dest, TAGs.DIST_GERAL));
                        tmp = new List<object>(len_partition + 2);
                        if (i == matrix.Length) break;
                        tmp.Add(matrix[i]);
                    }

                }
            }
        }
    }
}
