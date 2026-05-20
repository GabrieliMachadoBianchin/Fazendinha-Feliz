using UnityEngine;

/// Abre o armazém ao clicar no objeto (requer Collider).
public class StorageInteract : MonoBehaviour
{
    public StorageManager storageManager;

    void OnMouseDown()
    {
        if (storageManager != null && !storageManager.IsOpen())
            storageManager.OpenStorage();
    }
}