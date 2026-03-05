using UnityEngine;

public class RecompensaInimigo : MonoBehaviour
{
    [Header("Recompensa Base")]
    public int xpDrop = 50;
    public int moedasDrop = 5;

    private void Start()
    {
        AtributosCombate atributos = GetComponent<AtributosCombate>();

        //Ganha 50% extra de bonus por nivel
        if(atributos != null && atributos.nivel > 1)
        {
            //Arredonda o valor e converte para inteiro
            xpDrop += Mathf.RoundToInt(xpDrop * 0.5f * (atributos.nivel - 1));
            moedasDrop += Mathf.RoundToInt(moedasDrop * 0.5f * (atributos.nivel - 1));
        }
    }
}
