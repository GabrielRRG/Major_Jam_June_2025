using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Shapeshifting : MonoBehaviour
{
    [Header("Player")] 
    [SerializeField] private GameObject _playerModel;
    
    [Header("Animal Forms")]
    [SerializeField] private List<AnimalFormData> _animalForms;

    [Header("Settings")]
    [SerializeField] private Transform _spawnPostion;
    [SerializeField] private float _transformsChance = 10f;
    [SerializeField] private float _timeBetweenTransforms = 1f;
    [SerializeField] private float _transformationDuration = 20f;

    private GameObject _currentAnimalInstance;
    private bool _isTransformed = false;
    private float _timer;

    private void Start()
    {
        StartCoroutine(TransformationCycle());
    }

    private IEnumerator TransformationCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeBetweenTransforms);
            TransformIntoAnimal();
            if (!_isTransformed) continue;

            yield return new WaitForSeconds(_transformationDuration);
            RevertToOriginal();
        }
    }

    private void TransformIntoAnimal()
    {
        if (_isTransformed || _animalForms.Count == 0) return;

        if(Random.Range(0, 101) > _transformsChance) return;
        
        _isTransformed = true;
        
        var selectedForm = _animalForms[Random.Range(0, _animalForms.Count)];
        
        _currentAnimalInstance = Instantiate(
            selectedForm.prefab,
            _spawnPostion.position,
            transform.rotation,
            transform
        );
        
        _playerModel.SetActive(false);
    }

    private void RevertToOriginal()
    {
        if (!_isTransformed) return;

        _isTransformed = false;
        
        _playerModel.SetActive(true);
        
        if (_currentAnimalInstance != null)
            Destroy(_currentAnimalInstance);
        
    }
}
