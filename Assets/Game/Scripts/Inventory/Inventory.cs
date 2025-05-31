using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Inventory : MonoBehaviour
{
    [SerializeField] private Tool[] backpack = new Tool[3]; // Backpack Size
    private int currentToolIndex = 0;

    private void Start()
    {
        // Optionally initialize or assign tools here
        // Example: backpack[0] = GetComponent<Axe>();
    }

    public void UseCurrentTool()
    {
        if (backpack[currentToolIndex] != null)
        {
            backpack[currentToolIndex].Use();
        }
        else
        {
            Debug.Log("No tool in this slot!");
        }
    }
}