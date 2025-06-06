using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public bool isPossessed = false;

    private float _rotSpeed = 30f;
    public virtual void Update()
    {
        if(isPossessed) { return; }
        transform.Rotate(0, _rotSpeed * Time.deltaTime, 0);
    }
    public abstract void Use();

    public void EquipTool()
    {
        isPossessed = true;

        GameObject _backpack = GameObject.FindGameObjectWithTag("Backpack");
        if (_backpack == null)
        {
            Debug.LogError($"[Tool.EquipTool] Объект с тегом \"Backpack\" не найден в сцене для {name}");
            return;
        }

        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogError($"[Tool.EquipTool] Объект с тегом \"Player\" не найден в сцене для {name}");
            return;
        }

        Inventory _playerInventory = _player.GetComponent<Inventory>();
        if (_playerInventory == null)
        {
            Debug.LogError($"[Tool.EquipTool] На объекте Player отсутствует компонент Inventory (для {name})");
            return;
        }

        // Если всё найдено — продолжаем:
        transform.SetParent(_backpack.transform);

        transform.localPosition = Vector3.right;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(_player.transform.forward);

        _playerInventory.AddToBackpack(this);
        transform.localPosition += new Vector3(-0.2f, 1, 0);
    }

}