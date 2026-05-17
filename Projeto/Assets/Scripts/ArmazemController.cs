using UnityEngine;
using System.Collections.Generic;

public class ArmazemController : MonoBehaviour
{
    private Dictionary<string, int> itens =
        new Dictionary<string, int>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            MostrarItens();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =
                Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    MostrarItens();
                }
            }
        }
    }

    public void AdicionarItem(string nome, int quantidade)
    {
        if (itens.ContainsKey(nome))
        {
            itens[nome] += quantidade;
        }
        else
        {
            itens.Add(nome, quantidade);
        }

        Debug.Log(nome + " adicionado ao armazém.");
    }

    public bool RemoverItem(string nome, int quantidade)
    {
        if (itens.ContainsKey(nome))
        {
            if (itens[nome] >= quantidade)
            {
                itens[nome] -= quantidade;

                if (itens[nome] <= 0)
                {
                    itens.Remove(nome);
                }

                return true;
            }
        }

        return false;
    }

    public int GetQuantidade(string nome)
    {
        if (itens.ContainsKey(nome))
        {
            return itens[nome];
        }

        return 0;
    }

    public void MostrarItens()
    {
        Debug.Log("=== ARMAZÉM ===");

        foreach (var item in itens)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
}