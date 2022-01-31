#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <unistd.h>

pthread_mutex_t H[5];

void* filosofo(void* ptr){

    int n = *((int*)ptr);
    int i1,i2;

    while(1){

        // PENSAR
        printf("Filosofo %d pensando...\n", n);

        //i1 = n;
        ///i2 = (n+1)%5;

        // TENTAR PEGAR O PRIMEIRO HASHI, E SE TIVER DISPONIVEL PEGAR E TORNA-LO INDISPONIVEL PARA OS OUTROS.
        pthread_mutex_lock(&H[n]);
        // TENTAR PEGAR O SEGUNDO HASHI, E SE TIVER DISPONIVEL PEGAR E TORNA-LO INDISPONIVEL PARA OS OUTROS.
        pthread_mutex_lock(&H[(n+1)%5]);

        // COMER
        printf("Filosofo %d comendo...\n", n);
        //sleep(1);

        // LIBERA OS HASHIS
        pthread_mutex_unlock(&H[(n+1)%5]);
        pthread_mutex_unlock(&H[n]);
    }

    return NULL;

}

int main()
{
    pthread_t f0,f1,f2,f3,f4;
    int n0 = 0, n1 = 1, n2 = 2, n3 = 3, n4 = 4;

    pthread_create(&f0, NULL, filosofo, &n0);
    pthread_create(&f1, NULL, filosofo, &n1);
    pthread_create(&f2, NULL, filosofo, &n2);
    pthread_create(&f3, NULL, filosofo, &n3);
    pthread_create(&f4, NULL, filosofo, &n4);

    pthread_join(f0, NULL);
    pthread_join(f1, NULL);
    pthread_join(f2, NULL);
    pthread_join(f3, NULL);
    pthread_join(f4, NULL);

    return 0;
}
