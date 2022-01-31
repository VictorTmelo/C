#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>
#include <unistd.h>

sem_t vagos;
sem_t preenchidos;
pthread_mutex_t rc;
int buffer[10];
int in = 0;
int out = 0;

void* produtor (void* n)
{

    int* p;
    int nProd;
    p = (int *)n;
    nProd = *p;
    int item;

    for(int i=0;i<10;i++)
    {
        item = rand();
        sem_wait(&vagos);
        pthread_mutex_lock(&rc);
        buffer[in] = item;
        printf("Produtor %d: insere item %d em %d\n",nProd,item,in);
        sleep(1);
        in = (in + 1)%10;
        pthread_mutex_unlock(&rc);
        sem_post(&preenchidos);
    }
    return NULL;
}


void* consumidor (void* n)
{

    int* p;
    int nCons;
    p = (int *)n;
    nCons = *p;
    int item;

    for(int i=0;i<10;i++)
    {
        sem_wait(&preenchidos);
        pthread_mutex_lock(&rc);
        item = buffer[out];
        printf("Consumidor %d: retira item %d de %d\n",nCons,item,out);
        sleep(1);
        out = (out + 1)%10;
        pthread_mutex_unlock(&rc);
        sem_post(&vagos);

    }

    return NULL;
}



int main()
{
    sem_init(&vagos,0,10);
    sem_init(&preenchidos,0,0);

    pthread_t p1, p2, p3, p4, p5;
    pthread_t c1, c2, c3, c4, c5;

    int np1 = 1;
    int np2 = 2;
    int np3 = 3;
    int np4 = 4;
    int np5 = 5;

    int nc1 = 1;
    int nc2 = 2;
    int nc3 = 3;
    int nc4 = 4;
    int nc5 = 5;

    pthread_create(&p1,NULL,produtor,&np1);
    pthread_create(&p2,NULL,produtor,&np2);
    pthread_create(&p3,NULL,produtor,&np3);
    pthread_create(&p4,NULL,produtor,&np4);
    pthread_create(&p5,NULL,produtor,&np5);

    pthread_create(&c1,NULL,consumidor,&nc1);
    pthread_create(&c2,NULL,consumidor,&nc2);
    pthread_create(&c3,NULL,consumidor,&nc3);
    pthread_create(&c4,NULL,consumidor,&nc4);
    pthread_create(&c5,NULL,consumidor,&nc5);

    pthread_join(p1,NULL);
    pthread_join(p2,NULL);
    pthread_join(p3,NULL);
    pthread_join(p4,NULL);
    pthread_join(p5,NULL);

    pthread_join(c1,NULL);
    pthread_join(c2,NULL);
    pthread_join(c3,NULL);
    pthread_join(c4,NULL);
    pthread_join(c5,NULL);

    return 0;
}
