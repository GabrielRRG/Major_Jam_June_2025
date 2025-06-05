using UnityEngine;

[CreateAssetMenu(menuName = "Transformation/Animal Form")]
public sealed class AnimalFormData : ScriptableObject
{
    public GameObject prefab;
    public int maxHP;
}