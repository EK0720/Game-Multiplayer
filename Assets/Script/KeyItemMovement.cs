using UnityEngine;

public class KeyItemMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float movementDistance = 0.5f;
    private Vector3 startingPosition;
    private float movementDirection = 1f;
    public GameObject door;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        float movementAmount = movementSpeed * Time.deltaTime;
        float newYPosition = transform.position.y + (movementAmount * movementDirection);
        if (Mathf.Abs(newYPosition - startingPosition.y) > movementDistance)
        {
            movementDirection *= -1f;
        }
        transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}