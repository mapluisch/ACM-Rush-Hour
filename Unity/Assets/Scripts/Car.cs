using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Car {
    public int id;
    public int length;
    public Vector2Int position;
    public Orientation orientation;

    public Car(int id, int length, Orientation orientation, Vector2Int position) {
        this.id = id;
        this.length = length;
        this.orientation = orientation;
        this.position = position;
    }
}

public enum Orientation { horizontal, vertical };
public enum Direction { forward, backward };