using UnityEngine;
using UnityEngine.UI;

public class AtributosCombate : MonoBehaviour
{
    public string nomePersonagem;
    public int hpMaximo = 100;
    public int hpAtual;
    public int danoBase = 10;

    [Header("UI")]
    public Slider minhaBarraDeVida;

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            if(DadosGlobais.hpAtualJogador != -1)
            {
                hpAtual = DadosGlobais.hpAtualJogador;
            }
            else
            {
                hpAtual = hpMaximo;
            }
        }
        else
        {
            hpAtual = hpMaximo;
        }

            AtualizarBarra();
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
