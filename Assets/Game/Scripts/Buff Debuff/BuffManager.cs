using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine.UI;
using Unity.VisualScripting;

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

            if (_player == null)
            {
                break;
            }
            
            if(_player.GetComponent<Shapeshifting>().isTransformed) { continue; }

            if(applyForPlayer) 
            {
                BuffDebuff _effect = GetRandomEffect();
                ApplyEffect(_effect, _player);
                _buffImage.gameObject.SetActive(true);
                _buffImage.sprite = _effect.effectIcon;
                StartCoroutine(RemoveBuffIcon(_effect));
            }
            if(applyForEnemy)
            {
                GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if(_enemies != null)
                {
                    foreach (GameObject _enemy in _enemies)
                    {
                        BuffDebuff _effect = GetRandomEffect();
                        if(_effect.name == "Slowpoke" || _effect.name == "Speedster") { continue; }
                        ApplyEffect(_effect,_enemy);
                        Image iconImage = _enemy.GetComponentInChildren<Image>(true);
                        if(iconImage)
                        {
                            iconImage.gameObject.SetActive(true);
                            iconImage.sprite = _effect.effectIcon;
                            StartCoroutine(RemoveEnemyBuffIcon(_effect,iconImage));
                        }
                    }
                }
            }
        }
    }
    private BuffDebuff GetRandomEffect()
    {
        return _allEffects[Random.Range(0, _allEffects.Count)];
    }
    private void ApplyEffect(BuffDebuff _effect,GameObject _target)
    {
        print($"Applying {_effect.effectName} to {_target.name}");

        _effect.Apply(_target);
        if (_target.CompareTag("Player"))
        {
            if(_target.GetComponentInChildren<Gun>()) _target.GetComponentInChildren<Gun>().ShowGunUI();
            _target.GetComponent<CharacterHealth>().UpdateSlider();
        }

        if (_effect.effectDuration > 0)
        {
            StartCoroutine(RemoveAfterDelay(_effect, _target, _effect.effectDuration));
        }
    }

    private IEnumerator RemoveAfterDelay(BuffDebuff _effect, GameObject target, int delay)
    {
        yield return new WaitForSeconds(delay);
        if (_player == null) yield break;
            _effect.Remove(target);
        if (target.CompareTag("Player"))
        {
            //if(target.GetComponentInChildren<Gun>()) target.GetComponentInChildren<Gun>().ShowGunUI();
            target.GetComponent<CharacterHealth>().UpdateSlider();
        }
        print($"Removing {_effect.effectName} from {target.name}");
    }
    private IEnumerator RemoveEnemyBuffIcon(BuffDebuff _effect, Image iconImage)
    {
        yield return new WaitForSeconds(_effect.effectDuration);
        iconImage.gameObject.SetActive(false);
    }
    private IEnumerator RemoveBuffIcon(BuffDebuff _effect)
    {
        yield return new WaitForSeconds(_effect.effectDuration);
        _buffImage.gameObject.SetActive(false);
    }
}
