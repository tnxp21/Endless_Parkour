using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform[] levelPart;
    [SerializeField] Transform respawnPos;
    [SerializeField] Vector3 nextSpawnPosition;
    [SerializeField] float distanceToSpawn;
    [SerializeField] float distanceToDelete;

    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SpawnLevelPart();
        DeleteLevelPart();
    }

    void DeleteLevelPart()
    {
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);
            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete) Destroy(partToDelete.gameObject);
        }
    }
    private void SpawnLevelPart()
    {
        while (Vector2.Distance(player.transform.position, nextSpawnPosition) < distanceToSpawn)
        {
            Transform selectedPart = levelPart[Random.Range(0, levelPart.Length)];
            Vector3 spawmPosition = nextSpawnPosition - selectedPart.Find("StartPoint").position;
            Transform newPart = Instantiate(selectedPart, spawmPosition, transform.rotation, transform);
            nextSpawnPosition = newPart.Find("EndPoint").position;
        }
    }
}
