using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public void GoHome()
    {
        SceneManager.LoadScene("Menu");
    }
}