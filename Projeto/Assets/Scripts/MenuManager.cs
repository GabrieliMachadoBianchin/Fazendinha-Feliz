using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("ModoJogo"); 
    }
    public void Sair()
    {
        Application.Quit();
    }
}