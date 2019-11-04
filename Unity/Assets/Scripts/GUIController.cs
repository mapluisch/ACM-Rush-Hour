using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
    public UnityEngine.UI.Text selectedCarLabel, parkingLotLabel;
    public UnityEngine.UI.InputField parkingLotInput;
    private int carID = 0;
    private Car currentCar;

    // Editor related stuff
    public static bool isEditing = false;
    public static int editorValue = 0;

    public void SelectNextCar() {
        carID++;
        if (carID >= Main.instance.parkingLot.cars.Count) carID = 0;
        currentCar = Main.instance.parkingLot.cars[carID];
    }

    public void SelectPreviousCar() {
        carID--;
        if (carID < 0) carID = Main.instance.parkingLot.cars.Count - 1;
        currentCar = Main.instance.parkingLot.cars[carID];
    }

    public void MoveCarForward() {
        if (Main.instance.parkingLot.CanCarMove(currentCar, Direction.forward))
            Main.instance.parkingLot.MoveCar(currentCar, Direction.forward);
    }

    public void MoveCarBackwards() {
        if (Main.instance.parkingLot.CanCarMove(currentCar, Direction.backward))
            Main.instance.parkingLot.MoveCar(currentCar, Direction.backward);
    }

    void Update() {
        selectedCarLabel.text = "Car " + carID;
        ShowParkingLotAsString();

        // editor value parsing
        if (isEditing) {
            // check if numpad is pressed and change editorValue accordingly
            // using a loop here to minify code (otherwise use 10 different if(Input.GetKeyDown(KeyCode.AlphaX)) editorValue = X)
            for (int i = 0; i <= 9; i++) {
                if (Input.GetKeyDown(i.ToString()))
                    editorValue = i;
            }
        }
    }

    void ShowParkingLotAsString() {
        parkingLotLabel.text = "";
        for (int i = 0; i < Main.instance.parkingLot.area.GetLength(0); i++) {
            for (int j = 0; j < Main.instance.parkingLot.area.GetLength(1); j++) {
                parkingLotLabel.text += Main.instance.parkingLot.area[j, i] + ", ";
            }
            parkingLotLabel.text += "\n";
        }
    }

    public void GenerateParkingLotWithInput() {
        Main.instance.GenerateParkingLot(parkingLotInput.text);
    }



    public UnityEngine.UI.Text solutionStepLabel;
    int solutionStep = 0;
    public void NextSolutionStep() {
        solutionStep++;
        if (solutionStep >= Main.instance.solutionPath.Count) solutionStep = 0;

        Node solution = Main.instance.solutionPath[Main.instance.solutionPath.Count - 1 - solutionStep];

        Main.instance.parkingLot = solution.parkingLot;

        solutionStepLabel.text = "Step " + solutionStep;
    }

    public void PreviousSolutionStep() {
        solutionStep--;
        if (solutionStep <= 0) solutionStep = Main.instance.solutionPath.Count - 1;

        Node solution = Main.instance.solutionPath[Main.instance.solutionPath.Count - 1 - solutionStep];

        Main.instance.parkingLot = solution.parkingLot;

        solutionStepLabel.text = "Step " + solutionStep;
    }

    public void ClearButton() {
        isEditing = false;
        solutionStep = 0;
        solutionStepLabel.text = "Step 0";
        StopAllCoroutines();
        GenerateParkingLotWithInput();
    }
    public void ToggleEditor() {
        isEditing = !isEditing;
    }

    public UnityEngine.UI.Slider playbackSpeedSlider;
    public UnityEngine.UI.Text playbackSpeedLabel;
    float speed = 0.5f; // show next step every <speed> seconds (basically 1 second / 2 steps)
    public void AutoPlaySolutionHelper() {
        StartCoroutine(AutoPlaySolution(speed));
    }

    public void ChangeSpeed() {
        speed = (1 / playbackSpeedSlider.value);
        playbackSpeedLabel.text = playbackSpeedSlider.value + " steps per second";
        StopAllCoroutines();
        AutoPlaySolutionHelper();
    }

    IEnumerator AutoPlaySolution(float s) {
        while (true) {
            NextSolutionStep();
            yield return new WaitForSeconds(s);
        }
    }


    public void LoadACMInput() {
        Main.instance.GenerateParkingLot("8\n" +
                                         "2 H 2 1\n" +
                                         "2 H 0 0\n" +
                                         "3 V 0 5\n" +
                                         "3 V 1 0\n" +
                                         "3 V 1 3\n" +
                                         "2 V 4 0\n" +
                                         "2 H 4 4\n" +
                                         "3 H 5 2\n");
    }
}
