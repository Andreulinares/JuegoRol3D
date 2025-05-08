using UnityEngine;

public class SpawnerPersonaje : MonoBehaviour
{
    public GameObject personaje1Prefab; // Guerrero
    public GameObject personaje2Prefab; // Arquero
    public Transform puntoSpawn;

    void Start()
    {
        int seleccion = PlayerPrefs.GetInt("PersonajeSeleccionado", 1); // Valor por defecto: 1

        if (seleccion == 1)
        {
            Instantiate(personaje1Prefab, puntoSpawn.position, puntoSpawn.rotation);
        }
        else if (seleccion == 2)
        {
            Instantiate(personaje2Prefab, puntoSpawn.position, puntoSpawn.rotation);
        }
    }
}