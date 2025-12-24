using UnityEngine;

public class TestPlayerControls : MonoBehaviour
{
    void OnGUI()
    {
        // Tombol Simulasi Ganti Item di Pojok Kiri Atas Layar
        GUILayout.BeginArea(new Rect(10, 10, 200, 200));
        GUILayout.Label("Current Item: " + FarmPlot.playerHeldItem);

        if (GUILayout.Button("Hold: Seed Sack (Karung)"))
        {
            FarmPlot.playerHeldItem = "SeedSack";
        }

        if (GUILayout.Button("Hold: Watering Can (Penyiram)"))
        {
            FarmPlot.playerHeldItem = "WateringCan";
        }

        if (GUILayout.Button("Hold: Hand (Kosong)"))
        {
            FarmPlot.playerHeldItem = "Hand";
        }
        GUILayout.EndArea();
    }
}