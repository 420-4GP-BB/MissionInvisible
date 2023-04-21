using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionJoueurs : MonoBehaviour
{
    [SerializeField] private int distance;

    
    private GameObject[] _players;

    // Start is called before the first frame update
    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("PlayerCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        ChercherJoueurs();

    }

    private void ChercherJoueurs()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Ignore Raycast");

        foreach (GameObject obj in _players)
        {
            LogiqueAgent infos = obj.GetComponent<LogiqueAgent>();
            Vector3 direction = obj.transform.position - transform.position ;

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, direction, out hit, distance, layerMask))
            {
                

                if (hit.collider.gameObject == obj && infos.EstActif)
                {
                    float angle = Vector3.Angle(transform.forward, direction);
                    if (angle <= 60.0f)
                    {
                        infos.AssommerJoueur();
                        infos.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }
        }
    }
}
