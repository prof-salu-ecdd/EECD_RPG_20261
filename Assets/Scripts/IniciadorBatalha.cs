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
        if (listaInimigos == null || listaInimigos.Count == 0)
        {
            //Segurança
            return;
        }

        //1. Salva a posição atual do jogador no mundo
        DadosGlobais.posicaoRetornoJogador = player.transform.position;

        //2. Salva a identidade e a formação dos inimigos
        DadosGlobais.idInimigoEmCombate = idInimigo;
        DadosGlobais.prefabsInimigos = new List<GameObject>(listaInimigos);
        DadosGlobais.niveisInimigosArena = new List<int>(niveis);

        //Captura os dados do player antes de leva-lo para a arena
        IniciadorBatalha.SalvarDadosJogador(player);

        //3. Carrega a arena de combate
        SceneManager.LoadScene("Arena");
    }

    public static void SalvarDadosJogador(GameObject player)
    {
        if(player == null) {  return; }

        AtributosCombate atributos = player.GetComponent<AtributosCombate>();
        ProgressoJogador progresso = player.GetComponent<ProgressoJogador>();
        SistemaInventario inventario = player.GetComponent<SistemaInventario>();

        if(atributos != null)
        {
            DadosGlobais.hpAtualJogador = atributos.hpAtual;
            DadosGlobais.nivelAtualJogador = atributos.nivel;
            DadosGlobais.bonusAtaque = atributos.bonusAtaque;
            DadosGlobais.bonusDefesa = atributos.bonusDefesa;
        }

        if(inventario != null)
        {
            DadosGlobais.moedasAtualJogador = inventario.moedas;
        }

        if(progresso != null)
        {
            DadosGlobais.xpAtualJogador = progresso.xpAtual;
        }

        Debug.Log("Dados salvos com sucesso!");
    }
    public static void CarregarDadosJogador(GameObject player)
    {
        if(player == null && DadosGlobais.hpAtualJogador == -1) { return; }

        AtributosCombate atributos = player.GetComponent<AtributosCombate>();
        ProgressoJogador progresso = player.GetComponent<ProgressoJogador>();

        if (atributos != null) 
        {
            atributos.nivel = DadosGlobais.nivelAtualJogador;
            atributos.bonusAtaque = DadosGlobais.bonusAtaque;
            atributos.bonusDefesa = DadosGlobais.bonusDefesa;
            atributos.CalcularStatus();
            //Calcular os STATUS antes de definir o HP ATUAL
            atributos.hpAtual = DadosGlobais.hpAtualJogador;
            atributos.AtualizarBarra();
        }

        if (progresso != null)
        {
            progresso.xpAtual = DadosGlobais.xpAtualJogador;
        }

        Debug.Log("Dados carregados com Sucesso!");

    }
}
