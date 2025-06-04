using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private List<BuffDebuff> _allEffects;
    private GameObject _player;
    [SerializeField] private int _minDuration = 4;
    [SerializeField] private int _maxDuration = 10;


    [SerializeField] private bool applyForPlayer;
    [SerializeField] private bool applyForEnemy;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ApplyRandomEffects());
    }

    IEnumerator ApplyRandomEffects()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(_minDuration,_maxDuration));

            if(_player.GetComponent<Shapeshifting>().isTransformed) { continue; }

            if(applyForPlayer) { ApplyEffect(_player); }
            if(applyForEnemy)
            {
                GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if(_enemies != null)
                {
                    foreach(GameObject _enemy in _enemies)
                    {
                        ApplyEffect(_enemy);
                    }
                }
            }
        }
    }

    private void ApplyEffect(GameObject _target)
    {
        BuffDebuff _effect = _allEffects[Random.Range(0, _allEffects.Count)];
        print($"Applying {_effect.effectName} to {_target.name}");
        _effect.Apply(_target);

        if (_effect.effectDuration > 0)
        {
            StartCoroutine(RemoveAfterDelay(_effect, _target, _effect.effectDuration));
        }
    }

    private IEnumerator RemoveAfterDelay(BuffDebuff _effect, GameObject target, int delay)
    {
        yield return new WaitForSeconds(delay);
        _effect.Remove(target);
        print($"Removing {_effect.effectName} from {target.name}");
    }
}
