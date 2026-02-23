using System.Collections.Generic;
using UnityEngine;

public class GerenciadorBatalha : MonoBehaviour
{

    [Header("Posições")]
    public Transform posicaoJogador;
    //Agora precisamos de uma lista com todas as posições dos inimigos
    public List<Transform> posicaoInimigos = new List<Transform>();

    [Header("Prefabs")]
    public GameObject prefabJogador;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Posiciona o jogador
        GameObject jogador = Instantiate(prefabJogador, posicaoJogador.position, Quaternion.identity);

        //1. Desativar a movimentação do jogador
        if(jogador.GetComponent<MovimentacaoExploracao>() != null)
        {
            jogador.GetComponent<MovimentacaoExploracao>().enabled = false;
        }

        //Verifica qual é a lista de inimigos
        List<GameObject> grupoPrefabs = DadosGlobais.prefabsInimigos;

        for (int i = 0; i < grupoPrefabs.Count; i++)
        {
            Instantiate(grupoPrefabs[i], 
                        posicaoInimigos[i].transform.position, 
                        Quaternion.identity);
        }       
    }
}
