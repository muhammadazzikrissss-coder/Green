using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // Drag Player ke sini

    [Header("Posisi & Jarak (PENTING)")]
    // Atur angka ini di Inspector agar pas (Misal X:0, Y:10, Z:-8)
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    [Header("Zoom Settings (Perspective)")]
    public float zoomSpeed = 20f;
    public float minFOV = 20f; // Zoom paling dekat
    public float maxFOV = 80f; // Zoom paling jauh

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // --- FIX BUG POSISI AWAL ---
        // Jika target ada, langsung teleport kamera ke posisi idealnya detik ini juga.
        // Jadi tidak peduli di mana kamu menaruh kamera di Scene Editor, 
        // pas Play dia langsung nempel ke Player sesuai offset.
        if (target != null)
        {
            transform.position = target.position + offset;

            // Opsional: Agar kamera selalu nunduk melihat player
            // transform.LookAt(target); 
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // --- 1. Logic Follow (Smooth) ---
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // --- 2. Logic Zoom (Perspective / FOV) ---
        float scrollInput = 0f;

        // Deteksi Input (PC vs Mobile)
        // Jika di PC pakai Scroll Mouse
        scrollInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        // Jika di HP pakai Cubit (Pinch)
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            
            // Bagi 10 biar tidak terlalu ngebut di HP
            scrollInput = deltaMagnitudeDiff * 0.1f; 
        }


        // Eksekusi Zoom (Mengubah Field of View karena Perspective)
        cam.fieldOfView += scrollInput;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFOV, maxFOV);
    }
}