using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Image _buffImage;

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

            if(applyForPlayer) 
            { 
                BuffDebuff _effect = ApplyEffect(_player);
                _buffImage.gameObject.SetActive(true);
                _buffImage.sprite = _effect.effectIcon;
            }
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

    private BuffDebuff ApplyEffect(GameObject _target)
    {
        BuffDebuff _effect = _allEffects[Random.Range(0, _allEffects.Count)];
        print($"Applying {_effect.effectName} to {_target.name}");

        _effect.Apply(_target);

        if (_effect.effectDuration > 0)
        {
            StartCoroutine(RemoveAfterDelay(_effect, _target, _effect.effectDuration));
        }
        return _effect;
    }

    private IEnumerator RemoveAfterDelay(BuffDebuff _effect, GameObject target, int delay)
    {
        yield return new WaitForSeconds(delay);
        _effect.Remove(target);
        print($"Removing {_effect.effectName} from {target.name}");
    }
    private IEnumerator RemoveBuffIcon(BuffDebuff _effect)
    {
        yield return new WaitForSeconds(_effect.effectDuration);
        _buffImage.gameObject.SetActive(false);
    }
}
