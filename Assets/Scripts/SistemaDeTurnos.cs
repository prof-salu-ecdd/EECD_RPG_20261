using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoBatalha
{
    Preparacao, TurnoJogador, TurnoInimigo,
    Vitoria, Derrota
}
public class SistemaDeTurnos : MonoBehaviour
{
    public EstadoBatalha estadoAtual;

    AtributosCombate atributosHeroi;
    AtributosCombate atributosInimigo;//Por enquanto apenas 1 inimigo

    private void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;

        StartCoroutine(ConfigurarBatalha());
    }

    IEnumerator ConfigurarBatalha()
    {
        Debug.Log("Preparando a batalha...");

        yield return new WaitForSeconds(1f);

        //Encontra os personagens dentro da arena utilizando as tags
        atributosHeroi = GameObject.FindGameObjectWithTag("Player").
                                                GetComponent<AtributosCombate>();
        atributosInimigo = GameObject.FindGameObjectWithTag("Inimigo").
                                                GetComponent<AtributosCombate>();

        estadoAtual = EstadoBatalha.TurnoJogador;

        IniciarTurnoJogador();
    }

    private void IniciarTurnoJogador()
    {
        Debug.Log("Sua vez herói. Pressione ESPAÇO para ATACAR!");
    }

    private void Update()
    {
        switch (estadoAtual)
        {
            case EstadoBatalha.TurnoJogador:
                //Ataque basico (ESPAÇO)
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Herói atacou o inimigo.");
                    atributosInimigo.ReceberDano(atributosHeroi.danoBase);

                    VerificaFimDeTurnoJogador();
                }
                break;
        }
    }

    void VerificaFimDeTurnoJogador()
    {
        //Verifica se o combate se encerrou
        if (atributosInimigo.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Vitoria;
            //Finalizar a batalha(vitoria do jogador)
            StartCoroutine(FinalizarBatalha(true));
        }
        else
        {
            estadoAtual = EstadoBatalha.TurnoInimigo;
            //Iniciar turno do inimigo
            StartCoroutine(TurnoDoInimigo());
        }
    }

    IEnumerator TurnoDoInimigo()
    {
        Debug.Log("Inimigo pensando ...");
        yield return new WaitForSeconds(2f);
        //Ataca Jogador
        Debug.Log("Inimigo ataca o jogador");
        atributosHeroi.ReceberDano(atributosInimigo.danoBase);

        //Verifica se o combate encerrou
        if (atributosHeroi.hpAtual <= 0)
        {
            estadoAtual = EstadoBatalha.Derrota;
            //Finalizar a batalha   
            StartCoroutine (FinalizarBatalha(false));
        }
        else
        {
            estadoAtual = EstadoBatalha.TurnoJogador;
            IniciarTurnoJogador();
        }
    }

    IEnumerator FinalizarBatalha(bool jogadorVenceu)
    {
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
