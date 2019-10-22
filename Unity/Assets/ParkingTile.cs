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
    public UnityEngine.UI.Image image;

    void Start() {
        this.image = this.GetComponent<UnityEngine.UI.Image>();
        this.position = new Vector2Int(this.transform.GetSiblingIndex(), this.transform.parent.GetSiblingIndex());
    }

    void Update() {
        image.color = Main.instance.carColors[Main.instance.parkingLot.area[position.x, position.y]];
        // change sprite if parkingTile is empty
        if (Main.instance.parkingLot.area[position.x, position.y] == 0) {
            this.image.sprite = Main.instance.asphaltSprite;
        } else {
            this.image.sprite = null;
        }
    }

    // if the editor is enabled, you can click on a ParkingTile to store a car's value in the same ParkingLot.area position
    public void Draw() {
        if (GUIController.isEditing) {
            Main.instance.parkingLot.area[position.x, position.y] = GUIController.editorValue;
            Main.instance.parkingLot.CreateCarsBasedOnArea();
        }
    }
}
