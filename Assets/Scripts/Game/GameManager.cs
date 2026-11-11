
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text checkpointText;
    public GameObject finishPanel;
    public TMP_Text finalTimeText;

    [Header("Player References")]
    public PlayerMovement playerMovement;
    public ThirdPersonCamera thirdPersonCamera;

    [Header("Victory Effects")]
    public ParticleSystem finishConfetti;

    private float elapsedTime = 0f;
    private int currentCheckpoint = 0;

    public bool IsFinished { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        elapsedTime = 0f;
        currentCheckpoint = 0;
        IsFinished = false;

        if (finishPanel != null)
            finishPanel.SetActive(false);

        if (checkpointText != null)
            checkpointText.text = "Checkpoint: Start";

        UpdateTimer();
    }

    private void Update()
    {
        if (IsFinished)
            return;

        elapsedTime += Time.deltaTime;
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (timerText != null)
            timerText.text = "TIME  " + FormatTime(elapsedTime);
    }

    private string FormatTime(float time)
    {
        int totalCentiseconds = Mathf.FloorToInt(time * 100f);

        int minutes = totalCentiseconds / 6000;
        int seconds = (totalCentiseconds / 100) % 60;
        int centiseconds = totalCentiseconds % 100;

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, centiseconds);
    }

    public void ActivateCheckpoint(int number, Vector3 position, PlayerMovement player)
    {
        if (IsFinished || number <= currentCheckpoint)
            return;

        currentCheckpoint = number;
        player.SetCheckpoint(position);

        if (checkpointText != null)
        {
            checkpointText.text = "Checkpoint: " + currentCheckpoint;
        }

        Debug.Log("Checkpoint " + number + " activated!");
    }

    public void FinishGame()
    {
        if (IsFinished)
            return;

        IsFinished = true;

        if (finishConfetti != null)
            finishConfetti.Play();

        if (finalTimeText != null)
        {
            finalTimeText.text = "FINAL TIME\n" + FormatTime(elapsedTime);
        }

        if (finishPanel != null)
            finishPanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Game Finished! " + FormatTime(elapsedTime));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
