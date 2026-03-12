using TMPro;
using UnityEngine;

public class HudMissao : MonoBehaviour
{
    [Header("HUD da missão")]
    public TextMeshProUGUI textoTrackerQuest;

    private void Update()
    {
        if (textoTrackerQuest == null)
        {
            return;
        }

        if (DadosGlobais.historiaConcluida)
        {
            textoTrackerQuest.text = "História concluída";
        }
        else if(DadosGlobais.QuestAtiva != null)
        {
            if (DadosGlobais.QuestAtiva.tipoDeMissao == TipoQuest.CacarMonstros ||
                DadosGlobais.QuestAtiva.tipoDeMissao == TipoQuest.ColetarItens) 
            {
                textoTrackerQuest.text = $"Missão ativa: " +
                    $"{DadosGlobais.QuestAtiva.descricaoNoHUD} " +
                    $"({DadosGlobais.progressoAtual} / {DadosGlobais.QuestAtiva.quantidade} " +
                    $"{DadosGlobais.QuestAtiva.nomeObjetivo})";
            }
            else
            {
                textoTrackerQuest.text = $"Missão Ativa: {DadosGlobais.QuestAtiva.descricaoNoHUD}";
            }
        }else if(DadosGlobais.questDisponivel != null)
        {
            textoTrackerQuest.text = $"Nova Missão: Procure o triângulo AZUL no (a) " +
                $"{DadosGlobais.questDisponivel.nomeNPCEmissor}";
        }
        else
        {
            textoTrackerQuest.text = "Nenhuma missão ativa.";
        }
    }
}
