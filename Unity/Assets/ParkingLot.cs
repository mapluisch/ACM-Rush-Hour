using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class ParkingLot {
    public int[,] area = new int[6, 6];
    public List<Car> cars = new List<Car>();

    public bool CanCarMove(Car c, Direction direction) {
        if (direction == Direction.forward) {
            if (c.orientation == Orientation.horizontal) {
                if (c.x + c.length < area.GetLength(1)) {
                    return (area[c.x + c.length, c.y] == 0);
                }
            } else {
                if (c.y + c.length < area.GetLength(0)) {
                    return (area[c.x, c.y + c.length] == 0);
                }
            }
        } else if (direction == Direction.backward) {
            if (c.orientation == Orientation.horizontal) {
                if (c.x - 1 >= 0) {
                    return (area[c.x - 1, c.y] == 0);
                }
            } else {
                if (c.y - 1 >= 0) {
                    return (area[c.x, c.y - 1] == 0);
                }
            }
        }
        return false;
    }

    public void PlaceCars() {
        foreach (Car c in cars) {
            for (int i = 0; i < c.length; i++) {
                if (c.orientation == Orientation.horizontal) {
                    area[c.x + i, c.y] = c.id;
                } else {
                    area[c.x, c.y + i] = c.id;
                }
            }
        }
    }

    public void CreateCarsBasedOnArea() {
        cars = new List<Car>();
        for (int i = 1; i <= 10; i++) {
            for (int y = 0; y < area.GetLength(1); y++) {
                for (int x = 0; x < area.GetLength(0); x++) {
                    if (area[x, y] == i) {
                        Car c = new Car(i,
                                        GetCarLenghtBasedOnArea(x, y, i),
                                        GetCarOrientationBasedOnArea(x, y, i),
                                        new Vector2Int(x, y));
                        cars.Add(c);
                        // assign x and y to break out of nested loops but don't break outer i-loop
                        x = y = area.GetLength(0);
                    }
                }
            }
        }
    }

    public Orientation GetCarOrientationBasedOnArea(int x, int y, int id) {
        if (x + 1 >= area.GetLength(0)) return Orientation.vertical;
        return (area[x + 1, y] == id ? Orientation.horizontal : Orientation.vertical);
    }

    public int GetCarLenghtBasedOnArea(int x, int y, int id) {
        int length = 1;
        for (int i = 1; i < area.GetLength(0) - x; i++) {
            if (area[x + i, y] == id) length++;
        }
        for (int i = 1; i < area.GetLength(1) - y; i++) {
            if (area[x, y + i] == id) length++;
        }
        return length;
    }


    public void MoveCar(Car c, Direction direction) {
        if (direction == Direction.forward) {
            // remove 'tail' of the car
            area[c.x, c.y] = 0;
            if (c.orientation == Orientation.horizontal) {
                // add new 'head' of the car in adjacent tile
                area[c.x + c.length, c.y] = c.id;
                // set new initial car position
                c.x++;
            } else {
                // add new 'head' of the car in adjacent tile
                area[c.x, c.y + c.length] = c.id;
                // set new initial car position
                c.y++;
            }

        } else if (direction == Direction.backward) {
            if (c.orientation == Orientation.horizontal) {
                // remove 'tail' of the car
                area[c.x + (c.length - 1), c.y] = 0;
                // add new 'head' of the car in adjacent tile
                area[c.x - 1, c.y] = c.id;
                // set new initial car position
                c.x--;
            } else {
                // remove 'tail' of the car
                area[c.x, c.y + (c.length - 1)] = 0;
                // add new 'head' of the car in adjacent tile
                area[c.x, c.y - 1] = c.id;
                // set new initial car position
                c.y--;
            }
        }
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
