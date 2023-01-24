using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonne : MonoBehaviour
{
    [SerializeField] GameObject licht;
    [SerializeField] GameObject sonne;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sonne.activeInHierarchy)
        {
            licht.SetActive(false);
        }
    }
}
