using System;
using UnityEngine;

/**
 * Classe utilitaire pour les fonctions réutilisables
 */
public class Utilitaires
{

    public static Collider? TrouverColliderClique(Vector3 positionSouris)
    {
        Collider colliderClique = null;
        // Trouver le lien avec la caméra
        Ray ray = Camera.main.ScreenPointToRay(positionSouris);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            colliderClique = hit.collider;
        }
        return colliderClique;
    }


    public static Vector3 TrouverPointCentreEcran(Vector3 niveauContact)
    {
        Vector3 milieuEcran = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 pointContact = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(milieuEcran);
        Plane newPlane = new Plane(Vector3.up, niveauContact);
        float distance = 0;

        newPlane.Raycast(ray, out distance);
        pointContact = ray.GetPoint(distance);
        return pointContact;
    }

    public static Vector3? TrouverPointContact(Vector3 positionSouris, Collider colliderClique)
    {
        Vector3? pointContact = null;

        // Trouver le lien avec la caméra
        Ray ray = Camera.main.ScreenPointToRay(positionSouris);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (colliderClique == hit.collider)
            {
                pointContact = hit.point;
            }
        }

        return pointContact;
    }

    public static bool ApproximerEgal(float a, float b, float epsilon)
    {
        return a >= b - epsilon && a <= b + epsilon;
    }
}
