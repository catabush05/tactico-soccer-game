using UnityEngine;
using Unity.Cinemachine;

public class attackController : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public Camera mainCamera;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float activationDistance = 15f; // Distance from camera to activate

    private GameObject[] awayPlayers;
    private GameObject ball;
    private bool[] playerActivated;

    void Start()
    {
        mainCamera = Camera.main;

        // Find the ball
        ball = GameObject.Find("Ball");
        if (ball == null)
        {
            Debug.LogError("Ball object not found in scene!");
        }

        // Find all AwayPlayer objects
        awayPlayers = GameObject.FindGameObjectsWithTag("AwayPlayer");
        playerActivated = new bool[awayPlayers.Length];
    }

    void Update()
    {
        // Check each AwayPlayer
        for (int i = 0; i < awayPlayers.Length; i++)
        {
            if (awayPlayers[i] == null) continue;

            float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, awayPlayers[i].transform.position);
            bool isInRange = distanceFromCamera <= activationDistance;

            // Activate if in view OR in range
            if ((isInRange) && !playerActivated[i])
            {
                playerActivated[i] = true;
                Debug.Log($"AwayPlayer {i} activated!");
            }

            // If activated, move towards the ball
            if (playerActivated[i])
            {
                MoveTowardsBall(awayPlayers[i]);
            }
        }
    }

    void MoveTowardsBall(GameObject player)
    {
        // Calculate direction to ball
        Vector3 direction = (ball.transform.position - player.transform.position).normalized;

        // Move towards ball
        player.transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
