#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <semaphore.h>

int saldo = 0;

sem_t semaforo;

void entrar(){
    sem_wait(&semaforo);
}

void sair(){
    sem_post(&semaforo);
}

void* depositar(void* x) {

    int n;

    //n = (int)*x;
    n = *((int*)x);

    entrar();

    for (int i = 0; i < 100000; i++) {

        printf("Thread %d em execucao... \n", n);

        saldo = saldo + 1; // Região Critica

    }

    sair();

    return NULL;
}

int main()
{

    sem_init(&semaforo,0,1);

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
