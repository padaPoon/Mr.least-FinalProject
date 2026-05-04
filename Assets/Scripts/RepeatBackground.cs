using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;

    // 1.14 (1) assign the width of the background to a variable
    private float repeatWidth = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;

        // 1.14 (2) get the width of the background from the BoxCollider component
        // and divide it by 2 to get the repeat point
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    // 1.13 repeat the background when it reaches a certain point (assuming the background is 50 units wide)
    void Update()
    {
        // 1.14 (3) use the repeatWidth variable instead of a hardcoded value
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
