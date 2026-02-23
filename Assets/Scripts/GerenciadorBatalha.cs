using System.Collections.Generic;
using UnityEngine;

public class GerenciadorBatalha : MonoBehaviour
{

    [Header("Posições")]
    public Transform posicaoJogador;
    public Transform posicaoInimigo;

    [Header("Prefabs")]
    public GameObject prefabJogador;
    public GameObject prefabInimigo1;
    public GameObject prefabInimigo2;

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
        
        foreach(GameObject inimigo in grupoPrefabs)
        {
            Instantiate(inimigo, posicaoInimigo.position, Quaternion.identity);
        }       
    }
}
