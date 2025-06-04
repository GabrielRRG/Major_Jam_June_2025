using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Shapeshifting : MonoBehaviour
{
    [Header("Player")] 
    [SerializeField] private GameObject _playerModel;
    
    [Header("Animal Forms")]
    [SerializeField] private List<AnimalFormData> _animalForms;

    [Header("Timing")]
    [SerializeField] private float _timeBetweenTransforms = 10f;
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

            yield return new WaitForSeconds(_transformationDuration);
            RevertToOriginal();
        }
    }

    private void TransformIntoAnimal()
    {
        if (_isTransformed || _animalForms.Count == 0) return;

        _isTransformed = true;
        
        var selectedForm = _animalForms[Random.Range(0, _animalForms.Count)];
        _playerModel.SetActive(false);
        
        _currentAnimalInstance = Instantiate(
            selectedForm.prefab,
            transform.position,
            transform.rotation,
            transform
        );
    }

    private void RevertToOriginal()
    {
        if (!_isTransformed) return;

        _isTransformed = false;
        
        if (_currentAnimalInstance != null)
            Destroy(_currentAnimalInstance);
        
        _playerModel.SetActive(true);
    }
}
