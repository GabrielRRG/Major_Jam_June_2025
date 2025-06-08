using UnityEngine;

public abstract class Tool : MonoBehaviour
{

    public string toolName;
    
    public bool isPossessed;

    private float _rotSpeed = 30f;
    public virtual void Update()
    {
        //transform.localRotation = Quaternion.Euler(-90, 0, 90);
        if(isPossessed) { return; }
        transform.Rotate(0, _rotSpeed * Time.deltaTime, 0);
    }
    public abstract void Use();

    public void EquipTool()
    {
        Inventory.instance.AddToBackpack(this);
    }

}