using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// A Node is one encapsulated possibility used for the breadth-first-based Solve-algorithm
// It keeps track of a new parkingLot-configuration, which is basically:
// parent.parkingLot, but movedCar being moved a step into movedDirection

public class Node {
    public Node parent;
    public ParkingLot parkingLot;
    public int movedCarID;
    public Direction movedDirection;

    public Node(Node parent, ParkingLot parkingLot, int movedCarID, Direction movedDirection) {
        this.parent = parent;
        this.parkingLot = parkingLot.Copy();
        this.movedCarID = movedCarID;
        this.movedDirection = movedDirection;
    }

    public int GetNodeValue() {
        StringBuilder sb = new StringBuilder();
        foreach (Car car in parkingLot.cars) {
            sb.Append(car.position.x);
            sb.Append(car.position.y);
        }
        return sb.ToString().GetHashCode();
    }

    public bool IsParkingLotSolved() {
        return (parkingLot.cars[0].position.x + parkingLot.cars[0].length == parkingLot.area.GetLength(0));
    }
}