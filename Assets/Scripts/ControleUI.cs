using UnityEngine;

public class ControleUI : MonoBehaviour
{
    [Header("Paineis")]
    //Adicionar TODOS os paineis do jogo
    public GameObject painelInventario;
    public GameObject painelCrafting;
    public GameObject painelStatus;


    //IMPORTE QUE TODOS OS PAINEIS COMEÇEM DESLIGADOS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Fecha os outros paineis antes de abrir o atual
            painelCrafting.SetActive(false);
            painelStatus.SetActive(false);
            //Inverte o estado do inventario abre/fecha
            painelInventario.SetActive(!painelInventario.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Fecha os outros paineis antes de abrir o atual
            painelCrafting.SetActive(false);
            painelInventario.SetActive(false);
            //Inverte o estado do crafting abre/fecha
            painelStatus.SetActive(!painelStatus.activeSelf);

            if (painelStatus.activeSelf)
            {
                painelStatus.GetComponent<FichaStatus>().AtualizarFicha();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            //Fecha os outros paineis antes de abrir o atual
            painelInventario.SetActive(false);
            painelStatus.SetActive(false);
            //Inverte o estado do crafting abre/fecha
            painelCrafting.SetActive(!painelCrafting.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Desliga todos os paineis
            painelInventario.SetActive(false);
            painelCrafting.SetActive(false);
        }
    }

}