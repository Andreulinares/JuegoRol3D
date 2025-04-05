using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoMelee : MonoBehaviour
{
    public bool tieneTipo = false; 
    public AsignarTipo.TipoElemental tipoActual;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObtenerTipoElemental(AsignarTipo.TipoElemental tipo)
    {
        tipoActual = tipo;
        tieneTipo = true;

        // Aquí puedes añadir efectos visuales, cambios de color, partículas, etc.
        Debug.Log("Enemigo melee ha recibido tipo elemental: " + tipo);
    }

    public void MoverHaciaElemental(Vector3 enemigoElemental)
    {
        if (agent != null)
        {
            agent.SetDestination(enemigoElemental);
            Debug.Log("Enemigo melee se está moviendo hacia el elemental");
        }
    }
}
