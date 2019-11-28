﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour {
    public UnityEngine.UI.Text selectedCarLabel, parkingLotLabel;
    public UnityEngine.UI.InputField parkingLotInput;
    public UnityEngine.UI.Image editorImage;
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
        ShowParkingLotAsString();

        // editor value parsing
        if (isEditing) {
            // check if numpad is pressed and change editorValue accordingly
            // using a loop here to minify code (otherwise use 10 different if(Input.GetKeyDown(KeyCode.AlphaX)) editorValue = X)
            for (int i = 0; i <= 9; i++) {
                if (Input.GetKeyDown(i.ToString()))
                    editorValue = i;
            }
            // use 0 as 10th car and ß as 0th car (to remove a car)
            if (Input.GetKeyDown(KeyCode.Alpha0)) editorValue = 10;
            if (Input.inputString == "ß") editorValue = 0;
        }
    }

    public void SwitchParkingLotLabelFormat() {
        labelFormatInt = !labelFormatInt;
    }

    private bool labelFormatInt = true;
    void ShowParkingLotAsString() {
        parkingLotLabel.text = "";
        for (int i = 0; i < Main.instance.parkingLot.area.GetLength(0); i++) {
            for (int j = 0; j < Main.instance.parkingLot.area.GetLength(1); j++) {
                if (labelFormatInt) {
                    parkingLotLabel.text += "<color=#" + ColorUtility.ToHtmlStringRGB(Main.instance.carColors[Main.instance.parkingLot.area[j, i]]) + ">" + Main.instance.parkingLot.area[j, i] + "</color>";
                    if (j < Main.instance.parkingLot.area.GetLength(1) - 1) parkingLotLabel.text += ", ";
                } else {
                    if (Main.instance.parkingLot.area[j, i] == 0) {
                        parkingLotLabel.text += "<color=#" + ColorUtility.ToHtmlStringRGB(Main.instance.carColors[Main.instance.parkingLot.area[j, i]]) + ">□</color>";
                    } else {
                        parkingLotLabel.text += "<color=#" + ColorUtility.ToHtmlStringRGB(Main.instance.carColors[Main.instance.parkingLot.area[j, i]]) + ">■</color>";
                    }
                    if (j < Main.instance.parkingLot.area.GetLength(1) - 1) parkingLotLabel.text += " ";
                }
            }
            if (i < Main.instance.parkingLot.area.GetLength(0) - 1) parkingLotLabel.text += "\n";
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
        if (solutionStep < 0) solutionStep = Main.instance.solutionPath.Count - 1;

        Node solution = Main.instance.solutionPath[Main.instance.solutionPath.Count - 1 - solutionStep];

        Main.instance.parkingLot = solution.parkingLot;

        solutionStepLabel.text = "Step " + solutionStep;
    }

    public void ClearButton() {
        if (isEditing) ToggleEditor();
        if (autoPlaying) AutoPlaySolutionHelper();
        solutionStep = 0;
        solutionStepLabel.text = "Step 0";
        StopAllCoroutines();
        Main.instance.GenerateParkingLot("");
    }
    public void ToggleEditor() {
        isEditing = !isEditing;
        if (isEditing) {
            editorImage.color = Color.white;
        } else {
            editorImage.color = new Color(0.196f, 0.196f, 0.196f);
        }
    }

    public UnityEngine.UI.Slider playbackSpeedSlider;
    public UnityEngine.UI.Text playbackSpeedLabel;
    public UnityEngine.UI.Image autoPlayButtonImage;
    public Sprite autoPlayOn, autoPlayOff;
    float speed = 0.5f; // show next step every <speed> seconds (basically 1 second / 2 steps)
    bool autoPlaying = false;
    public void AutoPlaySolutionHelper() {
        if (!autoPlaying) {
            autoPlayButtonImage.sprite = autoPlayOn;
            StartCoroutine(AutoPlaySolution());
        } else {
            autoPlayButtonImage.sprite = autoPlayOff;
            autoPlaying = false;
            StopAllCoroutines();
        }
    }

    public void ChangeSpeed() {
        speed = (1 / playbackSpeedSlider.value);
        playbackSpeedLabel.text = playbackSpeedSlider.value + " steps per second";
    }

    IEnumerator AutoPlaySolution() {
        autoPlaying = true;
        while (true) {
            NextSolutionStep();
            yield return new WaitForSeconds(speed);
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
