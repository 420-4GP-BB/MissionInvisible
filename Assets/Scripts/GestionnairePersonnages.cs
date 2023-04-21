using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestionnairePersonnages : MonoBehaviour
{
    [SerializeField] private List<GameObject> _personnages;
    [SerializeField] private GameObject[] _zonesInformations;
    [SerializeField] private TMP_Text _nomJoueurActif;

    private List<TMP_Text> _texts;
    private List<Image> _images;

    private Image _imageJoueurActif;
    private bool _changementRequis = false;

    private int _indiceActif = 0;


    public List<GameObject> PersonnagesList
    {
        get
        {
            return _personnages;
        }
    }

    public GameObject? JoueurActif
    {
        set
        {
            _indiceActif = _personnages.IndexOf(value);
            _changementRequis = true;
        }

        get
        {
            if (_personnages.Count > 0)
            {
                return _personnages[_indiceActif];
            }
            else
            { 
                return null; 
            }
        }
    }

    public void Start()
    {
        foreach (GameObject player in _personnages)
        {
            // Quand il faut retirer un joueur, il faut se le faire dire...
            player.GetComponent<LogiqueAgent>().AgentEstAssomeHandler += RetirerAgent;

        }

        // PATCH: On sait qu'il y en a deux...
        LogiqueAgent infosBleu = _personnages[0].GetComponent<LogiqueAgent>();
        infosBleu.Nom = ParametresUtilisateur.Instance.NomJoueurBleu;
        infosBleu.ActionPointsMax = ParametresUtilisateur.Instance.PointsActionBleu;
        infosBleu.ActionPoints = infosBleu.ActionPointsMax;

        LogiqueAgent infosVert = _personnages[1].GetComponent<LogiqueAgent>();
        infosVert.Nom = ParametresUtilisateur.Instance.NomJoueurVert;
        infosVert.ActionPointsMax = ParametresUtilisateur.Instance.PointsActionVert;     // Faire une classe et la donner au GameObject
        infosVert.ActionPoints = infosVert.ActionPointsMax;



        _texts = new List<TMP_Text>(_personnages.Count);
        _images = new List<Image>(_personnages.Count);

        for (int i = 0; i < _personnages.Count; i++)
        {
            _texts.Add(GameObject.Find($"PointsAction ({i})").GetComponent<TMP_Text>());  // Un peu spaghetti
            _images.Add(GameObject.Find($"MiniJoueur ({i})").GetComponent<Image>());
        }

        _imageJoueurActif = GameObject.Find("ImageJoueurActif").GetComponent<Image>();


        SelectCharacter(_indiceActif);
        AfficherPersonnages();
        MettreAJourImage();
        MettreAJourInfos();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CentrerCameraSur(JoueurActif);
        }

    }

    public void OnGUI()
    {
        if (_changementRequis)
        {
            AfficherPersonnages();
            MettreAJourImage();
            _changementRequis = false;
        }
        MettreAJourInfos();
    }

    private void AfficherPersonnages()
    {
        int indice = 0;
        foreach (GameObject player in _personnages)
        {
            _zonesInformations[indice++].SetActive(true);
        }

        for (; indice < _zonesInformations.Length; indice++)
        {
            _zonesInformations[indice].SetActive(false);
        }
    }

    public void SelectCharacter(int indice)
    {
        if (indice < _personnages.Count)
        {
            JoueurActif = _personnages[indice];
            CentrerCameraSur(JoueurActif);
            _indiceActif = indice;
        } 
    }

    private void CentrerCameraSur(GameObject target)
    {
        CameraJoueurs mouvement = Camera.main.gameObject.GetComponent<CameraJoueurs>();
        mouvement.CentrerSur(target.transform);
    }

    public void CentrerSurJoueurActif()
    {
        CentrerCameraSur(JoueurActif);
    }

    private void MettreAJourImage()
    {
        LogiqueAgent infos = _personnages[_indiceActif].GetComponent<LogiqueAgent>();
        _imageJoueurActif.sprite = infos.ImageJoueur;
        _nomJoueurActif.text = infos.Nom;
    }

    private void MettreAJourInfos()
    {
        for (int i = 0; i < _personnages.Count; i++)
        {
            LogiqueAgent infos = _personnages[i].GetComponent<LogiqueAgent>();
            _texts[i].text = infos.ActionPoints.ToString("F");
            _images[i].sprite = infos.ImageJoueur;
        }
    }

    public void OuvrirPorte()
    {
        LogiqueAgent joueur = JoueurActif.GetComponent<LogiqueAgent>();
        OuvrirPortes porteAOuvrir = joueur.Porte;

        if (joueur.ActionPoints >= 2.0f)
        {
            porteAOuvrir.Ouvrir();
            joueur.ActionPoints -= 2.0f;
        }
    }

    public void RetirerAgent(LogiqueAgent ch)
    {
        _personnages.Remove(ch.gameObject);
        if (_personnages.Count == 0)
        {
            StartCoroutine(PartiePerdue());
        }
        else
        {
            JoueurActif = _personnages[0];
        }
    }

    private IEnumerator PartiePerdue()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Defaite");
    }
}
