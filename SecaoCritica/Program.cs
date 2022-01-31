using System;
using System.Threading;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;


namespace SecaoCritica {
    class Program {

        public static volatile int n = 0; // diz ao compilador para evitar otimizações
        public static int MAX = 1000;

        public static List<Thread> threads = new List<Thread>();

        static void Main(string[] args)
        {

            Console.WriteLine("*********************************************");

            Thread p = new Thread(process);
            Thread q = new Thread(process);
            Thread t = new Thread(process);
            Thread a = new Thread(process);
            Thread b = new Thread(process);

            threads.Add(p);
            threads.Add(q);
            threads.Add(t);
            threads.Add(a);
            threads.Add(b);

            p.Start(1);
            t.Start(2);
            q.Start(3);
            a.Start(4);
            b.Start(5);

            p.Join();
            q.Join();
            t.Join();
            a.Join();
            b.Join();

            Console.WriteLine("n: " + n); // saida: 5000
        }
        public static int wait = 1;
        public static void process(object o){

            int id = (int)o;

            int valor = threads.Count;

            for (int i = 1; i <= MAX; i++)
            {
                while (wait != id); //pre-protocolo

                    int temp = n; // secao critica
                    n = n + 1;    // secao critica

                     wait = id + 1; // pos-protocolo

                    if (id == valor) wait = 1; // pos-protocolo

                    /* OU

                    int x = id % valor;

                    if (x == 0) wait = 1; */

                Thread.Sleep(10);
                
            }
            Console.WriteLine("id={0}", o);
        }
    }
}
