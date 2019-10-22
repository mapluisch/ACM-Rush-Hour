using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// A Node is one encapsulated possibility used for the breadth-first-based Solve-algorithm
// It keeps track of a new parkingLot-configuration, which is basically:
// parent.parkingLot, but movedCar being moved a step into movedDirection

[System.Serializable]
public class Node {
    public Node parent;
    public ParkingLot parkingLot;
    public Car movedCar;
    public Direction movedDirection;


    public Node(Node parent, ParkingLot parkingLot, Car movedCar, Direction movedDirection) {
        this.parent = parent;
        this.parkingLot = parkingLot.DeepClone<ParkingLot>();
        if (movedCar != null) this.movedCar = movedCar.DeepClone<Car>();
        this.movedDirection = movedDirection;
    }

    public string GetParkingLotString() {
        string result = "";
        for (int i = 0; i < this.parkingLot.area.GetLength(0); i++) {
            for (int j = 0; j < this.parkingLot.area.GetLength(1); j++) {
                result += this.parkingLot.area[j, i];
            }
        }
        return result;
    }

    public bool IsParkingLotSolved() {
        return (parkingLot.cars[0].x + parkingLot.cars[0].length == parkingLot.area.GetLength(0));
    }

}
