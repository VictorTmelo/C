using System;
using System.Collections.Generic;
using System.Text;
using MPI;

namespace MPI_Teste
{
    public class MPITest{

        protected MPI.Environment mpi = null;
        protected Intracommunicator comm_world;
        protected readonly int ROOT = 0;
        protected int rank;
        protected int size;


        public void mpi_start() {

            string[] args = System.Environment.GetCommandLineArgs();

            mpi = new MPI.Environment(ref args);

            comm_world = Communicator.world;

            size = comm_world.Size;

            rank = comm_world.Rank;

        }

        public void mpi_stop() {

            mpi.Dispose();
        }

        public void reduceAdd() {

            Console.WriteLine("Sou id " + rank + "! Somos " + size + " hosts.");

            int sum = rank;

            sum = comm_world.Reduce<int>(sum, Operation<int>.Add, ROOT);

            if(rank == ROOT) {
                Console.WriteLine("Oi Sou o ROOT! A soma dos ranks é: " + sum);
            }

            comm_world.Barrier(); // TAREFAS VÃO AGUARDAR AQUI

        }


        public void reduceMultiply()
        {

            Console.WriteLine("Sou id " + rank + "! Somos " + size + " hosts.");

            int mul = rank;

            mul = comm_world.Reduce<int>(mul, Operation<int>.Multiply, ROOT);

            if (rank == ROOT)
            {
                Console.WriteLine("Oi Sou o ROOT! A multiplicação dos ranks é: " + mul);
            }

            comm_world.Barrier(); // TAREFAS VÃO AGUARDAR AQUI

        }


        public void allReduceMultiply()
        {

            double[] rms_work = { 1, 2, 3, 4 };

            var rms = new double[rms_work.Length];

            comm_world.Allreduce<double>(rms_work, Operation<double>.Multiply, ref rms);

            if (rank == ROOT) {

                foreach(double d in rms){

                    Console.WriteLine(d + " ");
                }
            }

            comm_world.Barrier(); // TAREFAS VÃO AGUARDAR AQUI

        }


        public void iSendReceive()
        {

            int tag = 0;

            //Request[] requests = new Request[2];

            RequestList bag = new RequestList();

            int origem = (rank - 1) < 0 ? size - 1 : rank - 1;

            int destino = (rank + 1) == size ? 0 : rank + 1;

            ulong[] matriz = { (ulong) rank, (ulong)rank, (ulong)rank, (ulong)rank };

            var tmp = new ulong[matriz.Length];

            //requests[0] = comm_world.ImmediateSend<ulong>(matriz, destino, tag);

            //requests[1] = comm_world.ImmediateReceive<ulong>(origem, tag, tmp);


            /*foreach(Request r in requests)
            {
                bag.Add(r);
            }*/

            //bag.WaitAll();

            bag.Add(comm_world.ImmediateSend<ulong>(matriz, destino, tag));

            bag.Add(comm_world.ImmediateReceive<ulong>(origem, tag, tmp));

            bag.WaitAll();


            string s = "\nNode: " + rank + " Recebeu: ";

            foreach (var a in tmp)
                s = s + a + " ";

            s = s + "\nNode: " + rank + " enviou: ";

            foreach (var a in matriz)
                s = s + a + " ";

            s = s + "\n";


            Console.WriteLine(s);

        }

    } 
}
