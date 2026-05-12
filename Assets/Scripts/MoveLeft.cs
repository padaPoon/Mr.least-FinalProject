using System.Collections;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 10f;
    
    // ใช้ static เพื่อให้ทุก object ใช้ค่าเดียวกัน รวมถึง object ที่ spawn ทีหลัง
    public static float speedMultiplier = 1f;

    void Update()
    {
        GameObject player = GameObject.Find("Player");
        bool isGameOver = player.GetComponent<PlayerController>().gameOver;
        if (isGameOver)
        {
            return;
        }

        transform.Translate(Vector3.left * speed * speedMultiplier * Time.deltaTime);

        if (transform.position.x < -15 && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
        
    }
}