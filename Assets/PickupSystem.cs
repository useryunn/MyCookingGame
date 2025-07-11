using UnityEngine;

/// <summary>
/// 放到「PlayerCamera」或任何能代表視角的物件上
/// </summary>
public class PickupSystem : MonoBehaviour
{
    [Header("References")]
    [Tooltip("遊戲中用作視線射線起點的攝影機")]
    public Transform playerCamera;

    [Tooltip("拿在手上的參考位置 (請設成 PlayerCamera 子物件)")]
    public Transform holdPoint;

    [Header("Settings")]
    [Tooltip("可撿取距離 (公尺)")]
    public float pickupRange = 2f;

    // ────────────── 私有狀態 ──────────────
    private GameObject heldObject;
    private Rigidbody  heldRb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    /* ────────────── 嘗試撿取 ────────────── */
    void TryPickup()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            // 只撿取 Tag = "Pickup" 的物件
            if (hit.collider.CompareTag("Pickup"))
            {
                PickupObject(hit.collider.gameObject);
            }
        }
    }

    /* ────────────── 執行撿取 ────────────── */
    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        heldRb     = obj.GetComponent<Rigidbody>();

        if (heldRb == null)
        {
            Debug.LogWarning($"{obj.name} 沒有 Rigidbody，無法拾取！");
            heldObject = null;
            return;
        }

        // 關閉物理影響並吸附到手
        heldRb.useGravity  = false;
        heldRb.isKinematic = true;

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    /* ────────────── 放下物件 ────────────── */
    void DropObject()
    {
        // 解除父子關係，稍微上提防止穿透
        heldObject.transform.SetParent(null);
        heldObject.transform.position += Vector3.up * 0.05f;

        // 恢復物理
        heldRb.isKinematic = false;
        heldRb.useGravity  = true;

        // 清空狀態
        heldObject = null;
        heldRb     = null;
    }
}
