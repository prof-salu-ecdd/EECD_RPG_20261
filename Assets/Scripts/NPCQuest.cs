using TMPro;
using UnityEngine;

public class NPCQuest : MonoBehaviour
{
    [Header("Identidade")]
    public string nome;

    [Header("Apenas para o primeiro NPC do Jogo")]
    public Quest questInicial;

    [Header("Interface de dialogo")]
    public GameObject painelDialogo;
    public TextMeshProUGUI textoDiaologo;

    [Header("Visuais")]
    public SpriteRenderer indicadorVisual;//AZUL [inicio] | LARANJA [destino]

    private bool jogadorPerto = false;
    private GameObject jogadorRef;

    private void Start()
    {
        if(questInicial != null && DadosGlobais.questDisponivel == null 
            && DadosGlobais.QuestAtiva == null && DadosGlobais.historiaConcluida == false)
        {
            DadosGlobais.questDisponivel = questInicial;
        }
    }

    private void Update()
    {
        AtualizarIconeVisual();
        if(jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            Interagir();
        }
    }

    private void AtualizarIconeVisual()
    {
        if (indicadorVisual == null) return;

        indicadorVisual.gameObject.SetActive(false);

        if (DadosGlobais.historiaConcluida) return;

        //AZUL (nova quest)
        if(DadosGlobais.QuestAtiva == null && DadosGlobais.questDisponivel != null
            && DadosGlobais.questDisponivel.nomeNPCEmissor == nome)
        {
            indicadorVisual.color = Color.blue;
            indicadorVisual.gameObject.SetActive(true);
        }else if(DadosGlobais.QuestAtiva != null 
            && DadosGlobais.QuestAtiva.nomeNPCDestino == nome) //LARANJA (entrega)
        {
            indicadorVisual.color = Color.orange;
            indicadorVisual.gameObject.SetActive(true);
        }
    }

    public void Interagir() 
    {
        //Liga a caixa de dialogo (painel)
        if(painelDialogo != null)
        {
            painelDialogo.SetActive(true);
        }

        if(textoDiaologo == null)
        {
            return;
        }

        if (DadosGlobais.historiaConcluida)
        {
            textoDiaologo.text = ($"{nome} diz: A paz reina na nossa floresta graças a você!");
            return;
        }

        //Cenario 1: Temos uma missão
        if(DadosGlobais.QuestAtiva != null)
        {
            Quest quest = DadosGlobais.QuestAtiva;

            if(quest.nomeNPCDestino == nome)
            {
                //Verifica o tipo de quest
                bool terminouCaca = (quest.tipoDeMissao == TipoQuest.CacarMonstros
                    && DadosGlobais.progressoAtual >= quest.quantidade);

                bool terminouColeta = (quest.tipoDeMissao == TipoQuest.ColetarItens
                    && DadosGlobais.progressoAtual >= quest.quantidade);

                bool terminouEntrega = (quest.tipoDeMissao == TipoQuest.EncontrarNPC);

                if( terminouCaca || terminouColeta || terminouEntrega)
                {
                    textoDiaologo.text = ($"{nome} diz: {quest.falaConclusao} " +
                        $"(Recebeu {quest.recompensaOuro}) Ouro");
                    //Entregar recompensa
                    EntregarRecompensa(quest);
                }
                else
                {
                    textoDiaologo.text = ($"{nome} diz: {quest.falaAndamento} Progresso: " +
                        $"{DadosGlobais.progressoAtual} / {quest.quantidade} " +
                        $"{quest.nomeObjetivo}");
                }

            }
            else
            {
                textoDiaologo.text = ($"{nome} diz: O {quest.nomeNPCDestino} está a sua espera." +
                        $"Não perca tempo aqui!");
            }
            return;
        }

        //Cenario 2: Nova missao no mundo
        if(DadosGlobais.questDisponivel != null )
        {
            if(DadosGlobais.questDisponivel.nomeNPCEmissor == nome)
            {
                textoDiaologo.text = ($"{nome} diz: {DadosGlobais.questDisponivel.falaInicio}");
                DadosGlobais.QuestAtiva = DadosGlobais.questDisponivel;
                DadosGlobais.questDisponivel = null;
                DadosGlobais.progressoAtual = 0;
            }
            else
            {

                textoDiaologo.text = ($"{nome} diz: Ouvi dizer que o " +
                    $"{DadosGlobais.questDisponivel.nomeNPCEmissor} esta a sua procura.");
            }
            return;
        }

        //Cenario 3: NPC sem quest
        textoDiaologo.text = ($"{nome} diz: Olá aventureiro! O dia está lindo hoje, não acha?");

    }

    void EntregarRecompensa(Quest questConcluida)
    {
        //Entrega as recompensas ao player
        DadosGlobais.moedasAtualJogador += questConcluida.recompensaOuro;
        DadosGlobais.xpAtualJogador += questConcluida.recompensaXP;

        //Proxima quest da questline
        DadosGlobais.QuestAtiva = null;
        DadosGlobais.questDisponivel = questConcluida.proximaQuest;

        //Se nao tem quests disponiveis a historia foi concluida!
        if(DadosGlobais.questDisponivel == null)
        {
            DadosGlobais.historiaConcluida = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = true;
            jogadorRef = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = false;

            if(painelDialogo != null)
            {
                //Fecha a caixa de dialogo (painel)
                painelDialogo.SetActive(false);
            }            
        }
    }
}
