using System;
using System.Collections.Generic;
using System.Text;

namespace AulaMPI2
{
    public class Util
    {
        public static ulong[][] multiply(ulong[][] A, ulong[][] B)
        {
            int a_rows = A.GetLength(0); // Linha A
            int b_cols = B[0].GetLength(0); // Coluna B
            int n = A[0].GetLength(0);// Coluna A, Linha B

            if (n != B.GetLength(0)) // Linha B!=n ?
                throw new Exception("A.columns != B.rows");

            ulong[][] C = new ulong[a_rows][];//, b_cols];
            for (int k = 0; k < a_rows; k++)
                C[k] = new ulong[b_cols];

            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < b_cols; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        C[i][j] = C[i][j] + A[i][k] * B[k][j];
                    }
                }
            }
            return C;
        }

        /*public static ulong[,] multiply(ulong[][] A, ulong[][] B)
        {
            //Console.WriteLine(A[0].GetLength(0));
            //Console.WriteLine(B.GetLength(1));


            ulong[,] a = new ulong[A.GetLength(0), A[0].GetLength(0)];
            ulong[,] b = new ulong[B.GetLength(0), B[0].GetLength(0)];
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaa");


            for (int i = 0; i < A.GetLength(0); i++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    a[i, j] = A[i][j];
                }
            }

            for (int i = 0; i < B.GetLength(0); i++)
            {
                for (int j = 0; j < B.GetLength(1); j++)
                {
                    b[i, j] = B[i][j];
                }
            }


            int a_rows = a.GetLength(0); // Linha A
            int b_cols = b.GetLength(1); // Coluna B
            int n = a.GetLength(1);// Coluna A, Linha B

            if (n != b.GetLength(0)) // Linha B!=n ?
                throw new Exception("A.columns != B.rows");

            ulong[,] C = new ulong[a_rows, b_cols];
            for (int i = 0; i < a_rows; i++)
            {
                for (int j = 0; j < b_cols; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        C[i, j] = C[i, j] + a[i, k] * b[k, j];
                    }
                }
            }
            return C;
        }*/

        public static void printMatriz(ulong[][] m)
        {
            Console.WriteLine("**************** " + m.Length + " x " + m[0].Length + " ****************");
            ulong maior = 0;
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m[0].Length; j++)
                {
                    if (m[i][j] > maior)
                        maior = m[i][j];
                }
            }
            for (int i = 0; i < m.Length; i++)
            {
                Console.Write(" ");
                for (int j = 0; j < m[0].Length; j++)
                {
                    Console.Write(leftZero(m[i][j], maior) + " ");
                }
                Console.WriteLine("");
            }
        }
        public static string leftZero(ulong n, ulong len)
        {
            string s = "" + n;
            while ((len + "").Length > s.Length)
                s = "0" + s;
            return s;
        }
        public static void insertionSort(ulong[] vetor)
        {

            for (int i = 1; i < vetor.Length; i++)
            {
                ulong key = vetor[i];
                int index = i - 1;
                for (int aux = index; index >= 0 && vetor[index] > key; index--)
                {
                    vetor[index + 1] = vetor[index];
                }

                vetor[index + 1] = key;
            }
        }
    }
}
