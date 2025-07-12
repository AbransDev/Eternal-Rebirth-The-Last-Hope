using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Cactus : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackRadius = 1f;
    public float attackDamage = 10f;
    public float rotationSpeed = 5f;
    public float patrolRadius = 10f; // Devriye yar��ap�
    public int numberOfPatrolPoints = 5; // Devriye noktas� say�s�
    public Animator animator;
    private Transform player;

    private int currentPatrolIndex;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isAttacking = false;
    private List<Vector3> patrolPoints = new List<Vector3>();

    private enum State { Patrol, Idle, Chase, Attack }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        currentPatrolIndex = 0;
        currentState = State.Patrol;
        GenerateRandomPatrolPoints();
        GoToNextPatrolPoint();
        UpdateAnimator();
        
        player=GameManager.Instance.Player.transform;
    }

    void Update()
    {
        if (isAttacking) return;

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Idle:
                if (!IsInvoking("StartPatrol"))
                {
                    Invoke("StartPatrol", 3f); // 3 saniye sonra tekrar devriye gezecek
                }
                break;

            case State.Chase:
                Chase();
                if (distanceToPlayer < attackRange)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange)
                {
                    currentState = State.Patrol;
                    GoToNextPatrolPoint();
                }
                break;

            case State.Attack:
                Attack();
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chase;
                }
                break;
        }

        UpdateAnimator();
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Idle;
        }
    }

    void Chase()
    {
        agent.destination = player.position;
    }

    void Attack()
    {
        isAttacking = true;
        agent.isStopped = true; // NavMeshAgent'� durdur

        // Rigidbody hareketini durdur
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Sald�r� animasyonunu oynat
        animator.SetBool("IsAttacking", true);

        // Oyuncuya hasar verme i�lemi
        DealDamage();

        // Sald�r� s�resi kadar bekle
        Invoke("EndAttack", 1f); // �rne�in 1 saniye sonra sald�r�y� bitir
    }

    void DealDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Oyuncuya hasar ver
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        agent.isStopped = false; // NavMeshAgent'� yeniden ba�lat
        animator.SetBool("IsAttacking", false);
        currentState = State.Chase; // Sald�r�dan sonra kovalamaya devam et

        // Sald�r�dan sonra d��man� oyuncuya do�ru d�nd�r
        RotateTowardsPlayer();
    }

    void StartPatrol()
    {
        currentState = State.Patrol;
        GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex];
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsRunning", currentState == State.Patrol || currentState == State.Chase);
        animator.SetBool("IsIdle", currentState == State.Idle);
        animator.SetBool("IsAttacking", currentState == State.Attack);
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // Sald�r� yar��ap�n� g�rselle�tirmek i�in
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Devriye noktalar�n� g�rselle�tirmek i�in
        Gizmos.color = Color.blue;
        foreach (var point in patrolPoints)
        {
            Gizmos.DrawWireSphere(point, 0.5f);
        }
    }

    void GenerateRandomPatrolPoints()
    {
        patrolPoints.Clear();
        for (int i = 0; i < numberOfPatrolPoints; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * patrolRadius;
            randomPoint.y = transform.position.y; // Y�ksekli�i d��manla ayn� tut
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                patrolPoints.Add(hit.position);
            }
        }
    }
}