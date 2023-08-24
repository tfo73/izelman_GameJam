using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public BoxCollider2D collider;
    public Rigidbody2D rb;

    private float width;
    private float scrollSpeed = -2f;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        width = collider.size.x;
        collider.enabled = false;

        // rb.velocity = new Vector2(Input.GetAxis("Vertical") * 5 * -1, rb.velocity.y); ;
    }

    void Update()
    {
        /*if (transform.position.x < -width)
        {
            Vector2 resetPosition = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }*/
        rb.velocity = new Vector2(GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity.x * -1, 0);

    }
}
