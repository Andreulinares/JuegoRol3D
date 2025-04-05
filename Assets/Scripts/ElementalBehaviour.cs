using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ElementalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private GameObject jugador;
    public EnemigoMelee enemigo;

    private bool isAlive = true;
    private bool HayEnemigoMeleeCerca = false;
    private bool HayJugadorCerca = false;
    private bool HayEnemigoMeleeEnArea = false; 

    private ElementalPatrol ElementalPatrolScript;
    private AsignarTipo AsignadorDeTipos;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ElementalPatrolScript = GetComponent<ElementalPatrol>();
        AsignadorDeTipos = GetComponent<AsignarTipo>();
        jugador = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            Muerte();
        }else{
            HayEnemigoMeleeCerca = ComprobarEnemigosMelee();
            HayJugadorCerca = ComprobarJugador();
        }

        if (HayEnemigoMeleeCerca)
        {
            HayEnemigoMeleeEnArea = ComprobarAreaInfluencia();
            if (!HayEnemigoMeleeEnArea){
                LlamarEnemigoMelee();
            } else{

                if(!EnemigoTieneTipoElemental()){
                    AsignarTipo();
                } else if(HayJugadorCerca){
                    if (JugadorEnRangoDeAtaque()){
                        AtacarDistancia();
                    }else{
                        AcercarseAlJugador();
                    }
                }
            }
        }
        else if (HayJugadorCerca)
        {
            if (JugadorEnRangoDeAtaque()){
                AtacarDistancia();
            }else{
                AcercarseAlJugador();
            }
        }
        else
        {
            Patrullar();
        }
    }

    bool ComprobarEnemigosMelee(){
        return true;
    }

    bool ComprobarJugador(){
        return true;
    }

    bool ComprobarAreaInfluencia(){
        return true;
    }

    bool EnemigoTieneTipoElemental(){
        return enemigo.tieneTipo;
    }

    bool JugadorEnRangoDeAtaque(){
        return true;
    }

    void Muerte(){

    }

    void AsignarTipo(){
        AsignadorDeTipos.AsignarTipoElemental(enemigo);
    }

    void AcercarseAlJugador(){
        agent.SetDestination(jugador.transform.position);
        Debug.Log("Moviendose hacia el jugador");
    }

    void AtacarDistancia(){

    }

    void Patrullar(){
        ElementalPatrolScript.ActivarPatrullaje();
    }

    void LlamarEnemigoMelee(){
        enemigo.MoverHaciaElemental(transform.position);
    }
}
