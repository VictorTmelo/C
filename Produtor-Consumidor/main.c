#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pthread.h>
#include <semaphore.h>

int buffer[10] = {0,0,0,0,0,0,0,0,0,0};

int in = 0;
int out = 0;

sem_t vagos;
sem_t preenchidos;

pthread_mutex_t slot;

void* produtor(void* ptr){

    int n = *((int*)ptr);
    int item;

    for(int i = 0; i < 10; i++){

        item = rand();

        sem_wait(&vagos);

        pthread_mutex_lock(&slot);

        buffer[in] = item;

        in = (in+1)%10;

        printf("Produtor %d inseriu um item no slot %d...\n", n, in);

        sleep(1);

        pthread_mutex_unlock(&slot);

        sem_post(&preenchidos);
    }

    return NULL;
}


void* consumidor(void* ptr){

    int n = *((int*)ptr);
    int x;

    for(int i = 0; i < 10; i++){

        sem_wait(&preenchidos);

        pthread_mutex_lock(&slot);

        x = buffer[out];

        out = (out+1)%10;

        printf("Consumidor %d retirou um item no slot %d...\n", n, out);

        sleep(1);

        pthread_mutex_unlock(&slot);

        sem_post(&vagos);
    }

    return NULL;
}


int main()
{
    sem_init(&vagos,0,10);
    sem_init(&preenchidos,0,0);
    pthread_mutex_unlock(&slot);

    pthread_t p1,p2,p3,p4,p5;
    pthread_t c1,c2,c3,c4,c5;

    int np1 = 1, np2 = 2, np3 = 3, np4 = 4, np5 = 5;
    int nc1 = 1, nc2 = 2, nc3 = 3, nc4 = 4, nc5 = 5;

    pthread_create(&p1, NULL, produtor, &np1);
    pthread_create(&p2, NULL, produtor, &np2);
    pthread_create(&p3, NULL, produtor, &np3);
    pthread_create(&p4, NULL, produtor, &np4);
    pthread_create(&p5, NULL, produtor, &np5);

    pthread_create(&c1, NULL, consumidor, &nc1);
    pthread_create(&c2, NULL, consumidor, &nc2);
    pthread_create(&c3, NULL, consumidor, &nc3);
    pthread_create(&c4, NULL, consumidor, &nc4);
    pthread_create(&c5, NULL, consumidor, &nc5);

    pthread_join(p1, NULL);
    pthread_join(p2, NULL);
    pthread_join(p3, NULL);
    pthread_join(p4, NULL);
    pthread_join(p5, NULL);

    pthread_join(c1,NULL);
    pthread_join(c2,NULL);
    pthread_join(c3,NULL);
    pthread_join(c4,NULL);
    pthread_join(c5,NULL);

    return 0;
}
