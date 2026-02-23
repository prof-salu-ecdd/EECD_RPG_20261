using UnityEngine;

public class AtributosCombate : MonoBehaviour
{
    public string nomePersonagem;
    public int hpMaximo = 100;
    public int hpAtual;
    public int danoBase = 10;

    private void Start()
    {
        hpAtual = hpMaximo;
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
    }
}
