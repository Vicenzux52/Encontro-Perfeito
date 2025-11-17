using UnityEngine;

public static class DeathSaver
{
    public static Vector3 deathPosition;
    public static string deathScene;

    public static Vector3 lastDeathPosition;
    public static bool hasSavedPosition = false;
    public static string returnScene;
    public static bool estereggConcluido = false;
    public static bool estereggNaoConcluido = false;
    public static bool emEasterEgg = false;
}