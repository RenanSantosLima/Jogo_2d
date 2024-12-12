using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private IsGroundedChecker isGroundedChecker;
    private Health health;

    //velocidade da movimentação e jump
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    [Header("Propriedades de ataque")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask attackLayer;

    //moveDirection
    private float moveDirection;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isGroundedChecker = GetComponent<IsGroundedChecker>();
        health = GetComponent<Health>();

        //ação dos eventos
        health.OnHurt += PlayerSoundHurt;
        health.OnDead += HandlePlayerDeath;
    }

    private void Start()
    {
        GameManager.Instance.inputManager.OnJump += HandleJump;
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();

        FlipSpritePlayerDirection();
    }

    //refatoração do update
    private void MovePlayer()
    {
        moveDirection = GameManager.Instance.inputManager.Movement;
        //transform.Translate(moveDirection * Time.deltaTime * moveSpeed, 0, 0);
        Vector2 directionToMove = new Vector2(moveDirection * moveSpeed, rigidBody.velocity.y);
        rigidBody.velocity = directionToMove;
    }

    private void FlipSpritePlayerDirection()
    {
        if (moveDirection < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (moveDirection > 0) {
            transform.localScale = Vector3.one;
        }
    }

    //pulo
    private void HandleJump()
    {
        if (isGroundedChecker.IsGrounded() == false) return;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        //rigidBody.velocity = Vector2.up * jumpForce;
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerJump);
    }

    //som do dano
    private void PlayerSoundHurt()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerHurt);
    }

    //ação do evento de mmorte para desativar todos os componentes
    private void HandlePlayerDeath()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerDeath);
        GetComponent<Collider2D>().enabled = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.inputManager.DisablePlayerInput();
    }

    //som andar
    private void PlayerWalkSound()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerWalk);
    }

    //ação de ataque
    private void Attack()
    {
        GameManager.Instance.AudioManager.PlaySFX(SFX.PlayerAttack);
        Collider2D[] hittedEnemies = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayer);
        print("Making enemy take damage");
        print(hittedEnemies.Length);

        foreach (Collider2D hittedEnemy in hittedEnemies) {
            print("Checking enemy");
            if (hittedEnemy.TryGetComponent(out Health enemyHealth))
            {
                print("Getting damage");
                enemyHealth.TakeDamage();
            }
        }
    }

    //raio de ataque
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }


    //teste
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform") {
            transform.parent = collision.transform;
            //print("Entrou na plataforma");
        }
    }

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.parent = null;
            //print("Saiu da plataforma");
        }
    }*/
}
