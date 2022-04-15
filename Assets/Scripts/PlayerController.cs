using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float playerSpeed = 5.0f;
    [SerializeField]
    private float inputDelayAfterMoving = 0.5f;

    public Direction direction;

    [Header("References")]
    [SerializeField]
    private AudioClip[] audioClips;

    private AudioSource audioSource;
    private BoxCollider2D boxCollider;

    private bool canMove = true;

    private void Start()
    {
        // Setting audio source
        audioSource = GetComponent<AudioSource>();

        // Setting box collider
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = gameObject.GetComponent<RectTransform>().sizeDelta;

        // Making player start in idle/stopped state
        direction = Direction.STOPPED;
    }

    // Update is called once per frame
    void Update()
    {
        // If the hacking game isn't completed prevent the player's input (this is here in case I want to add UI animations after a hack and don't want the player to move)
        if (gameObject.GetComponentInParent<HackingGame>().hackCompleted == false)
        {
            PlayerKeyDown();
            MovePlayer();
        }
    }

    private void PlayerKeyDown()
    {
        if (canMove == true)
        {
            if (Input.GetKeyDown(KeyCode.W) && (direction != Direction.Down && direction != Direction.Up))
            {
                direction = Direction.Up;
                StartCoroutine(nameof(PreventInput));
            }

            if (Input.GetKeyDown(KeyCode.S) && (direction != Direction.Up && direction != Direction.Down))
            {
                direction = Direction.Down;
                StartCoroutine(nameof(PreventInput));
            }

            if (Input.GetKeyDown(KeyCode.A) && (direction != Direction.Right && direction != Direction.Left))
            {
                direction = Direction.Left;
                StartCoroutine(nameof(PreventInput));
            }

            if (Input.GetKeyDown(KeyCode.D) && (direction != Direction.Left && direction != Direction.Right))
            {
                direction = Direction.Right;
                StartCoroutine(nameof(PreventInput));
            }
        }
    }

    private void MovePlayer()
    {
        switch (direction)
        {
            case Direction.Up:
                transform.position = new Vector2(transform.position.x, transform.position.y + playerSpeed * Time.deltaTime);
                break;
            case Direction.Down:
                transform.position = new Vector2(transform.position.x, transform.position.y - playerSpeed * Time.deltaTime);
                break;
            case Direction.Left:
                transform.position = new Vector2(transform.position.x - playerSpeed * Time.deltaTime, transform.position.y);
                break;
            case Direction.Right:
                transform.position = new Vector2(transform.position.x + playerSpeed * Time.deltaTime, transform.position.y);
                break;
            case Direction.STOPPED:
                transform.position = transform.position;
                break;
        }
    }

    IEnumerator PreventInput()
    {
        canMove = false;
        audioSource.clip = audioClips[0];
        audioSource.Play();

        yield return new WaitForSeconds(inputDelayAfterMoving);

        canMove = true;
    }

    ///////// Sounds /////////
    
    public void PlayHazardSound()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
}

// Various directions the player can move towards
public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    STOPPED
}
