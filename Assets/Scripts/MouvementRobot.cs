using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouvementRobot : MonoBehaviour, IEnnemi
{
    [SerializeField] private float _actionPoints;
    [SerializeField] private Transform[] _waypoints;
    private int _indiceWaypoint;

    private Animator _animator;

    private NavMeshAgent _agent;
     
    private float _maxActionPoints;
    private Vector3 _lastPosition;
    private bool _enMouvement;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _maxActionPoints = _actionPoints;
        _indiceWaypoint = 0;
        _lastPosition = transform.position;
        _agent.SetDestination(_waypoints[_indiceWaypoint].position);
        _enMouvement = false;
        _agent.isStopped = true;

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enMouvement)
        {
            float distanceParcourue = Vector3.Distance(_lastPosition, transform.position);
            _actionPoints -= distanceParcourue;
            _lastPosition = transform.position;
            if (_actionPoints <= 0)
            {
                _enMouvement = false;
                _agent.isStopped = true;
                _animator.SetBool("isWalking", false);
            }
        }

        VerifierPointPatrouille();

    }

    private void VerifierPointPatrouille()
    {
        if (! _agent.hasPath && _agent.remainingDistance < 0.5f)
        {
            _indiceWaypoint = (_indiceWaypoint + 1) % _waypoints.Length;
            _agent.SetDestination(_waypoints[_indiceWaypoint].position);
        }
    }

    public void ActiverEnnemi()
    {
        _enMouvement = true;
        _agent.isStopped = false;
        _actionPoints = _maxActionPoints;
        _lastPosition = transform.position;
        _animator.SetBool("isWalking", true);
    }

    public bool EnnemiEstActif()
    {
        return _enMouvement;
    }
}
