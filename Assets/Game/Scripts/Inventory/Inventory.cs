using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public sealed class Inventory : MonoBehaviour
{
    [SerializeField] private Tool[] _backpack = new Tool[3];
    [SerializeField] private InputActionReference _interact;
    [SerializeField] private InputActionReference _useTool;

    private int _currentToolIndex = 0;
    private Tool _toolInRange = null;


    private void OnTriggerEnter(Collider other)
    {
        //if (!other.gameObject.GetComponent<Tool>()) { return; }
        _toolInRange = other.gameObject.GetComponent<Tool>();
        print("Press E to equip");
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.GetComponent<Tool>()) { return; }
        _toolInRange = null;
    }

    private void EquipToolInRange(InputAction.CallbackContext obj)
    {
        if(!_toolInRange || _toolInRange.isPossessed) { return; } //Return if tool is already possessed
        print("Tool Equipped");
        print(_toolInRange);
        _toolInRange.EquipTool();
    }
    private void OnEnable()
    {
        _interact.action.started += EquipToolInRange;
        _useTool.action.started += UseToolAction;
    }

    private void UseToolAction(InputAction.CallbackContext obj)
    {
        if (_backpack[_currentToolIndex] == null) { return; }
        UseCurrentTool();
    }

    private void OnDisable()
    {
        _interact.action.started -= EquipToolInRange;
        _useTool.action.started -= UseToolAction;
    }

    private void SwitchTools()
    {

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
    public void AddToBackpack(Tool tool)
    {
        for(int i = 0; i < _backpack.Length; i++)
        {
            if (_backpack[i] == null) 
            { 
                _backpack[i] = tool;
                _currentToolIndex = i;
                return;
            }
        }
    }
}