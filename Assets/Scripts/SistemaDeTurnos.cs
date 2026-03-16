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

    [Header("Integração com o inventário")]
    public DadosItem pocaoDevida;
    public Button btnPocao;

    private void Start()
    {
        estadoAtual = EstadoBatalha.Preparacao;

        StartCoroutine(ConfigurarBatalha());        
    }

    IEnumerator ConfigurarBatalha()
    {
        //DELAY [sobreescrevendo o valor]
        yield return new WaitForSeconds(1f);
        //1. Configurando o heroi
        atributosHeroi = GameObject.FindGameObjectWithTag("Player").
                                                GetComponent<AtributosCombate>();
        atributosHeroi.minhaBarraDeVida = sliderHeroi;

        atributosHeroi.hpMaximo += DadosGlobais.bonusDefesa;
        atributosHeroi.hpAtual = atributosHeroi.hpMaximo;

        atributosHeroi.AtualizarBarra();

        atributosHeroi.danoAtual += DadosGlobais.bonusAtaque;

        //Verifica se tem poções no inventario
        if (!GetComponent<SistemaInventario>().TemItem(pocaoDevida, 1))
        {
            btnPocao.interactable = false;
        }
            
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
        Debug.Log("Sua vez herói. Escolha uma ação.");
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
        alvo.ReceberDano(atributosHeroi.danoAtual);

        //Se a vida do inimigo chegou a zero, removemos ele de lista
        if(alvo.hpAtual <= 0)
        {
            //1. Busca os scripts
            RecompensaInimigo loot = alvo.GetComponent<RecompensaInimigo>();
            ProgressoJogador progresso = atributosHeroi.GetComponent<ProgressoJogador>();

            //2. Transfere as recompensas
            progresso.GanharXP(loot.xpDrop);
            DadosGlobais.moedasAtualJogador += loot.moedasDrop;

            Debug.Log($"Você encontrou {loot.moedasDrop} moedas!");

            //3. Grava os dados na memoria global
            DadosGlobais.xpAtualJogador = progresso.xpAtual;
            DadosGlobais.nivelAtualJogador = atributosHeroi.nivel;

            inimigosVivos.RemoveAt(0);

            if(loot != null && progresso != null)
            {
                //Rastreador de missão
                if (DadosGlobais.QuestAtiva != null)
                {
                    if (DadosGlobais.QuestAtiva.tipoDeMissao == TipoQuest.CacarMonstros
                        || DadosGlobais.QuestAtiva.tipoDeMissao == TipoQuest.ColetarItens)
                    {
                        DadosGlobais.progressoAtual++;
                        Debug.Log($"Quest atualizado no console: {DadosGlobais.progressoAtual}" +
                            $" / {DadosGlobais.QuestAtiva.quantidade}");
                    }
                }
            }           
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

        bool consumiuApenasUma = false;

        //1. Procura o item(poção) no inventário global
        foreach(SlotInventario slot in DadosGlobais.inventarioAtualJogador)
        {
            //Se tenho a poção no inventario e quantidade superior a ZERO
            if(slot.dadosDoItem == pocaoDevida && slot.quantidade > 0)
            {
                slot.quantidade--;
                consumiuApenasUma = true;

                //Remove da lista se a quantidade for zero
                if(slot.quantidade <= 0)
                {
                    DadosGlobais.inventarioAtualJogador.Remove(slot);
                    //Desabilita Botao
                    btnPocao.interactable = false;
                }
                break;
            }
        }

        if (consumiuApenasUma)
        {
            atributosHeroi.ReceberCura(50);
            Debug.LogWarning("Você bebeu a poção. Restarou 50 pontos de vida.");
            VerificaFimDeTurnoJogador();
        }
        else
        {
            Debug.LogWarning("Você não tem poções de vida!");
        }
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
        ProgressoJogador progresso = atributosHeroi.GetComponent<ProgressoJogador>();

        //1. Salva a vida e o XP
        DadosGlobais.hpAtualJogador = atributosHeroi.hpAtual;
        DadosGlobais.nivelAtualJogador = atributosHeroi.nivel;
        DadosGlobais.xpAtualJogador = progresso.xpAtual;
        

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
