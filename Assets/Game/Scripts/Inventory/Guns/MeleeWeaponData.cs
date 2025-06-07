using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class MeleeWeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Stats")]
    public int damage = 10;
}
