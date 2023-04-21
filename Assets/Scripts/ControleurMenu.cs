using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ControleurMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldNomJoueurBleu;
    [SerializeField] private TMP_InputField _inputFieldNomJoueurVert;
    [SerializeField] private TMP_InputField _inputFieldNiveauSecurite;
    [SerializeField] private TMP_InputField _inputFieldPointsActionBleu;
    [SerializeField] private TMP_InputField _inputFieldPointsActionVert;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _inputFieldNiveauSecurite.text = ParametresUtilisateur.Instance.NiveauSecurite.ToString();
        _inputFieldNomJoueurBleu.text = ParametresUtilisateur.Instance.NomJoueurBleu;
        _inputFieldPointsActionBleu.text = ParametresUtilisateur.Instance.PointsActionBleu.ToString();
        _inputFieldNomJoueurVert.text = ParametresUtilisateur.Instance.NomJoueurVert;
        _inputFieldPointsActionVert.text = ParametresUtilisateur.Instance.PointsActionVert.ToString();
    }

    public void NouvellePartie()
    {
        ParametresUtilisateur.Instance.NiveauSecurite = int.Parse(_inputFieldNiveauSecurite.text);
        ParametresUtilisateur.Instance.NomJoueurBleu = _inputFieldNomJoueurBleu.text;
        ParametresUtilisateur.Instance.PointsActionBleu = int.Parse(_inputFieldPointsActionBleu.text);
        ParametresUtilisateur.Instance.NomJoueurVert = _inputFieldNomJoueurVert.text;
        ParametresUtilisateur.Instance.PointsActionVert = int.Parse(_inputFieldPointsActionVert.text);
        SceneManager.LoadScene("PremierNiveau");
    }
    
    public void Quitter()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

}
