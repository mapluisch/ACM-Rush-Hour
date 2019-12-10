using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryCarSpawner : MonoBehaviour {
    public GameObject sceneryCar;
    public int maxSpawnedCars = 10;
    public float carSpeed;
    public float spawnProbability;

    void Update() {
        if (GetCurrentlySpawnedCars() < maxSpawnedCars && Random.Range(0f, 1f) <= spawnProbability) {
            SpawnCar();
        }
    }

    void SpawnCar() {
        GameObject newCar = Instantiate(sceneryCar);
        newCar.transform.SetParent(this.transform);
        newCar.SetActive(true);
    }

    int GetCurrentlySpawnedCars() {
        return this.transform.childCount;
    }

}
