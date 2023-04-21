using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void JoueurEntre(OuvrirPortes porte, GameObject joueur);
public delegate void JoueurSort(OuvrirPortes porte, GameObject joueur);

public class OuvrirPortes : MonoBehaviour
{
    public event JoueurEntre JoueurEntreHandler;
    public event JoueurSort JoueurSortHandler;

    private GameObject _porteGauche;
    private GameObject _porteDroite;

    public bool EstOuverte
    {
        private set;
        get;
    }

    private float vitesse = 2.0f;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _porteDroite = GameObject.Find($"/===== Portes =====/{gameObject.name}/Porte droite");
        _porteGauche = GameObject.Find($"/===== Portes =====/{gameObject.name}/Porte gauche");
        EstOuverte = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            StopAllCoroutines();
            StartCoroutine(OuvrirDroite(_porteDroite.transform, 6.0f));
            StartCoroutine(OuvrirGauche(_porteGauche.transform, -6.0f));
            EstOuverte = true;
        }

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            StopAllCoroutines();
            EstOuverte = false;
        }
    }

    public void Ouvrir()
    {
        if (!EstOuverte)
        {
            StartCoroutine(OuvrirDroite(_porteDroite.transform, 6.0f));
            StartCoroutine(OuvrirGauche(_porteGauche.transform, -6.0f));
            EstOuverte = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Joueur est entré dans zone");
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            JoueurEntreHandler?.Invoke(this, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("Joueur est sorti de la zone");
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            JoueurSortHandler?.Invoke(this, other.gameObject);
        }
    }

    private IEnumerator OuvrirDroite(Transform transformPorte, float positionX)
    {
        bool termine = false;

        while (!termine)
        {

            if (transformPorte.localPosition.x < positionX)
            {
                transformPorte.localPosition += Vector3.right * vitesse * Time.deltaTime;
            }

            if (transformPorte.localPosition.x >= positionX)
            {
                termine = true;
            }
            yield return new WaitForEndOfFrame();
        }

        transformPorte.localPosition = new Vector3(positionX, transformPorte.localPosition.y, transformPorte.localPosition.z);

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator OuvrirGauche(Transform transformPorte, float positionX)
    {
        bool termine = false;

        while (!termine)
        {

            if (transformPorte.localPosition.x > positionX)
            {
                transformPorte.localPosition += Vector3.left * vitesse * Time.deltaTime;
            }

            if (transformPorte.localPosition.x <= positionX)
            {
                termine = true;
            }
            yield return new WaitForEndOfFrame();
        }

        transformPorte.localPosition = new Vector3(positionX, transformPorte.localPosition.y, transformPorte.localPosition.z);

        yield return new WaitForEndOfFrame();
    }
}
