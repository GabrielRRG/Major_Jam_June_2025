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
        
        transform.SetParent(_backpack.transform);

        //transform.Rotate(_player.transform.forward);

        _playerInventory.AddToBackpack(this);
        transform.position = _playerInventory.gunsTarget.position;
        transform.localRotation = Quaternion.Euler(-90,0,90);
    }

}