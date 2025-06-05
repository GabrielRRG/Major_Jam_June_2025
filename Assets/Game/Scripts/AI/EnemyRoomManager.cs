using System.Collections;
using UnityEngine;

public class EnemyRoomManager : MonoBehaviour
{
    public Vector3 alarmPosition;
    public bool alarm = false;
    [SerializeField] private EnemyAIBase[] _enemies;
    [SerializeField] private float _alarmTime;
    private Coroutine _alarmRoutine;

    public void SetAlarm(Vector3 currentPlayerPosition)
    {
        alarm = true;
        if(_alarmRoutine != null) StopCoroutine(_alarmRoutine);
        _alarmRoutine = StartCoroutine(Alarm());
        for (int i = 0; i < _enemies.Length; i++)
        {
            alarmPosition = currentPlayerPosition;
        }
    }

    private IEnumerator Alarm()
    {
        yield return new WaitForSeconds(_alarmTime);
        alarm = false;
    }
}