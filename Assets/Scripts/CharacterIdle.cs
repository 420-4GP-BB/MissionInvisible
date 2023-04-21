using System;
using UnityEngine;

public class CharacterIdle : EtatPersonnage
{

    public CharacterIdle(GameObject obj) : base(obj)
    {
    }

    public override void Enter()
    {
        // Tout est faux dans Idle
        Animateur.SetBool("Stunned", false);
        Animateur.SetBool("Moving", false);
        Animateur.SetBool("Recovered", false);
        AgentJoueur.isStopped = true;
    }

    public override void Handle(float deltaTime)
    {
    }

    public override void Leave()
    {
        AgentJoueur.isStopped = false;
    }
}
