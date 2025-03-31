using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ElementalBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    private bool isAlive = true;
    private bool HayEnemigoMeleeCerca = false;
    private bool HayJugadorCerca = false;
    private bool HayEnemigoMeleeEnArea = false; 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
                    AtacarDistancia();
                }
            }
        }
        else if (HayJugadorCerca)
        {
            AtacarDistancia();
        }
        else
        {
            Patrullar();
        }
    }

    bool ComprobarEnemigosMelee(){

    }

    bool ComprobarJugador(){

    }

    bool ComprobarAreaInfluencia(){

    }

    bool EnemigoTieneTipoElemental(){

    }

    void Muerte(){

    }

    void AsignarTipo(){

    }

    void AtacarDistancia(){

    }

    void Patrullar(){

    }

    void LlamarEnemigoMelee(){

    }
}
