using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class EtatPersonnage
{
    public LogiqueAgent FSM
    {
        private set;
        get;
    }
    
    public Animator Animateur
    {
        private set;
        get;
    }

    public NavMeshAgent AgentJoueur
    {
        private set;
        get;
    }

    public EtatPersonnage(GameObject obj)
    {
        FSM = obj.GetComponent<LogiqueAgent>();
        Animateur = obj.GetComponent<Animator>();
        AgentJoueur = obj.GetComponent<NavMeshAgent>();
    }

    public abstract void Enter();
    public abstract void Handle(float deltaTime);
    public abstract void Leave();

}
