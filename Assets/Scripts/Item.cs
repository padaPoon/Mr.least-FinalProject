using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Heal,         // ฟื้นหัวใจ
        Immunity,     // อมตะชั่วคราว
        SpeedBoost    // เร่งความเร็วชั่วคราว
    }

    [Header("Item Settings")]
    public ItemType type = ItemType.Heal;
    public int healAmount = 1;         // ใช้กับ Heal
    public float duration = 5f;        // ใช้กับ SpeedBoost (Immunity ใช้ค่าจาก player)

    [Header("Effects")]
    public GameObject pickupVFX;
    public AudioClip pickupSound;
    public float rotateSpeed = 90f;

    void Update()
    {
        // หมุนไอเทมให้ดูมีชีวิตชีวา
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        ApplyEffect(player);

        if (pickupVFX != null)
            Instantiate(pickupVFX, transform.position, Quaternion.identity);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }

    void ApplyEffect(PlayerController player)
    {
        switch (type)
        {
            case ItemType.Heal:
                player.Heal(healAmount);
                break;
            case ItemType.Immunity:
                player.ActivateImmunity();
                break;
            case ItemType.SpeedBoost:
                player.ActivateSpeedBoost(duration);
                break;
        }
    }
}