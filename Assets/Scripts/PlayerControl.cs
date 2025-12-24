using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Mengambil komponen Rigidbody dan SpriteRenderer otomatis
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- UBAH BAGIAN INI ---

        float moveX = 0f;
        float moveZ = 0f;

        // Cek apakah ada Joystick di layar?
        if (MobileJoystick.Instance != null)
        {
            // Ambil input dari script Joystick yang kita buat
            moveX = MobileJoystick.Instance.Horizontal();
            moveZ = MobileJoystick.Instance.Vertical();
        }
        else
        {
            // Fallback keyboard biasa (jaga-jaga error)
            moveX = Input.GetAxisRaw("Horizontal");
            moveZ = Input.GetAxisRaw("Vertical");
        }

        // -----------------------

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Script selanjutnya sama seperti sebelumnya...
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        // Logic Flip gambar...
        if (moveX > 0) spriteRenderer.flipX = false;
        else if (moveX < 0) spriteRenderer.flipX = true;
    }
}