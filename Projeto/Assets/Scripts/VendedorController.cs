using UnityEngine;

public class VendedorController : MonoBehaviour
{
    public CoinManager coinManager;

    public ArmazemController armazem;

    [Header("Loja")]
    public string itemName = "Milho";

    public int buyPrice = 20;

    public int sellPrice = 10;

    public LevelSystem levelSystem;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =
                Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    Interagir();
                }
            }
        }
    }

    void Interagir()
    {
        Debug.Log("Interagiu com vendedor.");

        // TESTE:
        Comprar(1);

        // Para vender:
        // Vender(1);
    }

    public void Comprar(int quantidade)
    {
        int custo = buyPrice * quantidade;

        if (coinManager.RemoveCoins(custo))
        {
            armazem.AdicionarItem(itemName, quantidade);

            Debug.Log(
                "Comprou " +
                quantidade +
                " de " +
                itemName
            );
        }
    }

    public void Vender(int quantidade)
    {
        if (armazem.RemoverItem(itemName, quantidade))
        {
            int ganho = sellPrice * quantidade;

            coinManager.AddCoins(ganho);

            Debug.Log(
                "Vendeu " +
                quantidade +
                " de " +
                itemName
            );
        }
    }
}