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

    [Header("UI")]
    public Slider minhaBarraDeVida;

    private void Start()
    {
        CalcularStatus();

        if (gameObject.CompareTag("Player") && DadosGlobais.hpAtualJogador != -1)
        {
            hpAtual = DadosGlobais.hpAtualJogador;
        }
        else
        {
            hpAtual = hpMaximo;
        }

        AtualizarBarra();
    }

    public void CalcularStatus()
    {
        //Ganha +20 de HP e +5 de dano por nivel
        hpMaximo = hpBase + ((nivel - 1) * 20);
        danoAtual = danoBase + ((nivel - 1) * 5);

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
