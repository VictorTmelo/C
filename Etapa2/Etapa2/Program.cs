using System;
using System.Diagnostics;

namespace Etapa2 {
    public class Program {
        // GENERATOR:
        public static int N = 4; // Nro de matrizes para multiplicar
        public static int MIN = 50; // valor minimo dos dados de cada matriz
        public static int MAX = 100; // valor maximo dos dados de cada matriz

        public static string file = "D:\\workspace\\CSharp\\Parallel\\AulaMapReduce2\\MapReducer\\matrix"+N+".txt";

        public static void Main(string[] args) {
            var lc = Generator.Gen(file, N, MIN, MAX);

            var ms = Generator.MatrixList(file);

            var time = new Stopwatch();
            time.Start();
            var c = ms[0];
            for (int k = 1; k < N; k++) {
                c = multiplication(c, ms[k]);
                if (k % 100 == 0) Console.WriteLine("Matriz: " + k + " de " + N);
            }

            time.Stop();

            ulong check_sum = 0;
            for (int i = 0; i < c.GetLength(0); i++)
                for (int j = 0; j < c.GetLength(1); j++)
                    check_sum += c[i, j];

            Console.WriteLine($"Tempo de execucao: {time.ElapsedMilliseconds} ms \ncheck_sum: " + check_sum);
        }

        public static ulong[,] multiplication(ulong[,] A, ulong[,] B) {
            int a_rows = A.GetLength(0); //Linha A
            int b_cols = B.GetLength(1); //Coluna B
            int n = A.GetLength(1);//Coluna A, Linha B

            if (n != B.GetLength(0)) { // Linha B!=n?
                throw new Exception("A.columns != B.rows");
            }

            ulong[,] C = new ulong[a_rows, b_cols];
            for (int i = 0; i < a_rows; i++) {
                for (int j = 0; j < b_cols; j++) {
                    for (int k = 0; k < n; k++) {
                        C[i, j] = C[i, j] + A[i, k] * B[k, j];
                    }
                }
            }
            return C;
        }
        public static void printMatriz(ulong[,] m) {
            Console.WriteLine("**************** " + m.GetLength(0) + " x " + m.GetLength(1) + " ****************");
            ulong maior = 0;
            for (int i = 0; i < m.GetLength(0); i++) {
                for (int j = 0; j < m.GetLength(1); j++) {
                    if (m[i, j] > maior)
                        maior = m[i, j];
                }
            }
            for (int i = 0; i < m.GetLength(0); i++) {
                Console.Write(" ");
                for (int j = 0; j < m.GetLength(1); j++) {
                    Console.Write(leftZero(m[i, j], maior) + " ");
                }
                Console.WriteLine("");
            }
        }
        public static string leftZero(ulong n, ulong len) {
            string s = "" + n;
            while ((len + "").Length > s.Length) s = "0" + s;
            return s;
        }
    }
}
