using UnityEngine;

public class Platform : MonoBehaviour
{
    //velociadade e qual plataforma
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool platform1, platform2;
    private bool moveRight = true;
    private bool moveUp = true;

    // Update is called once per frame
    void Update()
    {
        if (platform1) {

            if (transform.position.x > -5) {
                moveRight = false;

            } else if (transform.position.x < -8) {
                moveRight = true;
            }

            if (moveRight) {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            } else {
                transform.Translate(Vector2.right * -moveSpeed * Time.deltaTime);
            }
        }

        if (platform2) {

            if (transform.position.y > 5) {
                moveUp = false;

            } else if (transform.position.y < 0) {
                moveUp = true;
            }

            if (moveUp) {
                transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            } else {
                transform.Translate(Vector2.up * -moveSpeed * Time.deltaTime);
            }
        }
    }
}
