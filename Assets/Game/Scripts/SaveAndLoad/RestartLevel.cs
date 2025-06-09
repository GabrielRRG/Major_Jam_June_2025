using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    public void Restart()
    {
        SaveLoad.instance.RestartLevel();
    }
}