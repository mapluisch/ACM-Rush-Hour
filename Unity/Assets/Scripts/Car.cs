using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Car {
    public int id;
    public int length;
    public Vector2Int position;
    public Orientation orientation;

    public Car (int id, int length, Orientation orientation, Vector2Int position) {
        this.id = id;
        this.length = length;
        this.orientation = orientation;
        this.position = position;
    }
}

public enum Orientation { horizontal, vertical }
public enum Direction { forward, backward }