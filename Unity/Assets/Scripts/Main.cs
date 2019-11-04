using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    public static Main instance;
    public int numOfVehicles = 0; // redundant, could just use cars.Count
    public ParkingLot parkingLot = new ParkingLot();
    public List<Color> carColors = new List<Color>();
    public UnityEngine.UI.Text solutionText;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(this);

        FillCarColors();
    }


    void FillCarColors() {
        // fills the carColors-List with random colors

        // the first color is always used for blank spaces
        // hence an asphalt-looking color
        carColors.Add(new Color(1f, 1f, 1f, 1f));

        // the second stays always the same - red
        // since it's always representing the escaping car
        carColors.Add(Color.red);
        // a maxmimum of 10 colors can be hardcoded here
        // based on ACM's description
        for (int i = 0; i < 9; i++) {
            carColors.Add(new Color(UnityEngine.Random.Range(0f, 1f),
                                    UnityEngine.Random.Range(0f, 1f),
                                    UnityEngine.Random.Range(0f, 1f),
                                    1f));
        }
    }
    // solve the given rush hour puzzle using the breadth-first search
    // use a FIFO queue of type node to store all possible solutions / moves
    // once a solution is found, return it (since it's guaranteed to be optimal)
    public Queue<Node> solutionQueue = new Queue<Node>();
    // visited nodes contains each visitied parking lot configuration stored as a string
    // (by concatenating each (x,y) cell)
    public List<string> visitedNodes = new List<string>();
    public void Solve() {
        // reset old remains (if any)
        solutionQueue.Clear();
        visitedNodes.Clear();
        solutionPath.Clear();

        CreateInitialNode();
        while (solutionQueue.Count > 0) {
            QueuePossibleMoves(solutionQueue.Dequeue());
        }
        if (solutionPath.Count == 0) {
            print("No Solution possible.");
        }
    }


    public void CreateInitialNode() {
        Node n = new Node(null, parkingLot, 0, 0);
        solutionQueue.Enqueue(n);
    }
    public List<Node> solutionPath = new List<Node>();
    public void CreateSolutionPath(Node endNode) {
        if (endNode.parent != null) {
            solutionPath.Add(endNode);
            CreateSolutionPath(endNode.parent);
        } else {
            // print amount of steps needed to solve the puzzle
            solutionText.text = ("Rush-Hour solved in " + solutionPath.Count + " steps.\n\n");
            // create ACM conform output
            for (int i = solutionPath.Count - 1; i >= 0; i--) {
                // subtract 1 from ID to get ACM-conform Car 0 - Car 9 (internally referred to as 1-10)
                solutionText.text += ("Car " + (solutionPath[i].movedCarID - 1) + ": " + (solutionPath[i].movedDirection == Direction.forward ? "F" : "B") + "\n");
            }
            // add the initial starting node to the solution path as well (useful for solutionStepper)
            solutionPath.Add(endNode);
        }
    }

    public void QueuePossibleMoves(Node n) {
        foreach (Car car in n.parkingLot.cars) {
            if (n.parkingLot.CanCarMove(car, Direction.forward)) {
                Node m = new Node(n, n.parkingLot, car.id, Direction.forward);
                m.parkingLot.MoveCar(m.parkingLot.cars[car.id - 1], Direction.forward);
                if (!visitedNodes.Contains(m.GetParkingLotString())) {
                    solutionQueue.Enqueue(m);
                    visitedNodes.Add(m.GetParkingLotString());
                    if (m.IsParkingLotSolved()) {
                        CreateSolutionPath(m);
                        solutionQueue.Clear();
                        return;
                    }
                }
            }
            if (n.parkingLot.CanCarMove(car, Direction.backward)) {
                Node m = new Node(n, n.parkingLot, car.id, Direction.backward);
                m.parkingLot.MoveCar(m.parkingLot.cars[car.id - 1], Direction.backward);
                if (!visitedNodes.Contains(m.GetParkingLotString())) {
                    solutionQueue.Enqueue(m);
                    visitedNodes.Add(m.GetParkingLotString());
                    if (m.IsParkingLotSolved()) {
                        CreateSolutionPath(m);
                        solutionQueue.Clear();
                        return;
                    }
                }
            }
        }
    }


    public void GenerateParkingLot(string input) {
        Main.instance.parkingLot = new ParkingLot();
        ReadInput(input);
        parkingLot.PlaceCars();
    }

    public void ReadInput(string input) {
        // split input into individual lines
        string[] inputLines = input.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        // carCounter is used to generate the car's id -> start at index 1, so that empty cells in the 
        // parkingLot use intValue 0 and not something obscure like -1
        int carCounter = 1;
        // iterate over every line and create a Car object
        foreach (String item in inputLines) {
            bool isNumeric = int.TryParse(item, out numOfVehicles);
            if (isNumeric && numOfVehicles == 0) {
                print("end of file");
                carCounter = 1;
                break;
            } else if (!isNumeric) {
                string[] carVars = item.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Car c = new Car(carCounter,
                                int.Parse(carVars[0]),
                                carVars[1] == "H" ? Orientation.horizontal : Orientation.vertical,
                                new Vector2Int(int.Parse(carVars[3]), int.Parse(carVars[2])));

                parkingLot.cars.Add(c);
                carCounter++;
            }
        }
    }
}
