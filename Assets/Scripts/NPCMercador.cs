using TMPro;
using UnityEngine;

public class NPCMercador : MonoBehaviour
{
    [Header("Interface da Loja")]
    public GameObject painelLoja;
    public TextMeshProUGUI textoFeedback;

    [Header("Inventario")]
    public SistemaInventario inventario;
    public DadosItem pocaoVida;

    [Header("Itens")]
    public int precoPocaoVida = 5;

    [Header("Upgrades")]
    public int precoBonusAtaque = 5;
    public int precoBonusDefesa = 10;

    private bool jogadorPerto;

    private void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            AbrirLoja();
        }
    }

    public void AbrirLoja()
    {
        painelLoja.SetActive(true);
        textoFeedback.text = "Seja bem-vindo aventureiro!\nO que vai querer hoje?";
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
    }

    public void ComprarPocaoCura()
    {
        //1. Verificar se o player tem dinheiro suficiente
        if(DadosGlobais.moedasAtualJogador >= precoPocaoVida)
        {
            //2. Player tem dinheiro. Cobramos o valor
            DadosGlobais.moedasAtualJogador -= precoPocaoVida;

            //3. Entrega o item
            inventario.AdicionarItem(pocaoVida, 1);

            //4. Exibe o feedback da compra
            textoFeedback.text = $"Poção de cura comprada com sucesso! " +
                $"Saldo atual: {DadosGlobais.moedasAtualJogador}";
        }
        else
        {
            //Player sem dinheiro
            textoFeedback.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    public void ComprarUpgradeAtaque()
    {
        //1. Verificar se o player tem dinheiro suficiente
        if (DadosGlobais.moedasAtualJogador >= precoBonusAtaque)
        {
            //2. Player tem dinheiro. Cobramos o valor
            DadosGlobais.moedasAtualJogador -= precoBonusAtaque;
            DadosGlobais.bonusAtaque += 5;

            textoFeedback.text = $"Bonus de ATAQUE comprado com sucesso! " +
                $"Saldo atual: {DadosGlobais.moedasAtualJogador}";

            //Deixa mais caro, dobra o preço a cada compra (Inflação RPG)
            precoBonusAtaque *= 2;
        }
        else
        {
            //Player sem dinheiro
            textoFeedback.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    public void ComprarUpgradeDefesa()
    {
        if(DadosGlobais.moedasAtualJogador >= precoBonusDefesa)
        {
            DadosGlobais.moedasAtualJogador -= precoBonusDefesa;
            DadosGlobais.bonusDefesa += 10;

            precoBonusDefesa *= 3;

            textoFeedback.text = $"Bonus de DEFESA comprado com sucesso! " +
                $"Saldo atual: {DadosGlobais.moedasAtualJogador}";
        }
        else
        {
            //Player sem dinheiro
            textoFeedback.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jogadorPerto = false;
            FecharLoja();
        }
    }
}
