using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Penting untuk deteksi sentuhan UI

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image bgImage;
    private Image handleImage;
    private Vector2 inputVector; // Menyimpan nilai X dan Y (-1 sampai 1)

    // Singleton sederhana agar mudah dipanggil dari script lain
    public static MobileJoystick Instance;

    private void Awake()
    {
        Instance = this;
        bgImage = GetComponent<Image>();
        handleImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            // Normalisasi agar inputVector bernilai max 1 (lingkaran sempurna)
            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Gerakkan gambar Handle
            handleImage.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bgImage.rectTransform.sizeDelta.x / 2.5f), inputVector.y * (bgImage.rectTransform.sizeDelta.y / 2.5f));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset posisi saat jari dilepas
        inputVector = Vector2.zero;
        handleImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    // Fungsi ini akan dipanggil oleh PlayerController
    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxisRaw("Horizontal"); // Fallback ke Keyboard
    }

    public float Vertical()
    {
        if (inputVector.y != 0) return inputVector.y;
        else return Input.GetAxisRaw("Vertical"); // Fallback ke Keyboard
    }
}