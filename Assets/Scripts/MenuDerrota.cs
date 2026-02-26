using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    public void BotaoJogarNovamente()
    {
        SceneManager.LoadScene("Arena");
    }
}
