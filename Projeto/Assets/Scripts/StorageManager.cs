using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public GameObject storageUI;

    private bool isOpen = false;

    public void ToggleStorage()
    {
        isOpen = !isOpen;

        storageUI.SetActive(isOpen);
    }
}