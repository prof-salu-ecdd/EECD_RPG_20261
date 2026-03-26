using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FichaStatus : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI txtNome;
    public TextMeshProUGUI txtNivel;
    public TextMeshProUGUI txtAtaque;
    public TextMeshProUGUI txtXP;

    [Header("Barras")]
    public Slider barraXP;
    public Slider barraVida;

    private ProgressoJogador progresso;
    private AtributosCombate atributos;

    private void Start()
    {
        /*O Painel de status deve começar ligado na unity para podermos capturar
        os dados antes do jogo*/
        gameObject.SetActive(false);
        //Captura os dados do jogador
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            progresso = player.GetComponent<ProgressoJogador>();
            atributos = player.GetComponent<AtributosCombate>();
        }

        AtualizarFicha();
    }

    public void AtualizarFicha()
    {
        if (atributos == null || progresso == null) return;

        txtNome.text = $"Heroi: {atributos.nomePersonagem}";
        txtNivel.text = $"Nivel: {atributos.nivel}";

        int ataqueSemBonus = atributos.danoAtual - atributos.bonusAtaque;
        int defesaSemBonus = atributos.hpMaximo - atributos.bonusDefesa;

        txtAtaque.text = $"Ataque: {atributos.danoAtual} (Base {ataqueSemBonus} + " +
            $"{atributos.bonusAtaque})";

        //Preenchendo a barra de vida
        barraVida.maxValue = atributos.hpMaximo;
        barraVida.value = atributos.hpAtual;

        //Preenchendo a barra de XP
        int metaXP = 0;
        int nivelHeroi = atributos.nivel;

        if(nivelHeroi <= progresso.xpNecessariaPorNivel.Length)
        {
            metaXP = progresso.xpNecessariaPorNivel[nivelHeroi-1];
        }

        barraXP.maxValue = metaXP;
        barraXP.value = progresso.xpAtual;
        txtXP.text = $"{progresso.xpAtual} / {metaXP}";
    }
}
