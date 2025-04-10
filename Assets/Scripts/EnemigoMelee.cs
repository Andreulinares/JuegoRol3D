using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemigoMelee : MonoBehaviour
{
    public bool tieneTipo = false; 
    public AsignarTipo.TipoElemental tipoActual;
    public ElementalBehaviour enemigoElemental;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!isAlive){
            Muerte();
        }else{
            EstoyConvertido = ComprobarConversion();
        }

        if (!EstoyConvertido){
            EstoySiendoLlamado = ComprobarLlamada();

            if(EstoySiendoLlamado){
                EstoyEnAreaInfluencia = ComprobarAreaConversion();
                if(EstoyEnAreaInfluencia){
                    if (!tieneTipo){
                        ObtenerTipoElemental((AsignarTipo.TipoElemental)Random.Range(0, System.Enum.GetValues(typeof(AsignarTipo.TipoElemental)).Length));
                    }else{
                        AplicarTransformacion();
                    }
                }else{
                    
                }
            }
        }*/
    }

    public void ObtenerTipoElemental(AsignarTipo.TipoElemental tipo)
    {
        tipoActual = tipo;
        tieneTipo = true;

        // Aquí puedes añadir efectos visuales, cambios de color, partículas, etc.
        Debug.Log("Enemigo melee ha recibido tipo elemental: " + tipo);
    }

    void AplicarTransformacion()
{
    switch (tipoActual)
    {
        case AsignarTipo.TipoElemental.Fuego:
            GetComponent<Renderer>().material.color = Color.red;
            break;
        case AsignarTipo.TipoElemental.Agua:
            GetComponent<Renderer>().material.color = Color.blue;
            break;
        case AsignarTipo.TipoElemental.Tierra:
            GetComponent<Renderer>().material.color = Color.green;
            break;
        case AsignarTipo.TipoElemental.Electricidad:
            GetComponent<Renderer>().material.color = Color.yellow;
            break;
    }
    Debug.Log("Transformación aplicada: " + tipoActual);
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
