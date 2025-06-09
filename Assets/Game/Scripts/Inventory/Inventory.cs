using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public sealed class Inventory : MonoBehaviour
{
    [Header("Character IK Targets")]
    [SerializeField] private RigLayer _rigLayer;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;

    public static Inventory instance;
    public Transform gunsTarget;
    public Tool[] backpack = new Tool[3];
    [SerializeField] private InputActionReference _interact;
    [SerializeField] private InputActionReference _useTool;
    [SerializeField] private InputActionReference _cycleTools;

    public int currentToolIndex = 0;
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
        PlayerPrefs.SetInt(PREFS_CURRENT_INDEX, currentToolIndex);
        PlayerPrefs.Save();

        Debug.Log($"Inventory retained: „{allIDs}“, currentIndex={currentToolIndex}");
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
            currentToolIndex = savedIndex;
            SwitchTools(currentToolIndex);
        }
        else
        {
            currentToolIndex = 0;
            if (backpack[0] != null)
                SwitchTools(0);
        }
    }

    private void SwitchTools(int newToolIndex)
    {
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null) backpack[i].gameObject.SetActive(false);
        }

        if (currentToolIndex >= 0 && currentToolIndex < backpack.Length && backpack[currentToolIndex] != null)
        {
            backpack[currentToolIndex].gameObject.SetActive(false);
        }

        currentToolIndex = newToolIndex;

        if (backpack[currentToolIndex] != null && backpack[currentToolIndex].GetComponent<Gun>())
        {
            Debug.Log("ShowGunUI" + backpack[currentToolIndex].name);
            backpack[currentToolIndex].GetComponent<Gun>().ShowGunUI();
            Transform lhAnchor = backpack[currentToolIndex].transform.Find("LeftHandTargetAnchor");
            Transform rhAnchor = backpack[currentToolIndex].transform.Find("RightHandTargetAnchor");
            _rigLayer.rig.weight = 1;
                
            if (lhAnchor) {
                rightHandTarget.localPosition = rhAnchor.localPosition;
                rightHandTarget.localRotation = rhAnchor.localRotation;
            }
            if (lhAnchor) {
                leftHandTarget.localPosition = lhAnchor.localPosition;
                leftHandTarget.localRotation = lhAnchor.localRotation;
            }
        }

        if (backpack[currentToolIndex] != null)
            backpack[currentToolIndex].gameObject.SetActive(true);
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

    public void AddToBackpack(Tool tool)
    {
        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] != null) backpack[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < backpack.Length; i++)
        {
            if (backpack[i] == null)
            {
                GameObject instanceGO = Instantiate(tool.gameObject, gunsTarget);
                instanceGO.transform.localPosition = Vector3.zero;
                instanceGO.transform.localRotation = Quaternion.identity;
                if (tool != null) Destroy(tool.gameObject);


                Tool instTool = instanceGO.GetComponent<Tool>();
                instTool.GetComponent<Gun>().enemyGun = false;
                instTool.isPossessed = true;
                if (instTool.GetComponent<Gun>())
                {
                    instTool.GetComponent<Gun>().ShowGunUI();
                }
                
                Transform lhAnchor = instanceGO.transform.Find("LeftHandTargetAnchor");
                Transform rhAnchor = instanceGO.transform.Find("RightHandTargetAnchor");
                
                if (lhAnchor) {
                    rightHandTarget.localPosition = rhAnchor.localPosition;
                    rightHandTarget.localRotation = rhAnchor.localRotation;
                }
                if (lhAnchor) {
                    leftHandTarget.localPosition = lhAnchor.localPosition;
                    leftHandTarget.localRotation = lhAnchor.localRotation;
                }
                
                _rigLayer.rig.weight = 1;

                backpack[i] = instTool;
                currentToolIndex = i;
                instanceGO.gameObject.transform.localScale = new Vector3(1, 1, 1);
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
        if (backpack[currentToolIndex] == null)
        {
            return;
        }

        UseCurrentTool();
    }

    private void StopFiring(InputAction.CallbackContext obj)
    {
        if (backpack[currentToolIndex] == null)
        {
            return;
        }

        Gun gunScript = backpack[currentToolIndex].GetComponent<Gun>();
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
            if (index >= 0 && index < backpack.Length && index != currentToolIndex)
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