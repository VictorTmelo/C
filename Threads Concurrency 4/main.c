#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

// Solução não perfeita, pois não é uma solucão universal, pois possui requisitos de arquitetura.

int saldo = 0;
int lock = 0;

void entrar(){

    int key = 1;

    while(key == 1){

        // swap(lock, key); Tranca a Porta
        asm("xchg %0, %1" : "+q" (key), "+m" (lock)); // Troca os valores de key e swap. Atraves de instrução assembly.
    }
}

void sair(){
    lock = 0; // Destranca a Porta
}

void* depositar(void* x) {

    int n;

    //n = (int)*x;
    n = *((int*)x);

    for (int i = 0; i < 100000; i++) {

        //printf("Thread %d em execucao... \n", n);

        entrar();
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
