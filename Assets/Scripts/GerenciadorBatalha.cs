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


        //Verifica qual é o inimigo
        string inimigo = DadosGlobais.inimigoParaGerar;
        GameObject monstroCriado = null;

        if (inimigo == "Topeira")
        {
            monstroCriado = Instantiate(prefabInimigo1, posicaoInimigo.position, Quaternion.identity);
        }
        else if (inimigo == "TopeiraEvil")
        {
            monstroCriado = Instantiate(prefabInimigo2, posicaoInimigo.position, Quaternion.identity);
        }

        //2. Desativar a movimentação do inimigo
        if(monstroCriado != null)
        {
            monstroCriado.GetComponent<ControladorInimigo>().enabled = false;
        }
    }
}
