using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    public float duracionEscudo = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ActivarEscudo()
    {
        gameObject.SetActive(true);
        Invoke("DesactivarEscudo", duracionEscudo);
    }

    public void DesactivarEscudo()
    { 
        gameObject.SetActive(false);
    }
}
