using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class KickBall : MonoBehaviour
{
    [SerializeField] float playerBounds = 2f;
    [SerializeField] float playerMoveSpeed = 5f;
    [SerializeField] float ballOffsetFromPlayer = 0.5f; // How far in front of player to place ball
    [SerializeField] float ballControlDistance = 1f; // Distance for player to have control of ball

    [SerializeField] float ballSpeed = 10f; // Speed of the ball
    [SerializeField] UIDocument uiDocument;
    [SerializeField] float awayPlayerCatchDistance = 0.5f; // Distance for away player to "catch" ball

    Button RestartButton;
    VisualElement tint;
    Label timerLabel;
    Label winText;

    private Camera mainCamera;
    private Vector3 targetPosition;
    private GameObject targetPlayer;
    private Vector3 playerTargetPosition;
    private bool isPlayerMoving = false;
    private bool isBallMoving = false;
    private bool ballHasStopped = true;
    private bool playerHasControl = false;
    private bool gameEnded = false;

    private float gameTimer = 0f;

    private InputAction clickAction;
    private InputAction pointerPositionAction;

    void Start()
    {
        mainCamera = Camera.main;

        // Setup Input System actions
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.Enable();

        pointerPositionAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/position");
        pointerPositionAction.Enable();

        // Hide restart button at start
        RestartButton = uiDocument.rootVisualElement.Q<Button>("Restart");
        RestartButton.style.display = DisplayStyle.None;
        RestartButton.clicked += ReloadScene;
        tint = uiDocument.rootVisualElement.Q<VisualElement>("tint");
        tint.style.display = DisplayStyle.None;

        // Get timer and win text UI elements
        timerLabel = uiDocument.rootVisualElement.Q<Label>("Timer");
        winText = uiDocument.rootVisualElement.Q<Label>("winText");

        if (winText != null)
        {
            winText.style.display = DisplayStyle.None;
        }

        // Initialize timer
        gameTimer = 0f;
    }

    void OnDestroy()
    {
        // Clean up input actions
        clickAction?.Disable();
        pointerPositionAction?.Disable();
    }

    void Update()
    {
        if (gameEnded) return; // Don't process input if game ended

        // Update timer
        gameTimer += Time.deltaTime;
        UpdateTimerDisplay();

        // Check if any player has control of the ball
        CheckPlayerControl();

        HandleMouseInput();

        // Move ball towards target
        if (isBallMoving)
        {
            MoveBallToTarget();
        }

        // Only move player towards target if ball has stopped
        if (isPlayerMoving && targetPlayer != null && ballHasStopped)
        {
            MovePlayerToTarget();
        }

        // Check if away player caught the ball
        CheckAwayPlayerCatch();
    }

    void HandleMouseInput()
    {
        // Check for mouse click using Input System
        if (clickAction.WasPressedThisFrame())
        {
            // Only allow clicks if a player has control of the ball
            if (!playerHasControl)
            {
                Debug.Log("Cannot pass - no player has control of the ball!");
                return;
            }

            Vector2 mousePosition = pointerPositionAction.ReadValue<Vector2>();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // Keep on 2D plane

            // Check if click is within bounds of any Player
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject nearestPlayer = null;
            GameObject playerInBounds = null;
            float nearestDistance = float.MaxValue;
            bool clickWithinBounds = false;

            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, worldPosition);

                // Check if within bounds
                if (distance <= playerBounds)
                {
                    clickWithinBounds = true;
                    playerInBounds = player;
                    break;
                }

                // Track nearest player
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPlayer = player;
                }
            }

            // If click within bounds of a player, pass ball to that player
            if (clickWithinBounds && playerInBounds != null)
            {
                // Calculate position in front of player
                Vector3 directionToClick = (worldPosition - playerInBounds.transform.position).normalized;
                Vector3 ballDestination = playerInBounds.transform.position + directionToClick * ballOffsetFromPlayer;

                // Update target to be in front of player
                targetPosition = ballDestination;

                // Start ball movement to target
                isBallMoving = true;
                ballHasStopped = false;

                // Stop player movement while ball is moving
                isPlayerMoving = false;
                Debug.Log($"Passing ball to player at {playerInBounds.name}");
            }
            // If click not within bounds, kick ball and move nearest player towards it once ball stops
            else if (!clickWithinBounds && nearestPlayer != null)
            {
                targetPosition = worldPosition;

                // Start ball movement to target
                isBallMoving = true;
                ballHasStopped = false;

                // Set up player to move once ball stops
                targetPlayer = nearestPlayer;
                playerTargetPosition = worldPosition;
                isPlayerMoving = true;

                Debug.Log($"Ball kicked! {nearestPlayer.name} will run after it stops");
            }
            else
            {
                Debug.Log("No players found with 'Player' tag");
            }
        }
    }

    void MovePlayerToTarget()
    {
        if (targetPlayer == null) return;

        // Calculate direction
        Vector3 direction = (playerTargetPosition - targetPlayer.transform.position).normalized;

        // Move player
        targetPlayer.transform.position += direction * playerMoveSpeed * Time.deltaTime;

        // Optional: Flip sprite based on direction
        if (direction.x != 0)
        {
            Vector3 scale = targetPlayer.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
            targetPlayer.transform.localScale = scale;
        }

        // Stop when close enough
        if (Vector3.Distance(targetPlayer.transform.position, playerTargetPosition) < 0.1f)
        {
            isPlayerMoving = false;
            targetPlayer.transform.position = playerTargetPosition;
            Debug.Log("Player reached target position");
        }
    }

    void MoveBallToTarget()
    {
        // Move ball towards target position
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.01f)
        {
            // Calculate direction and move
            Vector3 direction = (targetPosition - transform.position).normalized;
            float step = ballSpeed * Time.deltaTime;

            // Don't overshoot the target
            if (step >= distance)
            {
                transform.position = targetPosition;
                isBallMoving = false;
                ballHasStopped = true;
                Debug.Log("Ball has stopped at target");
            }
            else
            {
                transform.position += direction * step;
            }
        }
        else
        {
            // Ball has reached target
            transform.position = targetPosition;
            isBallMoving = false;
            ballHasStopped = true;
            Debug.Log("Ball has stopped at target");
        }
    }

    void CheckAwayPlayerCatch()
    {
        // Find all away players
        GameObject[] awayPlayers = GameObject.FindGameObjectsWithTag("AwayPlayer");

        foreach (GameObject awayPlayer in awayPlayers)
        {
            float distance = Vector3.Distance(transform.position, awayPlayer.transform.position);

            // If away player is close enough to the ball, end the game
            if (distance <= awayPlayerCatchDistance)
            {
                EndGame();
                break;
            }
        }
    }

    void CheckPlayerControl()
    {
        // Check if any home player is close enough to have control
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerHasControl = false;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // If player is close enough and ball has stopped, they have control
            if (distance <= ballControlDistance && ballHasStopped && !isBallMoving)
            {
                playerHasControl = true;
                break;
            }
        }
    }

    void EndGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log("Game Over! Away player caught the ball!");

        // Stop all movement
        isBallMoving = false;
        isPlayerMoving = false;

        // Show restart button
        RestartButton.style.display = DisplayStyle.Flex;
        tint.style.display = DisplayStyle.Flex;
    }

    void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;

        Debug.Log($"You Win! Time: {FormatTime(gameTimer)}");

        // Stop all movement
        isBallMoving = false;
        isPlayerMoving = false;

        // Show win text with time
        if (winText != null)
        {
            winText.text = $"GOAL!\nTime: {FormatTime(gameTimer)}";
            winText.style.display = DisplayStyle.Flex;
        }

        // Show restart button
        RestartButton.style.display = DisplayStyle.Flex;
        tint.style.display = DisplayStyle.Flex;
    }

    void UpdateTimerDisplay()
    {
        if (timerLabel != null)
        {
            timerLabel.text = FormatTime(gameTimer);
        }
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds * 100f) % 100f);
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // Collision detection for goal
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "goalMouth" || other.CompareTag("Goal"))
        {
            WinGame();
        }
    }

    // Public method to be called by restart button
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
