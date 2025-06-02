using UnityEngine;

[CreateAssetMenu(menuName = "Transformation/Animal Form")]
public sealed class AnimalFormData : ScriptableObject
{
    public GameObject animalPrefab;
    public string animalName;
}