using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatilhoGuardiao : MonoBehaviour
{
    [Header("Formação dos inimigos")]
    [Tooltip("Arraste os prefabs dos inimigos da aba PROJECT para cá.")]
    public List<GameObject> inimigos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Passa a lista inteira para a Memoria Global
            DadosGlobais.prefabsInimigos = new List<GameObject>(inimigos);

            SceneManager.LoadScene("Arena");
        }
    }
}
