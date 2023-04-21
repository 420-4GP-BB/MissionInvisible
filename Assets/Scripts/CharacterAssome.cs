using System;
using UnityEngine;


public class CharacterAssome : EtatPersonnage
{
    public CharacterAssome(GameObject obj) : base(obj)
    {
    }

    public override void Enter()
    {
        // Tout est faux dans Idle
        Animateur.SetBool("Stunned", true);
        AgentJoueur.isStopped = true;
    }

    public override void Handle(float deltaTime)
    {
    }

    public override void Leave()
    {
    }
}

