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
        //1. Inicializa o jogador o jogador
        GameObject jogador = Instantiate(prefabJogador, posicaoJogador.position, Quaternion.identity);

        AtributosCombate atributosJogador =  jogador.GetComponent<AtributosCombate>();
        atributosJogador.nivel = DadosGlobais.nivelAtualJogador;
        atributosJogador.CalcularStatus();

        //Desliga a movimentação do player
        if(jogador.GetComponent<MovimentacaoExploracao>() != null)
        {
            jogador.GetComponent<MovimentacaoExploracao>().enabled = false;
        }

        //2. Inicializa os inimigos
        List<GameObject> grupoPrefabs = DadosGlobais.prefabsInimigos;

        for (int i = 0; i < grupoPrefabs.Count; i++)
        {
            GameObject inimigo = Instantiate(grupoPrefabs[i], 
                                            posicaoInimigos[i].transform.position, 
                                            Quaternion.identity);

            //Pega os dados da memoria global e atribui aos inimigos
            AtributosCombate atributos = inimigo.GetComponent<AtributosCombate>();

            if(i < DadosGlobais.niveisInimigosArena.Count)
            {
                atributos.nivel = DadosGlobais.niveisInimigosArena[i];
                atributos.CalcularStatus();
                atributos.hpAtual = atributos.hpMaximo;
            }
        }
        
         
    }
}
