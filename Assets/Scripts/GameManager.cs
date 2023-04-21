using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/*
 * Cette classe déplace la caméra et gère
 * les événements souris.
 *
 * Elle sait également à qui le tour.
 *
 * Par exemple, si c'est le tour de l'ordinateur,
 * alors elle ne permettra pas de cliquer.
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _actionOuvrirPorte;
    [SerializeField] private Button _actionDesactiverRobot;
    [SerializeField] private TMP_Text _txtNiveauSecurite;

    private GameObject[] _walls;

    private bool _tourJoueurs;    // Pour savoir à qui est le tour.    
    private GestionnairePersonnages _joueurs;    // Les joueurs sont associés au GameManager comme cette classe
    private Ennemies _ennemies;  // Les objets ennemis
    private Camera _camera;

    private int _niveauSecurite;
    private int _niveauMax;

    private void Awake()
    {
        _walls = GameObject.FindGameObjectsWithTag("Murs");
        _tourJoueurs = true;
        _joueurs = GetComponent<GestionnairePersonnages>();
        _ennemies = GetComponent<Ennemies>();
        _camera = Camera.main;
        _camera.GetComponent<CameraEnnemi>().enabled = false;
        _camera.GetComponent<CameraJoueurs>().enabled = true;
        _niveauSecurite = 1 ;
        _niveauMax = ParametresUtilisateur.Instance.NiveauSecurite;


    }

    public void Start()
    {
        // On s'enregistre auprès des portes
        foreach (OuvrirPortes porte in FindObjectsOfType<OuvrirPortes>())
        {
            porte.JoueurEntreHandler += JoueurEntre;
            porte.JoueurSortHandler += JoueurSort;
        }
    }

    void Update()
    {
        // Si ce n'est pas le tour des joueurs. On accepte aucun événement souris
        if (Input.GetMouseButtonDown(0) && _tourJoueurs)
        {
            // Si  on clique sur le UI. Il ne faut rien faire.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Collider colliderClique = Utilitaires.TrouverColliderClique(Input.mousePosition);

            if (colliderClique != null)
            {
                // On sélectionne un joueur
                if (colliderClique?.gameObject.tag == "PlayerCharacter")
                {
                    _joueurs.JoueurActif = colliderClique.gameObject;
                }

                if (Utilitaires.ApproximerEgal(colliderClique.gameObject.transform.position.y, 0.0f, 0.0001f))
                {
                    LogiqueAgent mouvement = _joueurs.JoueurActif.GetComponent<LogiqueAgent>();
                    Vector3? destination = Utilitaires.TrouverPointContact(Input.mousePosition, colliderClique);
                    if (destination != null)
                    {
                        print($"Destination: {destination.Value}");
                        mouvement.SetDestination(destination.Value);
                    }
                }
            }
        }
        // Ici, on téléporte le joueur. C'est un Cheat Code.
        else if (Input.GetMouseButtonDown(1) && _tourJoueurs)
        {
            Collider colliderClique = Utilitaires.TrouverColliderClique(Input.mousePosition);

            if (colliderClique != null)
            {
                // On sélectionne un joueur
                if (colliderClique?.gameObject.tag == "PlayerCharacter")
                {
                    _joueurs.JoueurActif = colliderClique.gameObject;
                }

                // On bouge le joueur... pourrait être fait avec le bouton de droite
                if (Utilitaires.ApproximerEgal(colliderClique.gameObject.transform.position.y, 0.0f, 0.0001f))
                {

                    Vector3? destination = Utilitaires.TrouverPointContact(Input.mousePosition, colliderClique);
                    if (destination != null)
                    {
                        _joueurs.JoueurActif.GetComponent<LogiqueAgent>().Teleporter(destination.Value);
                    }
                }
            }
        }
    }

    public void OnGUI()
    {
        if (_joueurs.JoueurActif != null)
        {
            LogiqueAgent infosJoueur = _joueurs.JoueurActif.GetComponent<LogiqueAgent>();
            _actionOuvrirPorte.interactable = infosJoueur.Porte != null && !infosJoueur.Porte.EstOuverte &&
                                              infosJoueur.ActionPoints >= 2.0f;

            _actionDesactiverRobot.interactable = JoueurDerriereRobot(_joueurs.JoueurActif) != null &&
                                                  infosJoueur.ActionPoints >= 4.0f;
        }

        _txtNiveauSecurite.text = $"{_niveauSecurite}/{_niveauMax}";
    }

    public GameObject JoueurDerriereRobot(GameObject joueur)
    {
        // On doit vérifier si le joueur actif est derrière un robot (ennemi).
        // Il doit être dans un espace de 1.5f derrière le robot dans un angle de 40 degrés

        GameObject robotTrouve = null;
        foreach (GameObject robot in _ennemies.EnnemiesList)
        {
            Vector3 directionRobot = joueur.transform.position - robot.transform.position;
            float angle = Vector3.Angle(directionRobot, robot.transform.forward * -1.0f);  // On inverse la direction du robot car
                                                                                           // on doit être derrière le robot
            float distance = Vector3.Distance(robot.transform.position, joueur.transform.position);
            // print("Distance: " + distance);
            if (angle <= 40.0f && distance <= 1.5f)
            {
                robotTrouve = robot;
                break;
            }
        }
        return robotTrouve;
    }

    private void JoueurEntre(OuvrirPortes porte, GameObject joueur)
    {
        if (joueur.CompareTag("PlayerCharacter"))
        {
            joueur.GetComponent<LogiqueAgent>().Porte = porte;
        }
    }

    private void JoueurSort(OuvrirPortes porte, GameObject joueur)
    {
        if (joueur.CompareTag("PlayerCharacter"))
        {
            joueur.GetComponent<LogiqueAgent>().Porte = null;
        }
    }

    public void NouveauTour()
    {
        _niveauSecurite++;
        if (_niveauSecurite > _niveauMax)
        {
            SceneManager.LoadScene("Defaite");
        }
        else
        {
            StartCoroutine(FaireTourOrdi());
        }
    }

    private IEnumerator FaireTourOrdi()
    {
        ActivatePlayers(false);

        foreach (GameObject ennemi in _ennemies.EnnemiesList)
        {
            IEnnemi ennemiInterface = ennemi.GetComponent<IEnnemi>();
            ennemiInterface.ActiverEnnemi();
            _camera.GetComponent<CameraEnnemi>().Sujet = ennemi;
            while (ennemiInterface.EnnemiEstActif())
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(2.0f);
        }
        ActivatePlayers(true);
        yield return new WaitForEndOfFrame();
    }

    private void ActivatePlayers(bool actif)
    {
        if (actif)
        {
            _tourJoueurs = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _camera.GetComponent<CameraEnnemi>().enabled = false;
            _camera.GetComponent<CameraJoueurs>().enabled = true;
            _joueurs.SelectCharacter(0);
            foreach (GameObject joueur in _joueurs.PersonnagesList)
            {
                LogiqueAgent infos = joueur.GetComponent<LogiqueAgent>();
                infos.NouveauTour();
            }
        }
        else
        {
            _tourJoueurs = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _camera.GetComponent<CameraEnnemi>().enabled = true;
            _camera.GetComponent<CameraJoueurs>().enabled = false;
        }
    }
    private void ChangeWallsLayer(int num)
    {
        foreach (GameObject wall in _walls)
        {
            wall.layer = num;
            foreach (Transform  t in wall.GetComponentInChildren<Transform>())
            {
                t.gameObject.layer = num;
            }
        }
    }

    public void DesactiverRobot()
    {
        GameObject robot = JoueurDerriereRobot(_joueurs.JoueurActif);
        StartCoroutine(DesactiverRobotCoroutine(robot));
    }

    public IEnumerator DesactiverRobotCoroutine(GameObject robot)
    {
        robot.GetComponent<Animator>().SetBool("isInactive", true);
        _joueurs.JoueurActif.GetComponent<LogiqueAgent>().RetirerPointsAction(4.0f);
        yield return new WaitForSeconds(2.0f);
        _ennemies.EnnemiesList.Remove(robot);
        Destroy(robot);
    }

    public void AllerAuMenu()
    {
        if (_tourJoueurs)
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
