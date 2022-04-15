using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingGame : MonoBehaviour
{
    // Arrays are used here in case this system is expanded to potentially generate levels procedurally, or have different possible start/end positions
    [SerializeField]
    public Transform[] startPositions;
    [SerializeField]
    private Transform[] endPositions;

    private PlayerController player;
    private FinishLine finishLine;

    // Used to check if the game was completed or not
    public bool hackCompleted = false;
    // Used for if we want to pause the game while something is displayed to the player (ie. animation, popup, etc.)
    private bool gamePaused = false;

    private GameManager gameManager;

    private void Awake()
    {
        hackCompleted = false;
        gamePaused = false;

        // Set player position
        player = gameObject.GetComponentInChildren<PlayerController>();
        player.transform.position = startPositions[0].position;

        // Set finish line position
        finishLine = gameObject.GetComponentInChildren<FinishLine>();
        finishLine.transform.position = endPositions[0].position;

        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gamePaused == false)
        {
            if (hackCompleted == true)
            {
                Debug.Log("Firewall breached!");

                gamePaused = true;
                gameManager.HackCompleted();

                // Destroy the current hack minigame
                Destroy(this.gameObject);
            }
        }
    }
}
