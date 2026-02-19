using System.Collections.Generic;
using UnityEngine;

public class SeatedPeopleManager : MonoBehaviour
{
    [Header("Configuració")]
    [Range(0f, 1f)]
    public float emptyProbability = 0.3f; // 0.3 = 30% de cadires buides

    public GameObject[] peoplePrefabs; // Els 20 models de persones

    private List<GameObject> chairs = new List<GameObject>();

    void Start()
    {
        GetAllChairs();
        SpawnPeople();
    }

    void GetAllChairs()
    {
        GameObject[] foundChairs = GameObject.FindGameObjectsWithTag("Chair");
        chairs.AddRange(foundChairs);
    }

    void SpawnPeople()
    {
        foreach (GameObject chair in chairs)
        {
            // Decideix si la cadira queda buida
            if (Random.value < emptyProbability)
                continue;

            // Escull persona aleatòria
            GameObject randomPerson = peoplePrefabs[Random.Range(0, peoplePrefabs.Length)];

            // Instancia la persona a la posició de la cadira
            Instantiate(
                randomPerson,
                chair.transform.position,
                chair.transform.rotation
            );
        }
    }
}
