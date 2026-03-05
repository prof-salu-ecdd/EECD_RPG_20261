using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciadorBatalha : MonoBehaviour
{
    public void DispararBatalha(GameObject player, 
                                string idInimigo, 
                                List<GameObject> listaInimigos,
                                List<int> niveis)
    {
        //1. Salva a posição atual do jogador no mundo
        DadosGlobais.posicaoRetornoJogador = player.transform.position;

        //2. Salva a identidade e a formação dos inimigos
        DadosGlobais.idInimigoEmCombate = idInimigo;
        DadosGlobais.prefabsInimigos = new List<GameObject>(listaInimigos);
        DadosGlobais.niveisInimigosArena = new List<int>(niveis);

        //3. Carrega a arena de combate
        SceneManager.LoadScene("Arena");
    }
}
