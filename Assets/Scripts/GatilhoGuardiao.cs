using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatilhoGuardiao : MonoBehaviour
{
    [Header("Identificação")]
    [Tooltip("Dê um nome único. Ex. Marmota_001")]
    public string idUnico;

    [Header("Formação dos inimigos")]
    [Tooltip("Arraste os prefabs dos inimigos da aba PROJECT para cá.")]
    public List<GameObject> inimigos;

    private void Start()
    {
        //verifica a lista de inimigos derrotados
        if (DadosGlobais.inimigosDerrotados.Contains(idUnico))
        {
            //Desliga o inimigo caso ele tenha sido derrotado
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IniciadorBatalha iniciador = GetComponent<IniciadorBatalha>();

            if (iniciador != null) 
            {
                iniciador.DispararBatalha(collision.gameObject,
                                          idUnico,
                                          inimigos);
            }
        }
    }
}
