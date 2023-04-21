using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemies : MonoBehaviour
{
    [SerializeField] private List<GameObject> _ennemies;

    public List<GameObject> EnnemiesList
    {
        get
        {
            return _ennemies;
        }
    }
}
