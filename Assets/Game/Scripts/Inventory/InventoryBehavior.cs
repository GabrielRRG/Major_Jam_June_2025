using UnityEngine;

public class InventoryBehavior : MonoBehaviour
{
    private static Transform _savedTool = null;
    private static GameObject _backpack;

    public static void DisableInventory()
    {
        if (!_backpack) _backpack = GameObject.FindGameObjectWithTag("Backpack");
        Inventory.instance.enabled = false;
        Inventory.instance.gunsTarget = _backpack.transform;
        foreach (Transform toolTransform in _backpack.transform)
        {
            if (toolTransform.gameObject.activeSelf)
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
        Inventory.instance.enabled = true;
    }
}
