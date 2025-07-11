using UnityEngine;

public class SimpleFirstPerson : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Mouse")]
    public float sensitivity = 2f;
    private float xRotation = 0f;

    [Header("Links")]
    public Transform playerBody;  // 指向外層 Player（圓柱）
    private Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // 鎖定滑鼠
        rb = playerBody.GetComponent<Rigidbody>(); // 取得剛體
    }

    void Update()
    {
        // 滑鼠控制相機
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // 相機上下
        playerBody.Rotate(Vector3.up * mouseX);                        // 玩家左右
    }

    void FixedUpdate()  // 鍵盤控制移動（用剛體）
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = playerBody.forward * v + playerBody.right * h;

        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}
