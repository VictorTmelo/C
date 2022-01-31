#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int saldo = 0;

pthread_mutex_t semaforo;

void entrar(){
    pthread_mutex_lock(&semaforo);
}

void sair(){
    pthread_mutex_unlock(&semaforo);
}

void* depositar(void* x) {

    int n;

    //n = (int)*x;
    n = *((int*)x);

    for (int i = 0; i < 100000; i++) {

        printf("Thread %d em execucao... \n", n);

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
