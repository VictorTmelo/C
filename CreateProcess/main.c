#include <stdio.h>
#include <stdlib.h>
#include <processthreadsapi.h>
#include <windows.h>

int main()
{

    //Processo 1

    STARTUPINFOA si;
    PROCESS_INFORMATION pi;

    ZeroMemory(&si, sizeof(si));
    ZeroMemory(&pi, sizeof(pi));

    int x;

    x = CreateProcess("C:\Program Files\Sublime Text 3\sublime_text.exe", NULL, NULL, NULL, FALSE, NORMAL_PRIORITY_CLASS, NULL, NULL, &si, &pi);

    //Processo 2

    /*STARTUPINFOA si;
    PROCESS_INFORMATION pi;

    ZeroMemory(&si, sizeof(si));
    ZeroMemory(&pi, sizeof(pi));

    int x;

    x = CreateProcessA("C:\Users\victo\CodeBlocks Projects\CreateThread 2\bin\Debug\CreateThread 2.exe", NULL, NULL, NULL, FALSE, NORMAL_PRIORITY_CLASS, NULL, NULL, &si, &pi);*/

    return 0;

}
