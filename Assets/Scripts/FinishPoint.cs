using UnityEngine;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] bool goNextLevel;
    [SerializeField] string levelName;
    [SerializeField] float interactionDistance = 2f; // Distance at which the message will be displayed
    [SerializeField] KeyCode interactionKey = KeyCode.F; // Key to press for interaction
    [SerializeField] GameObject teleportCanvas; // Reference to the canvas displaying the message

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (goNextLevel)
            {
                SceneController.instance.NextLevel();
            }
            else
            {
                SceneController.instance.LoadScene(levelName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            teleportCanvas.SetActive(true); // Activate the canvas when the player is in range
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            teleportCanvas.SetActive(false); // Deactivate the canvas when the player exits the range
        }
    }
}
