using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private NavMeshAgent _agent;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

    }


    void Update()
    {
        if (_player)
        {
            _agent.SetDestination(_player.position);
        }
    }
    
}