using UnityEngine;

public class StorageInteract : MonoBehaviour
{
    public StorageManager storageManager;

    void OnMouseDown()
    {
        Debug.Log("CLICOU NO ARMAZEM");

        if (!storageManager.IsOpen())
        {
            storageManager.OpenStorage();
        }
    }
}


/*using UnityEngine;

public class StorageInteract : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Tecla I pressionada");

            StorageManager.Instance.ToggleStorage();
        }
    }
}

*/
/*using UnityEngine;

public class StorageInteract : MonoBehaviour
{
    public StorageManager storageManager;

    void OnMouseDown()
    {
        Debug.Log("CLICOU NO ARMAZEM");

        if (!storageManager.IsOpen())
        {
            storageManager.OpenStorage();
        }
    }
}*/