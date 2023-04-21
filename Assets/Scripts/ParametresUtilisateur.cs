using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametresUtilisateur 
{

    private static ParametresUtilisateur _instance = new ParametresUtilisateur();
    public static ParametresUtilisateur Instance
    {
        get
        {
            return _instance;
        }
    }

    private ParametresUtilisateur()
    {
        NiveauSecurite = 10;
        NomJoueurBleu = "Bob avec cheveux";
        PointsActionBleu = 8;
        NomJoueurVert = "Bob sans cheveux";
        PointsActionVert = 8;
    }


    public int NiveauSecurite { get; set; }
    public string NomJoueurBleu { get; set; }
    public string NomJoueurVert { get; set; }
    public int PointsActionBleu { get; set; }
    public int PointsActionVert { get; set; }
}
