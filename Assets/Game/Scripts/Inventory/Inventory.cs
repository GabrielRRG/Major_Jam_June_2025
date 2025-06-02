using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public sealed class Inventory : MonoBehaviour
{
    [SerializeField] private Tool[] _backpack = new Tool[3];
    private int _currentToolIndex = 0;

    private void Start()
    {
        // Optionally initialize or assign tools here
        // Example: backpack[0] = GetComponent<Axe>();
    }

    public void UseCurrentTool()
    {
        if (_backpack[_currentToolIndex] != null)
        {
            _backpack[_currentToolIndex].Use();
        }
        else
        {
            Debug.Log("No tool in this slot!");
        }
    }
}