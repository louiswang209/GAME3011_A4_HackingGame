using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    private void Start()
    {
        // Set box collider
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.size = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Player completed the current hack
            collision.gameObject.GetComponent<PlayerController>().direction = Direction.STOPPED;
            gameObject.GetComponentInParent<HackingGame>().hackCompleted = true;
        }
    }
}
