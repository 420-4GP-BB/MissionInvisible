using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnnemi : MonoBehaviour
{
    private GameObject _sujet;
    private Vector3 _anciennePositionSujet;
    public GameObject Sujet
    {
        set
        {
            _sujet = value;
            CentrerSur(_sujet.transform);
            _anciennePositionSujet = _sujet.transform.position;
        }
        get
        {
            return _sujet;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_sujet != null)
        {
            Vector3 delta = _sujet.transform.position - _anciennePositionSujet;
             transform.position += delta;
            _anciennePositionSujet = _sujet.transform.position;
        }
        else
        {
            Debug.Log("Aucun ennemi à suivre");
        }
    }

    public void CentrerSur(Transform target)
    {

        // Trouve le centre de l'écran
        Vector3 pointCentre = Utilitaires.TrouverPointCentreEcran(Vector3.zero);

        // Je veux la distance le pointCentre et le target
        Vector3 distance = target.position - pointCentre;

        // On déplace la caméra en conséquence
        transform.position += distance;
    }
}
