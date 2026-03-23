using UnityEngine;
using UnityEngine.UI;

public class AtributosCombate : MonoBehaviour
{
    public string nomePersonagem;

    [Header("Evolução")]
    public int nivel = 1;

    [Header("Status base")]
    public int hpBase = 100;
    public int danoBase = 10;

    [Header("Status calculados")]
    public int hpMaximo;
    public int hpAtual;
    public int danoAtual;

    [Header("Bonus (UPGRADES)")]
    public int bonusAtaque = 0;
    public int bonusDefesa = 0;

    [Header("UI")]
    public Slider minhaBarraDeVida;

    private void Awake()
    {
        CalcularStatus();
        hpAtual = hpMaximo;
    }

    private void Start()
    {
        AtualizarBarra();
    }

    public void CalcularStatus()
    {
        //Ganha +20 de HP e +5 de dano por nivel + adicionando os bonus (UPGRADES)
        hpMaximo = hpBase + ((nivel - 1) * 20) + bonusDefesa;
        danoAtual = danoBase + ((nivel - 1) * 5) + bonusAtaque;

        if(hpAtual > hpMaximo)
        {
            hpAtual = hpMaximo;
        }
    }

    public void ReceberDano(int valorDano)
    {
        hpAtual -= valorDano;
        Debug.Log($"{nomePersonagem} recebeu {valorDano} de dano!\nHP: {hpAtual}");

        if(hpAtual <= 0)
        {
            hpAtual = 0;//Evitar valores negativos
            gameObject.SetActive(false);
        }

        AtualizarBarra();
    }

    public void ReceberCura(int valorCura)
    {
        hpAtual += valorCura;
        Debug.Log($"{nomePersonagem} recebeu {valorCura} de cura!\nHP: {hpAtual}");

        if (hpAtual >= hpMaximo)
        {
            hpAtual = hpMaximo;//Evita valores superiores ao HP Maximo
        }

        AtualizarBarra();
    }

    public void AtualizarBarra()
    {
        if(minhaBarraDeVida != null)
        {
            minhaBarraDeVida.maxValue = hpMaximo;
            minhaBarraDeVida.value = hpAtual;
        }
    }
}
