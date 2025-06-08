using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class BuffManager : MonoBehaviour
{
    [SerializeField] private Image _buffImagePrefab;

    [SerializeField] private List<BuffDebuff> _allEffects;
    private GameObject _player;
    [SerializeField] private int _minDuration = 4;
    [SerializeField] private int _maxDuration = 10;
    [SerializeField] private int _maxEffects = 1;

    [SerializeField] private bool applyForPlayer;
    [SerializeField] private bool applyForEnemy;


    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ApplyRandomEffects());
    }

    IEnumerator ApplyRandomEffects()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minDuration, _maxDuration));

            if (_player == null)
            {
                break;
            }

            if (_player.GetComponent<Shapeshifting>().isTransformed)
            {
                continue;
            }

            if (applyForPlayer && _player.GetComponent<CharacterHealth>().buffsFolder.childCount < _maxEffects)
            {
                BuffDebuff _effect = GetRandomEffect();
                ApplyEffect(_effect, _player);
                Image currentBuffImage = Instantiate(_buffImagePrefab, _player.GetComponent<CharacterHealth>().buffsFolder);
                currentBuffImage.sprite = _effect.effectIcon;
                currentBuffImage.GetComponentInChildren<Image>().sprite = _effect.effectIcon;
                StartCoroutine(RemoveBuffIcon(_effect, currentBuffImage.gameObject));
            }

            if (applyForEnemy)
            {
                GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if (_enemies != null)
                {
                    foreach (GameObject _enemy in _enemies)
                    {
                        if (_enemy.GetComponent<CharacterHealth>().buffsFolder.childCount >= _maxEffects) continue;
                        BuffDebuff _effect = GetRandomEffect();
                        if (_effect.name == "Slowpoke" || _effect.name == "Speedster")
                        {
                            continue;
                        }

                        ApplyEffect(_effect, _enemy);
                        Image currentBuffImage = Instantiate(_buffImagePrefab, _enemy.GetComponent<CharacterHealth>().buffsFolder);
                        currentBuffImage.sprite = _effect.effectIcon;
                        currentBuffImage.GetComponentInChildren<Image>().sprite = _effect.effectIcon;
                        StartCoroutine(RemoveEnemyBuffIcon(_effect, currentBuffImage));
                    }
                }
            }
        }
    }

    private BuffDebuff GetRandomEffect()
    {
        return _allEffects[Random.Range(0, _allEffects.Count)];
    }

    private void ApplyEffect(BuffDebuff _effect, GameObject _target)
    {
        print($"Applying {_effect.effectName} to {_target.name}");

        _effect.Apply(_target);
        if (_target.CompareTag("Player"))
        {
            if (_target.GetComponentInChildren<Gun>()) _target.GetComponentInChildren<Gun>().ShowGunUI();
        }
        _target.GetComponent<CharacterHealth>().UpdateSlider();

        if (_effect.effectDuration > 0)
        {
            StartCoroutine(RemoveAfterDelay(_effect, _target, _effect.effectDuration));
        }
    }

    private IEnumerator RemoveAfterDelay(BuffDebuff _effect, GameObject target, int delay)
    {
        yield return new WaitForSeconds(delay);
        if (_player == null) yield break;
        if (target == null) yield break;
        if(target.CompareTag("Player")) yield return new WaitUntil(() => !_player.GetComponent<Shapeshifting>().isTransformed);
        Debug.Log("Transformation ended");
        _effect.Remove(target);
        if (target.CompareTag("Player"))
        {
            if (target.GetComponentInChildren<Gun>()) target.GetComponentInChildren<Gun>().ShowGunUI();
        }

        print($"Removing {_effect.effectName} from {target.name}");
            target.GetComponent<CharacterHealth>().UpdateSlider();
    }

    private IEnumerator RemoveEnemyBuffIcon(BuffDebuff _effect, Image iconImage)
    {
        yield return new WaitForSeconds(_effect.effectDuration);
        if (iconImage != null) Destroy(iconImage.gameObject);
    }

    private IEnumerator RemoveBuffIcon(BuffDebuff _effect, GameObject icon)
    {
        yield return new WaitForSeconds(_effect.effectDuration);
        yield return new WaitUntil(() => !_player.GetComponent<Shapeshifting>().isTransformed);
        Destroy(icon);
    }
}