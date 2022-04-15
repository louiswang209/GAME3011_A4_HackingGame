using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardCollider : MonoBehaviour
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
            // Send player back to start position
            collision.gameObject.GetComponent<PlayerController>().gameObject.transform.position = gameObject.GetComponentInParent<HackingGame>().startPositions[0].position;
            collision.gameObject.GetComponent<PlayerController>().direction = Direction.STOPPED;
            collision.gameObject.GetComponent<PlayerController>().PlayHazardSound();
        }
    }
}
