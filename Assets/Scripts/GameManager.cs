using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Difficulty difficulty = Difficulty.Easy;
    [SerializeField]
    private TMP_Text difficultyText;
    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text firewallsText;

    [SerializeField]
    private GameObject difficultyButtons;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject closeButton;

    [SerializeField]
    private AudioClip[] audioClips;

    private AudioSource audioSource;

    public int firewallsBreached = 0;
    private int numFirewalls = 3;

    [Header("Hack Prefabs")]
    [SerializeField]
    private GameObject[] easyHacks;
    [SerializeField]
    private GameObject[] mediumHacks;
    [SerializeField]
    private GameObject[] hardHacks;

    private float timeRemaining = 15.0f;
    private float additionalTime = 15.0f;

    private void Start()
    {
        firewallsBreached = 0;
        audioSource = GetComponent<AudioSource>();
    }

    private void Timer()
    {
        timerText.text = $"Time Remaining: {timeRemaining}";

        // Player still has time
        if (timeRemaining > 0.0f)
        {
            timeRemaining -= 1.0f;
        }
        else // Player is out of time
        {
            Destroy(GetComponentInChildren<HackingGame>().gameObject);
            closeButton.SetActive(false);
            difficultyButtons.SetActive(true);
            startButton.SetActive(true);
            PlayHackFailedSound();
            CancelInvoke(nameof(Timer));
        }
    }

    public void StartGame()
    {
        firewallsBreached = 0;

        timeRemaining = 20.0f;

        switch (difficulty)
        {
            case Difficulty.Easy:
                numFirewalls = 3;
                additionalTime = 20.0f;
                break;
            case Difficulty.Medium:
                numFirewalls = 4;
                additionalTime = 15.0f;
                break;
            case Difficulty.Hard:
                numFirewalls = 5;
                additionalTime = 20.0f;
                break;
        }

        firewallsText.text = $"Firewalls Remaining: {numFirewalls - firewallsBreached}";

        // Update timer every second
        InvokeRepeating(nameof(Timer), 0.0f, 1.0f);

        BeginNewHack();

        difficultyButtons.SetActive(false);
        startButton.SetActive(false);
        closeButton.SetActive(true);
    }

    public void HackCompleted()
    {
        // Increment number of firewalls that have been breached
        firewallsBreached++;
        // Add time to the timer
        timeRemaining += additionalTime;

        // Update text
        firewallsText.text = $"Firewalls Remaining: {numFirewalls - firewallsBreached}";

        // Player successfully hacked system
        if (firewallsBreached >= numFirewalls)
        {
            CancelInvoke(nameof(Timer));

            PlayHackCompleteSound();

            closeButton.SetActive(false);
            difficultyButtons.SetActive(true);
            startButton.SetActive(true);
        }
        else // Begin another hack
        {
            BeginNewHack();
        }
    }

    public void BeginNewHack()
    {
        int randomNum;

        switch (difficulty)
        {
            case Difficulty.Easy:
                randomNum = Random.Range(0, easyHacks.Length);
                Instantiate(easyHacks[randomNum], transform);
                Debug.Log(randomNum);
                break;
            case Difficulty.Medium:
                randomNum = Random.Range(0, mediumHacks.Length);
                Instantiate(mediumHacks[randomNum], transform);
                break;
            case Difficulty.Hard:
                randomNum = Random.Range(0, hardHacks.Length);
                Instantiate(hardHacks[randomNum], transform);
                break;
        }

        PlayFirewallBreachedSound();
    }

    // Sounds

    public void PlayHackCompleteSound()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    public void PlayHackFailedSound()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    public void PlayFirewallBreachedSound()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    // Buttons

    public void EasyButton()
    {
        difficulty = Difficulty.Easy;
        difficultyText.text = "Current Difficulty: Easy";
    }

    public void MediumButton()
    {
        difficulty = Difficulty.Medium;
        difficultyText.text = "Current Difficulty: Medium";
    }

    public void HardButton()
    {
        difficulty = Difficulty.Hard;
        difficultyText.text = "Current Difficulty: Hard";
    }

    public void CloseButton()
    {
        CancelInvoke(nameof(Timer));
        Destroy(GetComponentInChildren<HackingGame>().gameObject);

        closeButton.SetActive(false);
        difficultyButtons.SetActive(true);
        startButton.SetActive(true);
    }

}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
