#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int saldo = 0;
int busy = 0;

void entrar(){

    while(busy == 1){} // Batendo na Porta

    // Preempsão ocorre aqui

    busy = 1; // Tranca a Porta
}

void sair(){
    busy = 0; // Destranca a Porta
}

void* depositar(void* x) {

    int n;

    //n = (int)*x;
    n = *((int*)x);

    for (int i = 0; i < 10000; i++) {

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
