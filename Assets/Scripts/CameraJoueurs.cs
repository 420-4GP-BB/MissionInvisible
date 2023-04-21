using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraJoueurs : MonoBehaviour
{
    [SerializeField] private float _vitesse = 50.0f;
    [SerializeField] private float _vitesseScroll = 500.0f;
    [SerializeField] private float _minFieldOfView = 40;
    [SerializeField] private float _maxFieldOfView = 80;


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * _vitesse * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * _vitesse * Time.deltaTime;

        // modifiedForward ignore l'axe des y
        Vector3 modifiedForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Vector3 deplacement = transform.right * horizontal + modifiedForward * vertical;
        transform.position += deplacement;

        // Le déplacement de la roulette de la souris
        float mouse = Input.GetAxis("Mouse ScrollWheel");
        float delta = mouse * _vitesseScroll * Time.deltaTime;
        float newFieldOfView = Camera.main.fieldOfView - delta;
        newFieldOfView = Mathf.Clamp(newFieldOfView, _minFieldOfView, _maxFieldOfView);
        Camera.main.fieldOfView = newFieldOfView;

        //transform.position += deplacementSouris;
        //if (Camera.main.fieldOfView <= _minFieldOfView)
        //{
        //    // On doit replacer sur _minY
        //    Vector3 pointContact = Utilitaires.TrouverPointContact(Vector3.up * _minFieldOfView);
        //    transform.position = pointContact;
        //}
        //else if (Camera.main.fieldOfView >= _maxFieldOfView)
        //{
        //    // On doit replacer sur _maxY;
        //    Vector3 pointContact = Utilitaires.TrouverPointContact(Vector3.up * _maxFieldOfView);
        //    transform.position = pointContact;
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 pointContact = Utilitaires.TrouverPointCentreEcran(Vector3.zero);
            transform.RotateAround(pointContact, Vector3.up, 90);
            transform.LookAt(pointContact);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pointCentre = Utilitaires.TrouverPointCentreEcran(Vector3.zero);
            transform.RotateAround(pointCentre, Vector3.up, -90);
            transform.LookAt(pointCentre);
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
