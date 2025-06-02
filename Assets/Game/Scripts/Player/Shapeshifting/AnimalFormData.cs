using UnityEngine;

[CreateAssetMenu(menuName = "Transformation/Animal Form")]
public class AnimalFormData : ScriptableObject
{
    public GameObject animalPrefab;
    public string animalName;
}