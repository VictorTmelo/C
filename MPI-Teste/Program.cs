using System;

namespace MPI_Teste
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MPITest app = new MPITest();

            app.mpi_start();

            // PROGRAMA
            //app.reduceAdd();

            //app.reduceMultiply();

            app.allReduceMultiply();

            //app.iSendReceive();

            app.mpi_stop();

        }
    }
}
