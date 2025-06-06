using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Inventory : MonoBehaviour
{
    public static Inventory instance;    
    public Transform gunsTarget;
    public Tool[] backpack;
    [SerializeField] private InputActionReference _interact;
    [SerializeField] private InputActionReference _useTool;
    [SerializeField] private InputActionReference _cycleTools;

    private int _currentToolIndex = 0;
    private Tool _toolInRange = null;
    
    private const string PREFS_BACKPACK = "Player_Backpack";
    private const string PREFS_CURRENT_INDEX = "Player_CurrentTool";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void SaveBackpack()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null)
            {
                sb.Append(backpack[i].name);
            }

            if (i < backpack.Length - 1)
                sb.Append(";");
        }

        string allIDs = sb.ToString();
        PlayerPrefs.SetString(PREFS_BACKPACK, allIDs);
        PlayerPrefs.SetInt(PREFS_CURRENT_INDEX, _currentToolIndex);
        PlayerPrefs.Save();

        Debug.Log($"Inventory retained: „{allIDs}“, currentIndex={_currentToolIndex}");
    }

    private void SwitchTools(int _newToolIndex)
    {
        backpack[_currentToolIndex].gameObject.SetActive(false);
        _currentToolIndex = _newToolIndex;
        if (backpack[_currentToolIndex].gameObject.GetComponent<Gun>())
        {
            backpack[_currentToolIndex].gameObject.GetComponent<Gun>().ShowGunUI();
        }

        backpack[_currentToolIndex].gameObject.SetActive(true);
    }

    public void UseCurrentTool()
    {
        if (backpack[_currentToolIndex] != null)
        {
            backpack[_currentToolIndex].Use();
        }
        else
        {
            Debug.Log("No tool in this slot!");
        }
    }

    public void AddToBackpack(Tool tool)
    {
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] == null)
            {
                backpack[i] = tool;
                _currentToolIndex = i;
                if (tool.gameObject.GetComponent<Gun>()) //If tool is a gun
                {
                    tool.GetComponent<Gun>().ShowGunUI();
                }

                return;
            }
            else
            {
                backpack[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Tool>())
        {
            return;
        }

        _toolInRange = other.gameObject.GetComponent<Tool>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.GetComponent<Tool>())
        {
            return;
        }

        _toolInRange = null;
    }

    private void EquipToolInRange(InputAction.CallbackContext obj)
    {
        if (!_toolInRange || _toolInRange.isPossessed)
        {
            return;
        } //Return if tool is already possessed

        print("Tool Equipped");
        print(_toolInRange);
        _toolInRange.EquipTool();
    }

    private void UseToolAction(InputAction.CallbackContext obj)
    {
        if (backpack[_currentToolIndex] == null)
        {
            return;
        }

        UseCurrentTool();
    }

    private void StopFiring(InputAction.CallbackContext obj)
    {
        if (backpack[_currentToolIndex] == null)
        {
            return;
        }

        Gun gunScript = backpack[_currentToolIndex].GetComponent<Gun>();
        if (!gunScript)
        {
            return;
        } //If tool is not a gun then return

        gunScript.StopFire();
    }

    private void CycleThroughTools(InputAction.CallbackContext obj)
    {
        string key = obj.control.displayName;
        if (int.TryParse(key, out int index))
        {
            index -= 1; //Convert to a zero based index
            if (index >= 0 && index <= 2 && index != _currentToolIndex)
            {
                SwitchTools(index);
            }
            else
            {
                print("Index is out of bounds");
            }
        }
        else
        {
            print("Unknown key pressed");
        }
    }

    private void OnEnable()
    {
        _interact.action.started += EquipToolInRange;
        _cycleTools.action.started += CycleThroughTools;

        _useTool.action.started += UseToolAction;
        _useTool.action.canceled += StopFiring;
    }

    private void OnDisable()
    {
        _interact.action.started -= EquipToolInRange;
        _cycleTools.action.started -= CycleThroughTools;


        _useTool.action.started -= UseToolAction;
        _useTool.action.canceled -= StopFiring;

    }

}