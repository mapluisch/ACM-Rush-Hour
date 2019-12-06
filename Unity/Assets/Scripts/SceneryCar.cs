using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryCar : MonoBehaviour {
    Orientation orientation;
    Direction direction;
    int movementFactor;
    public float speed = 0.25f;

    void Start() {
        // get a random orientation & direction
        this.orientation = (Random.Range(0f, 1f) >= 0.5f) ? Orientation.horizontal : Orientation.vertical;
        this.direction = (Random.Range(0f, 1f) >= 0.5f) ? Direction.backward : Direction.forward;
        movementFactor = (direction == Direction.backward) ? -1 : 1;

        // set a random material color 
        this.GetComponent<MeshRenderer>().material.color = Main.instance.carColors[Random.Range(1, 11)];

        if (this.orientation == Orientation.horizontal) {
            this.transform.localScale = new Vector3(Random.Range(2, 4) * 2, 2, 1);
            this.transform.localPosition = new Vector3(-80, Random.Range(-38, 38), 60.6f);
            if (direction == Direction.backward) {
                this.transform.localPosition = new Vector3(80, Random.Range(-38, 38), 60.6f);
            }
        } else {
            this.transform.localScale = new Vector3(2, Random.Range(2, 4) * 2, 1);
            this.transform.localPosition = new Vector3(Random.Range(-70, 70), -50, 60.6f);
            if (direction == Direction.backward) {
                this.transform.localPosition = new Vector3(Random.Range(-70, 70), 50, 60.6f);
            }
        }
    }

    void Update() {
        if (this.orientation == Orientation.horizontal) {
            this.transform.Translate(movementFactor * speed, 0f, 0f);
        } else {
            this.transform.Translate(0f, movementFactor * speed, 0f);
        }

        if (this.transform.localPosition.x > 80 || this.transform.localPosition.x < -80 ||
            this.transform.localPosition.y > 50 || this.transform.localPosition.y < -50) {
            Destroy(this.gameObject);
        }
    }
}
