using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Car {
    public int id { get; set; }
    public int length { get; set; }
    public int x { get; set; }
    public int y { get; set; }

    public Orientation orientation { get; set; }

    public Car(int id, int length, Orientation orientation, Vector2Int position) {
        this.id = id;
        this.length = length;
        this.orientation = orientation;
        this.x = position.x;
        this.y = position.y;
    }

    public T DeepClone<T>() {
        using (var mem = new MemoryStream()) {
            var formatter = new BinaryFormatter();
            formatter.Serialize(mem, this);
            mem.Position = 0;
            return (T)formatter.Deserialize(mem);
        }
    }
}

public enum Orientation { horizontal, vertical };
public enum Direction { forward, backward };