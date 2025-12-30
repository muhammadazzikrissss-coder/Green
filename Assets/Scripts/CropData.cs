using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName; // Misal: "Wortel", "Lobak"
    public float growthTime; // Waktu tumbuh total atau per stage
    public Sprite[] growthSprites; // Masukkan 3 Sprite di sini (Kecil, Sedang, Panen)
    public Sprite iconUI; // Icon buat di Inventory
}