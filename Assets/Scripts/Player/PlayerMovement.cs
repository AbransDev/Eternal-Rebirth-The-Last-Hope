using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Image manaBar;
    public Transform bulletSpawn;
    public GameObject bullet;

    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float bulletSpeed = 20f;

    [SerializeField] GameObject vfxPrefab;
    [SerializeField] Transform staffTip;
    [SerializeField] float vfxSpawnDistance = 1f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackOffset = 1f;
    [SerializeField] float manaRegenRate = 10f;
    [SerializeField] int maxMana = 100;
    [SerializeField] float attackCooldown = 2f; // Saldırı için cooldown süresi

    private AudioSource audioSource;
 
    private Rigidbody rb;
    private Animator animator;
    private float turnSmoothVelocity;
    private float lastAttackTime = -Mathf.Infinity; // Son saldırı zamanı
    private bool isAttacking = false;
    public int mana = 100;

    public bool CanMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        StartCoroutine(RegenerateMana());
        UpdateManaBar();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (CanMove)
        {
            Move(moveX, moveZ, speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        animator.SetBool("isAttacking", Input.GetKey(KeyCode.Mouse0) && !isAttacking);

        animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        animator.SetBool("isMoving", moveX != 0f || moveZ != 0f);

        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("isLooting", true);
            SpawnVFX();
            audioSource.Play();
        }
        else
        {
            animator.SetBool("isLooting", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time; // Son saldırı zamanını güncelle
            PerformAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isAttacking && mana >= 30)
        {
            mana -= 15;
            UpdateManaBar();
            animator.SetBool("rangedAttack", true);
            ShootBullet();
        }
        else
        {
            animator.SetBool("rangedAttack", false);
        }
    }

    void Move(float moveX, float moveZ, float speed)
    {
        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 velocity = movement * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (movement != Vector3.zero)
        {
            float targetRotation = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            rb.MoveRotation(Quaternion.Euler(0f, smoothRotation, 0f));
        }
    }

    void SpawnVFX()
    {
        if (vfxPrefab != null && staffTip != null)
        {
            Vector3 spawnPosition = staffTip.position + staffTip.forward * vfxSpawnDistance;
            Quaternion spawnRotation = staffTip.rotation * Quaternion.Euler(180f, 0f, 0f);
            Instantiate(vfxPrefab, spawnPosition, spawnRotation);
        }
    }

    void PerformAttack()
    {
        isAttacking = true;

        Vector3 attackPosition = transform.position + transform.forward * attackOffset;
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
        isAttacking = false;
    }

    void ShootBullet()
    {
        if (bullet != null && bulletSpawn != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(1000); // Eğer hiçbir şeye çarpmazsa, ray yönünde uzak bir noktayı hedef al
            }

            Vector3 direction = (targetPoint - bulletSpawn.position).normalized;

            GameObject spawnedBullet = Instantiate(bullet, bulletSpawn.position, Quaternion.LookRotation(direction));
            Rigidbody bulletRb = spawnedBullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.useGravity = false;
                bulletRb.velocity = direction * bulletSpeed;
            }
            Destroy(spawnedBullet, 3f);
        }
    }

    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            mana = Mathf.Min(mana + (int)manaRegenRate, maxMana);
            UpdateManaBar();
        }
    }

    void UpdateManaBar()
    {
        if (manaBar != null)
        {
            manaBar.fillAmount = (float)mana / maxMana;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPosition = transform.position + transform.forward * attackOffset;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
