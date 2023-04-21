using System;
using UnityEngine;

public class CharacterMoving : EtatPersonnage
{
    private float facteurDistance = 2;
    private Vector3 _lastPosition;

    public CharacterMoving(GameObject obj) : base(obj)
    {
    }

    public override void Enter()
    {
        Animateur.SetBool("Moving", true);
        _lastPosition = FSM.gameObject.transform.position;
    }

    public override void Handle(float deltaTime)
    {
        if (AgentJoueur.pathPending || ! AgentJoueur.hasPath)
        {
            Debug.Log("Path pending");
            return;
        }

        if (FSM.ActionPoints <= 0 ||
            AgentJoueur.hasPath && AgentJoueur.remainingDistance <= 0.1f)
        {
            FSM.ChangerEtat(FSM.Idle);
        }
        else
        {
            Vector3 actualPosition = FSM.gameObject.transform.position;
            float distance = Vector3.Distance(_lastPosition, actualPosition);
            _lastPosition = actualPosition;

            FSM.ActionPoints -= distance / facteurDistance;
        }
    }

    public override void Leave()
    {
        Animateur.SetBool("Moving", false);
    }
}
