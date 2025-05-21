using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController playerController;
    public int daño = 10;
    private Collider golpe;
    // Start is called before the first frame update

    void Start()
    {
        golpe = GetComponent<Collider>();
        golpe.enabled = false; 
    }

    public void ActivarColliderGolpe()
    {
        golpe.enabled = true; 
        Invoke("DesactivarColliderGolpe", 0.5f); 
    }

    private void DesactivarColliderGolpe()
    {
        golpe.enabled = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        //playerController.manaActualPlayer = playerController.manaActualPlayer + 25;
        if (other.CompareTag("enemy")) // Verifica que el objeto golpeado es un enemigo
        {
            other.GetComponent<EnemigoMelee>().TakeDamage(daño);
            Debug.Log("Golpe impactó al enemigo!");
        }
        else if (other.CompareTag("Elemental"))
        {
            other.GetComponent<ElementalBehaviour>().TakeDamage(daño);
            Debug.Log("Golpe impactó al elemental!");
        }
        else if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossAI>().TakeDamage(daño);
            Debug.Log("Golpe impactó al boss!");
        }
}
}
