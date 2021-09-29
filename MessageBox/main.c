#include <stdio.h>
#include <stdlib.h>
#include <Windows.h>

int main()
{
    int x;
    x = MessageBox(NULL, (LPCWSTR)L"Claro ne?", (LPCWSTR)L"Sao Paulo?", MB_ICONQUESTION | MB_YESNO);

    if (x == IDYES) {
        printf("Voce respondeu que sim!");
    }
    else {
        printf("Voce respondeu que não!");
    }

    return 0;

}
