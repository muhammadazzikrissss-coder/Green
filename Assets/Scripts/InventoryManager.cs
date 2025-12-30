using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Data")]
    public List<ItemData> backpackItems = new List<ItemData>(); // Semua item di tas
    public ItemData[] hotbarSlots = new ItemData[3]; // 3 Slot Hotbar aktif

    [Header("UI References")]
    public Image[] hotbarIcons; // 3 Image icon di UI bawah
    public GameObject[] hotbarHighlighters; // 3 Image border/warna penanda aktif
    public GameObject inventoryWindow; // Panel UI Tas Besar (Grid)
    public Transform inventoryGridContent; // Tempat spawn tombol item di tas
    public GameObject inventoryItemPrefab; // Prefab tombol item di tas

    // State
    public int currentSlotIndex = 0; // Slot mana yang lagi dipilih (0, 1, 2)
    private bool isInventoryOpen = false;

    void Awake()
    {
        Instance = this;
        // Inisialisasi Hotbar kosong atau default
        if (hotbarSlots.Length < 3) hotbarSlots = new ItemData[3];
    }

    void Start()
    {
        UpdateHotbarUI();
        SelectSlot(0); // Pilih slot 1 di awal
        inventoryWindow.SetActive(false); // Tutup tas di awal
    }

    void Update()
    {
        // Tekan 'I' atau Tab buat buka tas
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    // --- FUNGSI HOTBAR ---
    public void SelectSlot(int index)
    {
        currentSlotIndex = index;

        // Visual Highlight
        for (int i = 0; i < hotbarHighlighters.Length; i++)
        {
            hotbarHighlighters[i].SetActive(i == index);
        }

        Debug.Log($"Memegang Slot {index}: " + (GetCurrentItem() ? GetCurrentItem().itemName : "Kosong"));
    }

    public ItemData GetCurrentItem()
    {
        return hotbarSlots[currentSlotIndex];
    }

    // --- FUNGSI TAS / INVENTORY ---
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryWindow.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            RefreshInventoryUI();
        }
    }

    void RefreshInventoryUI()
    {
        // Hapus item lama
        foreach (Transform child in inventoryGridContent)
        {
            Destroy(child.gameObject);
        }

        // Spawn ulang item
        foreach (ItemData item in backpackItems)
        {
            GameObject newSlot = Instantiate(inventoryItemPrefab, inventoryGridContent);

            // --- PERBAIKAN DI SINI ---
            // Mengambil component Image yang nempel langsung di tombol prefab
            Image imgComponent = newSlot.GetComponent<Image>();

            if (imgComponent != null)
            {
                imgComponent.sprite = item.icon;
            }
            else
            {
                Debug.LogError("Prefab tombol tidak punya component Image!");
            }
            // -------------------------

            // Setup Tombol
            newSlot.GetComponent<Button>().onClick.AddListener(() => {
                EquipItemToCurrentSlot(item);
            });
        }
    }

    // Logic: Item dari tas dimasukkan ke slot tangan yang lagi aktif
    public void EquipItemToCurrentSlot(ItemData item)
    {
        // Slot 0 (Tangan) biasanya dikunci gak boleh diganti, tapi terserah kamu.
        // Kita kunci Slot 0 biar tetep jadi "Hand" kalau mau.
        if (currentSlotIndex == 0 && item.itemType != ItemType.None)
        {
            Debug.Log("Slot 1 khusus Tangan Kosong/Alat Utama! Pilih Slot 2 atau 3 dulu.");
            return;
        }

        hotbarSlots[currentSlotIndex] = item;
        UpdateHotbarUI();
        Debug.Log($"Memasang {item.itemName} ke Slot {currentSlotIndex + 1}");
    }

    void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarIcons.Length; i++)
        {
            if (hotbarSlots[i] != null)
            {
                hotbarIcons[i].sprite = hotbarSlots[i].icon;
                hotbarIcons[i].enabled = true;
            }
            else
            {
                hotbarIcons[i].enabled = false;
            }
        }
    }
}