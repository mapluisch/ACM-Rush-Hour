using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Car {
    public int id { get; set; }
    public int length { get; set; }
    public Vector2Int position { get; set; }
    public Orientation orientation { get; set; }

    public Car (int id, int length, Orientation orientation, Vector2Int position) {
        this.id = id;
        this.length = length;
        this.orientation = orientation;
        this.position = position;
    }

    public Car (Car car) {
        this.id = car.id;
        this.length = car.length;
        this.orientation = car.orientation;
        this.position = car.position;
    }
}

public enum Orientation { horizontal, vertical }
public enum Direction { forward, backward }