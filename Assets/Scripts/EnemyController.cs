using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform player;    // Ссылка на стартовую точку
    private Transform target;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip audioClip;
    private bool hasPlayed = false;
    public float detectionRange = 10f;   // Дальность обнаружения игрока
    public float chaseDuration = 5f;     // Продолжительность преследования
    public LayerMask layerMask;
    private NavMeshAgent navMeshAgent;
    public List<Transform> targets;
    private float chaseTimer;
    private bool isChasing = false;
    public Light pointLight;
    private int i;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();

        TargetUpdate();
    }

    void Update()
    {
        if(!player) return;
        
        // Проверка, находится ли игрок в пределах дальности обнаружения
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Стреляем рейкастом в направлении игрока
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.position - transform.position, out hit, detectionRange, layerMask))
            {
                if (hit.collider.tag == "player")
                {
                    if (!isChasing)
                    {
                        LookTarget();
                        isChasing = true;
                        pointLight.color = Color.red;
                        chaseTimer = chaseDuration;

                        if (!hasPlayed)
                        {
                            audioSource.Stop();
                            audioSource.PlayOneShot(audioClip);
                            hasPlayed = true;
                        }
                        
                    }
                    // Начинаем преследование игрока
                  
                }
            }
            
        }
        else hasPlayed = false;

        // Преследование игрока или возврат на стартовую точку
        if (isChasing)
        {
            navMeshAgent.SetDestination(player.position);
            chaseTimer -= Time.deltaTime;

            if (chaseTimer <= 0f || distanceToPlayer > detectionRange)
            {
                // Время истекло или игрок вышел из дальности обнаружения
                isChasing = false;
                TargetUpdate();
            }
        }
        else
        {
            if (navMeshAgent.transform.position == navMeshAgent.pathEndPosition)
            {
                TargetUpdate();
            }
            pointLight.color = Color.yellow;
        }
    }
    
    void TargetUpdate()
    {
        i = Random.Range(0, targets.Count);
        navMeshAgent.SetDestination(targets[i].position);
        Debug.Log("New target set: " + targets[i].name);
    }

    void LookTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        // Необходимо заменить direction.x на direction.x и direction.z на direction.z,
        // чтобы игнорировать изменения высоты, чтобы персонаж не наклонялся вверх или вниз.
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        // Используйте Quaternion.Lerp вместо Quaternion.Slerp для более плавного поворота.
        // Можно также использовать более медленное время поворота, чтобы сделать его более плавным.
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0);
    }

}

