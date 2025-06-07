using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Transform gunsTarget;
    public Tool[] backpack = new Tool[3];
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

        if (PlayerPrefs.HasKey(PREFS_BACKPACK))
        {
            LoadBackpack();
        }
    }

    public void SaveBackpack()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null)
            {
                sb.Append(backpack[i].toolName);
            }

            if (i < backpack.Length - 1)
                sb.Append(";");
        }

        string allIDs = sb.ToString();
        Debug.Log(allIDs);
        PlayerPrefs.SetString(PREFS_BACKPACK, allIDs);
        PlayerPrefs.SetInt(PREFS_CURRENT_INDEX, _currentToolIndex);
        PlayerPrefs.Save();

        Debug.Log($"Inventory retained: „{allIDs}“, currentIndex={_currentToolIndex}");
    }

    public void LoadBackpack()
    {
        if (!PlayerPrefs.HasKey(PREFS_BACKPACK))
        {
            Debug.Log("Inventory: no data to load.");
            return;
        }

        string allIDs = PlayerPrefs.GetString(PREFS_BACKPACK);
        int savedIndex = PlayerPrefs.GetInt(PREFS_CURRENT_INDEX, 0);

        Debug.Log($"Inventory is loading: „{allIDs}“, savedIndex={savedIndex}");

        string[] ids = allIDs.Split(';');

        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null)
            {
                Destroy(backpack[i].gameObject);
                backpack[i] = null;
            }

            if (i < ids.Length && !string.IsNullOrEmpty(ids[i]))
            {
                string id = ids[i];
                GameObject prefab = Resources.Load<GameObject>($"Tools/{id}");
                if (prefab == null)
                {
                    Debug.LogWarning($"Inventory: there's no prefab for ToolID = {id} (Resources/Tools/{id}).");
                    continue;
                }

                GameObject tool = Instantiate(prefab);
                Tool toolComponent = tool.GetComponent<Tool>();
                if (toolComponent == null)
                {
                    Debug.LogError($"Prefab Tools/{id} has no component Tool!");
                    Destroy(tool);
                    continue;
                }
                AddToBackpack(toolComponent);
            }
        }

        if (savedIndex >= 0 && savedIndex < backpack.Length && backpack[savedIndex] != null)
        {
            _currentToolIndex = savedIndex;
            SwitchTools(_currentToolIndex);
        }
        else
        {
            _currentToolIndex = 0;
            if (backpack[0] != null)
                SwitchTools(0);
        }
    }

    private void SwitchTools(int newToolIndex)
    {
        if (_currentToolIndex >= 0 && _currentToolIndex < backpack.Length && backpack[_currentToolIndex] != null)
        {
            backpack[_currentToolIndex].gameObject.SetActive(false);
        }

        _currentToolIndex = newToolIndex;

        if (backpack[_currentToolIndex] != null && backpack[_currentToolIndex].GetComponent<Gun>())
        {
            backpack[_currentToolIndex].GetComponent<Gun>().ShowGunUI();
        }

        if (backpack[_currentToolIndex] != null)
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
                GameObject instanceGO = Instantiate(tool.gameObject, gunsTarget);
                
                instanceGO.transform.localPosition = Vector3.zero;
                instanceGO.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);
                if(tool != null) Destroy(tool.gameObject);
                

                Tool instTool = instanceGO.GetComponent<Tool>();
                instTool.isPossessed = true;
                if (instTool.GetComponent<Gun>())
                {
                    instTool.GetComponent<Gun>().ShowGunUI();
                }

                backpack[i] = instTool;
                _currentToolIndex = i;
                return;
            }
        }

        Debug.Log("Backpack is full, cannot add tool.");
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
        }

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
        }

        gunScript.StopFire();
    }

    private void CycleThroughTools(InputAction.CallbackContext obj)
    {
        string key = obj.control.displayName;
        if (int.TryParse(key, out int index))
        {
            index -= 1;
            if (index >= 0 && index < backpack.Length && index != _currentToolIndex)
            {
                SwitchTools(index);
            }
            else
            {
                Debug.Log("Index is out of bounds or same.");
            }
        }
        else
        {
            Debug.Log("Unknown key pressed");
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