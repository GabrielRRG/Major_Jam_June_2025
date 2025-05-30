using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;
        private void Update()
    {
        _agent.SetDestination(_player.position);
    }
}
