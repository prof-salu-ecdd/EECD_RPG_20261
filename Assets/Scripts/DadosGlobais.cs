using System.Collections.Generic;
using UnityEngine;

public static class DadosGlobais
{
    // Variaveis globais/static de combate
    public static List<GameObject> prefabsInimigos = new List<GameObject>();
    public static string idInimigoEmCombate;
    public static List<string> inimigosDerrotados = new List<string>();
    public static Vector2 posicaoRetornoJogador = Vector2.zero;
}
