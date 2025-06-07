using UnityEngine;

public class InventoryBehavior : MonoBehaviour
{
    private static Transform _savedTool = null;
    private static GameObject _backpack;
    private static Inventory _inventory;

    private void Start()
    {
        _backpack = GameObject.FindGameObjectWithTag("Backpack");
        _inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public static void DisableInventory()
    {
        _inventory.enabled = false;
        foreach (Transform toolTransform in _backpack.transform)
        {
            if (toolTransform.gameObject.activeSelf) //That means this is the tool
            {
                _savedTool = toolTransform;
                toolTransform.gameObject.SetActive(false);
            }
        }
    }
    public static void EnableInventory()
    {
        if(_savedTool)
        {
            _savedTool.gameObject.SetActive(true);
        }
        _inventory.enabled = true;
    }
}
