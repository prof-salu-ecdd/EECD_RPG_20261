using TMPro;
using UnityEngine;

public class NPCMercador : MonoBehaviour
{
    [Header("Interface da Loja")]
    public GameObject painelLoja;
    public GameObject painelCompra; //add
    public GameObject painelVenda; //add
    public TextMeshProUGUI textoFeedbackCompra; //add
    public TextMeshProUGUI textoFeedbackVenda; //add

    [Header("Gerador de Vendas")]
    public Transform containerVendas; //add
    public GameObject prefabBotaoVenda; //add

    [Header("Dados Jogador")]
    public SistemaInventario inventarioJogador; 
    public AtributosCombate atributosJogador; //add
    
    [Header("Itens")]
    public DadosItem pocaoVida;

    [Header("Valores")]
    public int precoBonusAtaque = 5;
    public int precoBonusDefesa = 10;
    public int precoPocaoVida = 5;

    [Header("Bonus (UPGRADES")]
    public int bonusAtaque = 5;
    public int bonusDefesa = 10;

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
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
    }

    public void FecharLoja()
    {
        painelLoja.SetActive(false);
        painelCompra.SetActive(false);
        painelVenda.SetActive(false);
    }

    public void AbrirCompras()
    {
        painelLoja.SetActive(false);
        painelCompra.SetActive(true);
        painelVenda.SetActive(false);
        textoFeedbackCompra.text = "Tenho muito itens especeias pra voce.";
    }

    public void AbrirVendas()
    {
        painelLoja.SetActive(false);
        painelCompra.SetActive(false);
        painelVenda.SetActive(true);
        GerarListaDeVendas();
        textoFeedbackVenda.text = "O que voce encontrou na floresta?";
    }

    public void ComprarPocaoCura()
    {
        //1. Verificar se o player tem dinheiro suficiente
        if(inventarioJogador.moedas >= precoPocaoVida)
        {
            //2. Player tem dinheiro. Cobramos o valor
            inventarioJogador.ModificadorMoedas(-precoPocaoVida);

            //3. Entrega o item
            inventarioJogador.AdicionarItem(pocaoVida, 1);

            //4. Exibe o feedback da compra
            textoFeedbackCompra.text = $"Poção de cura comprada com sucesso! " +
                $"Saldo atual: {inventarioJogador.moedas}";
        }
        else
        {
            //Player sem dinheiro
            textoFeedbackCompra.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    public void ComprarUpgradeAtaque()
    {
        //1. Verificar se o player tem dinheiro suficiente
        if (inventarioJogador.moedas >= precoBonusAtaque)
        {
            //2. Player tem dinheiro. Cobramos o valor
            inventarioJogador.ModificadorMoedas(-precoBonusAtaque);
            atributosJogador.bonusAtaque += bonusAtaque;

            //Recalcula o status do player
            //Recalcula o status do player
            atributosJogador.CalcularStatus();

            textoFeedbackCompra.text = $"Bonus de ATAQUE ({bonusAtaque}) comprado com sucesso! " +
                $"Saldo atual: {inventarioJogador.moedas}";

            //Deixa mais caro, dobra o preço a cada compra (Inflação RPG)
            precoBonusAtaque *= 2;
        }
        else
        {
            //Player sem dinheiro
            textoFeedbackCompra.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    public void ComprarUpgradeDefesa()
    {
        if(inventarioJogador.moedas >= precoBonusDefesa)
        {
            inventarioJogador.ModificadorMoedas(-precoBonusDefesa);
            atributosJogador.bonusDefesa += bonusDefesa;
            atributosJogador .CalcularStatus();
            atributosJogador.ReceberCura(bonusDefesa);
            precoBonusDefesa *= 3;

            textoFeedbackCompra.text = $"Bonus de DEFESA ({bonusDefesa}) comprado com sucesso! " +
                $"Saldo atual: {inventarioJogador.moedas}";
        }
        else
        {
            //Player sem dinheiro
            textoFeedbackCompra.text = "Ouro insuficiente. Vá caçar alguns monstros...";
        }
    }

    private void GerarListaDeVendas()
    {
        //Garantir que nao tenha itens duplicados
        foreach(Transform filho in containerVendas)
        {
            Destroy(filho.gameObject);
        }

        //Le o inventario do jogador e exibe os itens a venda
        foreach(SlotInventario slot in inventarioJogador.inventario)
        {
            if(slot.dadosDoItem.valorEmOuro >= 2 && slot.quantidade > 0)
            {
                //Cria um botao para o item
                GameObject novoBotao = Instantiate(prefabBotaoVenda, containerVendas);

                novoBotao.GetComponent<BotaoVendaUI>().ConfigurarBotao(slot.dadosDoItem,
                                                                       slot.quantidade, 
                                                                       this);
            }
        }
    }

    public void ExecutarVenda(DadosItem itemParaVender)
    {
        int lucro = itemParaVender.valorEmOuro / 2;
        inventarioJogador.ModificadorMoedas(lucro);

        inventarioJogador.RemoverItem(itemParaVender, 1);

        textoFeedbackVenda.text = $"Vendeu {itemParaVender.nomeDoItem} por {lucro} Ouro!";

        //Apos cada venda, atualiza a lista
        GerarListaDeVendas();
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
