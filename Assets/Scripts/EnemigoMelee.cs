using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemigoMelee : MonoBehaviour
{
    public bool tieneTipo = false; 
    private bool transformado = false;
    private bool EstoySiendoLlamado = false;
    private bool isAlive = true;
    private bool EstoyConvertido = false;
    private bool EstoyEnAreaInfluencia = false;
    public AsignarTipo.TipoElemental tipoActual;
    public Transform enemigoElemental;

    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){
            Muerte();
        }else{
            EstoyConvertido = ComprobarConversion();
        }

        if (!EstoyConvertido){
            if(EstoySiendoLlamado){
                EstoyEnAreaInfluencia = ComprobarAreaConversion();
                if(EstoyEnAreaInfluencia){
                    AplicarTransformacion();
                }else{
                    MoverHaciaElemental(enemigoElemental.position);
                }
            }
        }
    }

    public void ObtenerTipoElemental(AsignarTipo.TipoElemental tipo)
    {
        tipoActual = tipo;
        tieneTipo = true;

        Debug.Log("Enemigo melee ha recibido tipo elemental: " + tipo);
    }

    void AplicarTransformacion()
    {
        transformado = true;

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
        Debug.Log("Transformación aplicada correctamente");
    }

    public void Llamada(Transform elemental){
        enemigoElemental = elemental;
        EstoySiendoLlamado = true;
    }

    public void MoverHaciaElemental(Vector3 enemigoElemental)
    {
        if (agent != null)
        {
            agent.SetDestination(enemigoElemental);
            Debug.Log("Enemigo melee se está moviendo hacia el elemental");
        }
    }

    void Muerte(){

    }

    bool ComprobarConversion(){
        return false;
    }

    bool ComprobarAreaConversion(){
        return false;
    }
}
