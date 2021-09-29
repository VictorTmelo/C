#include <unistd.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

void* funcaoA() {

    for (int i = 0; i < 10; i++) {

        printf("Processo 1... Thread A... %d/10\n", i + 1);

        sleep(1);
    }

    return NULL;
}

void* funcaoB() {

    for (int i = 0; i < 10; i++) {

        printf("Processo 1... Thread B... %d/10\n", i + 1);

        sleep(1);
    }

    return NULL;
}

int main()
{

    pthread_t tA;
    pthread_t tB;

    pthread_create(&tA, NULL, funcaoA, NULL);
    pthread_create(&tB, NULL, funcaoB, NULL);

    pthread_join(tA, NULL);
    pthread_join(tB, NULL);

    return 0;
}
