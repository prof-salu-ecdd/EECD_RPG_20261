using System.Globalization;
using UnityEngine;

public enum TipoQuest { EncontrarNPC, CacarMonstros, ColetarItens}

[CreateAssetMenu(fileName ="NovaQuest", menuName ="Sistema RPG/Missao")]

public class Quest : ScriptableObject
{
    public string nomeQuest;
    public TipoQuest tipoDeMissao;

    [Header("Atores da missão")]
    [Tooltip("Nome do NPC que da a quest")]
    public string nomeNPCEmissor;
    [Tooltip("Nome do NPC que conclui a quest")]
    public string nomeNPCDestino;

    [Header("Textos da missão")]
    [TextArea] public string falaInicio; //Quando o NPC da a quest
    [TextArea] public string falaAndamento; //Quando o jogador ainda não acabou a quest
    [TextArea] public string falaConclusao; //Quando o jogador entrega a quest
    [TextArea] public string descricaoNoHUD; //Texto do rastreador

    [Header("Objetivos")]
    public int quantidade;
    [Tooltip("Ex: Carne de lobo, Planta curativa, Orcs, Goblins etc.")]
    public string nomeObjetivo;

    [Header("Recompensas")]
    public int recompensaOuro;
    public int recompensaXP;

    [Header("Sequencia da Quest Line")]
    [Tooltip("Deixe vazio caso seja a ultima quest!")]
    public Quest proximaQuest;
}
