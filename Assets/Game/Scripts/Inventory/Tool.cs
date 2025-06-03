using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public bool isPossessed = false;

    private float _rotSpeed = 30f;
    private void Update()
    {
        if(isPossessed) { return; }
        transform.Rotate(0, _rotSpeed * Time.deltaTime, 0);
    }
    public abstract void Use();

    public void EquipTool()
    {
        isPossessed = true;
        GameObject _backpack = GameObject.FindGameObjectWithTag("Backpack");
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        Inventory _playerInventory = _player.GetComponent<Inventory>();
        
        transform.SetParent(_backpack.transform);

        transform.localPosition = Vector3.right;
        transform.localRotation = Quaternion.identity;
        transform.Rotate(_player.transform.forward);

        _playerInventory.AddToBackpack(this);
        transform.localPosition -= Vector3.right * 0.2f;
    }
}