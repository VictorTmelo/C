#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

// Solução não perfeita, não cumpre a independencia entre tarefas, permite que uma Thread bloqueie a região critica mesmo sem estar utilizando.

int saldo = 0;
int vez = 1;

void entrar(int N){

    while(N != vez){}
}

void sair(){

    vez = vez%4 + 1; // Fila Circular (1 a 4)
}

void* depositar(void* x) {

    int n;

    //n = (int)*x;
    n = *((int*)x);

    for (int i = 0; i < 100000; i++) {

        //printf("Thread %d em execucao... \n", n);

        entrar(n);
        saldo = saldo + 1; // Região Critica
        sair();
    }

    return NULL;
}

int main()
{

    pthread_t tA,tB,tC,tD;

    int nA = 1;
    int nB = 2;
    int nC = 3;
    int nD = 4;

    pthread_create(&tA, NULL, depositar, &nA);
    pthread_create(&tB, NULL, depositar, &nB);
    pthread_create(&tC, NULL, depositar, &nC);
    pthread_create(&tD, NULL, depositar, &nD);

    pthread_join(tA, NULL);
    pthread_join(tB, NULL);
    pthread_join(tC, NULL);
    pthread_join(tD, NULL);

    printf("Saldo:  %d", saldo);

    return 0;
}
