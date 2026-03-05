using System.Collections.Generic;
using UnityEngine;

public static class DadosGlobais
{
    // Dados dos inimigos
    public static List<GameObject> prefabsInimigos = new List<GameObject>();
    public static string idInimigoEmCombate;
    public static List<string> inimigosDerrotados = new List<string>();
    public static List<int>niveisInimigosArena = new List<int>();//Armazena o lvls dos inimigos

    //Dados salvos do player
    public static Vector2 posicaoRetornoJogador = Vector2.zero;
    public static int hpAtualJogador = -1;
    public static int nivelAtualJogador = 1;
    public static int xpAtualJogador = 0;
    public static int moedasAtualJogador = 0;
}
