using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EstadoBatalha
{
    Preparacao, TurnoJogador, TurnoInimigo,
    Vitoria, Derrota
}
public class SistemaDeTurnos : MonoBehaviour
{
    public EstadoBatalha estadoAtual;

    public Slider sliderHeroi;

    private AtributosCombate atributosHeroi;
    
    private List<AtributosCombate> inimigosVivos = new List<AtributosCombate>();

    private void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;

        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {        

        //1. Configurando o heroi
        atributosHeroi = GameObject.FindGameObjectWithTag("Player").
                                                GetComponent<AtributosCombate>();
        atributosHeroi.minhaBarraDeVida = sliderHeroi;
        atributosHeroi.AtualizarBarra();

        Debug.Log("Preparando a batalha...");
        yield return new WaitForSeconds(1f);

        //2. Configurando os inimigos
        //Preenche a lista de inimigos
        GameObject[] objsInimigos = GameObject.FindGameObjectsWithTag("Inimigo");
        foreach (GameObject obj in objsInimigos)
        {
            inimigosVivos.Add(obj.GetComponent<AtributosCombate>());
        }
        
        IniciarTurnoJogador();
    }

    private void IniciarTurnoJogador()
    {
        Debug.Log("Sua vez herói. Pressione ESPAÇO para ATACAR!");
        estadoAtual = EstadoBatalha.TurnoJogador;
    }

    public void BotaoAtacar()
    {
        //ignora caso não seja a vez do jogador
        if(estadoAtual != EstadoBatalha.TurnoJogador)
        {
            return;
        }

        //Define o alvo do ataque. Sempre o primeiro elemento da lista
        AtributosCombate alvo = inimigosVivos[0];
        alvo.ReceberDano(atributosHeroi.danoBase);

        //Se a vida do inimigo chegou a zero, removemos ele de lista
        if(alvo.hpAtual <= 0)
        {
            inimigosVivos.RemoveAt(0);
        }

        VerificaFimDeTurnoJogador();
    }

    public void BotaoPocao()
    {
        //ignora caso não seja a vez do jogador
        if (estadoAtual != EstadoBatalha.TurnoJogador)
        {
            return;
        }

        //Chama a função de cura
        atributosHeroi.ReceberCura(30);

        VerificaFimDeTurnoJogador();
    }

    void VerificaFimDeTurnoJogador()
    {
        //Se a fila de inimigos ficou VAZIA
        if(inimigosVivos.Count == 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            //Ainda há inimigos vivos
            estadoAtual = EstadoBatalha.TurnoInimigo;
            StartCoroutine(TurnoDoInimigo());
        }
    }

    IEnumerator TurnoDoInimigo()
    {
        Debug.Log("Inimigo pensando ...");
        //Inimigos Atacam o Jogador
        Debug.Log("Inimigo ataca o jogador");
        
        foreach(AtributosCombate inimigo in inimigosVivos)
        {
            yield return new WaitForSeconds(1f);
            atributosHeroi.ReceberDano(inimigo.danoBase);

            //Verifica se o heroi morreu, para o ataque do proximo inimigo
            if(atributosHeroi.hpAtual <= 0)
            {
                break;
            }
        }

        //Verifica se o combate encerrou
        if (atributosHeroi.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Derrota;
            //Finalizar a batalha   
            StartCoroutine (FinalizarBatalha(false));
        }
        else
        {
            IniciarTurnoJogador();
        }
    }

    IEnumerator FinalizarBatalha(bool jogadorVenceu)
    {
        DadosGlobais.hpAtualJogador = atributosHeroi.hpAtual;
        yield return new WaitForSeconds(2f);
        if (jogadorVenceu)
        {
            Debug.Log("Vitória! Voltando para o mundo de exploração.");
            //Adiciona o id do inimigo ao cemiterio (lista inimigosDerrotados)
            DadosGlobais.inimigosDerrotados.Add(DadosGlobais.idInimigoEmCombate);
            
            SceneManager.LoadScene("Mundo");
        }
        else
        {
            Debug.Log("Derrota...");
            SceneManager.LoadScene("GameOver");
        }
    }
}
