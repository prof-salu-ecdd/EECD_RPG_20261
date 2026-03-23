using TMPro;
using UnityEngine;

public class BotaoVendaUI : MonoBehaviour
{
    public TextMeshProUGUI textoBotao;
    private DadosItem itemBotao;
    private NPCMercador mercadorVinculado;

    public void ConfigurarBotao(DadosItem item, int quantidade, NPCMercador mercador)
    {
        itemBotao = item;
        mercadorVinculado = mercador;

        //Sempre oferecera a metade do preco do item.
        int precoDeVenda = item.valorEmOuro / 2;

        textoBotao.text = $"{item.nomeDoItem} x({quantidade}) - {precoDeVenda} Ouro";
    }

    //Linkar essa função na Unity no evento OnClick do botao.
    public void ClickVender()
    {
        mercadorVinculado.ExecutarVenda(itemBotao);
    }
    
}
