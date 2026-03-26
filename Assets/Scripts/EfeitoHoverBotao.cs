using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// IPointerEnterHandler e IPointerExitHandler ==> interfaces
public class EfeitoHoverBotao : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Feedback mercador")]
    public TextMeshProUGUI feedbackMercador;

    [Header("Itens [Deixar vazio para upgrades]")]
    public DadosItem item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(gameObject.name == "Botao_Upgrade_01")
        {
            feedbackMercador.text = "Afiar Espada: 5 moedas";
        }else if(gameObject.name == "Botao_Upgrade_02")
        {
            feedbackMercador.text = "Reforçar Armadura: 10 moedas";
        }else if(gameObject.name == "Botao_Item_01" && item != null)
        {
            feedbackMercador.text = $"Poção de Cura: {item.valorEmOuro} moedas";
        }
    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        feedbackMercador.text = "Como posso ajudar você?";
    }
}
