using UnityEngine;

public class PlatformHorizontal : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private bool moveRight = true;

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x < pointA.position.x) {
            moveRight = true;
        } else if (transform.position.x > pointB.position.x) {
            moveRight = false;
        }

        if (moveRight) {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        } else {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }

    //colisao do player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.parent = transform;
            //print("Entrou na plataforma");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.parent = null;
            //print("Saiu da plataforma");
        }
    }
}
