using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AffichageResultat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttendreEtMenu());
    }

    private IEnumerator AttendreEtMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Menu");
    }

}
