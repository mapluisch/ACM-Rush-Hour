using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A ParkingTile resembles one 100px by 100px spot on the parkingLot
// There's a total of 6x6=36 invidiual ParkingTiles
// Each ParkingTile has a distinct position, which resembles the position on the ParkingLot.area

// Each ParkingTile takes the connected int-value from the current ParkingLot.area
// and refreshes its appearances based on the int-value.
public class ParkingTile : MonoBehaviour {
    public Vector2Int position;
    private MeshRenderer car;
    private Material carMaterial;

    void Start() {
        // fetch car-mesh
        this.car = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        // initialize position based on hierarchy index
        this.position = new Vector2Int(this.transform.GetSiblingIndex(), this.transform.parent.GetSiblingIndex());
        // create a new carMaterial
        carMaterial = new Material(car.material);
        car.material = carMaterial;
    }

    void Update() {
        if (!editMouseHover) {
            carMaterial.color = Main.instance.carColors[Main.instance.parkingLot.area[position.x, position.y]];
            // disable car-mesh if parkingTile is empty
            car.enabled = (Main.instance.parkingLot.area[position.x, position.y] != 0);
        } else {
            Color hoverColor = Main.instance.carColors[GUIController.editorValue];
            hoverColor.a = 0.75f;
            carMaterial.color = hoverColor;
            car.enabled = true;
        }
    }

    // if the editor is enabled, you can click on a ParkingTile to store a car's value in the same ParkingLot.area position
    public void Draw() {
        if (GUIController.isEditing) {
            Main.instance.parkingLot.area[position.x, position.y] = GUIController.editorValue;
            Main.instance.parkingLot.CreateCarsBasedOnArea();
            editMouseHover = false;
        }
    }
    private bool editMouseHover = false;
    public void OnPointerEnter() {
        if (GUIController.isEditing) {
            editMouseHover = true;
        }
    }

    public void OnPointerExit() {
        if (GUIController.isEditing) {
            editMouseHover = false;
        }
    }
}
