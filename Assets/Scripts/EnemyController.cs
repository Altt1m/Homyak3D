using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    private Transform player;    // Ссылка на стартовую точку
    private List<Transform> prevTargets = new List<Transform>();
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip found;
    public AudioClip attacked;
    private bool hasPlayed = false;
    public float detectionRange = 10f;   // Дальность обнаружения игрока
    public LayerMask layerMask;
    private NavMeshAgent navMeshAgent;
    public List<Transform> targets;
    private float chaseTimer;
    private bool isChasing = false;
    public Light pointLight;
    private int i;
    private float distanceToPlayer;
    private bool playerAlive = true;
    public static bool seedsFound;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        seedsFound = false;

        TargetUpdate();
        InvokeRepeating("UpdateTarget", 10f, 50f); // Вызываем функцию обновления цели каждые 120 секунд
    }

    void UpdateTarget()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        if (Vector3.Distance(targets.Find(n => n.name == "Target Hero Building").position, player.position) >= 20)
        {
            navMeshAgent.SetDestination(player.position);
            Debug.Log("New target set: " + player.name);
        }

    }

    //private void Awake()
    //{
    //    player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
    //}


    void Update()
    {
        if(!player) return;
        
        // Проверка, находится ли игрок в пределах дальности обнаружения
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        OnTriggerEnter();

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
                        navMeshAgent.speed = 6;
                        isChasing = true;
                        if (distanceToPlayer > 2.5f) pointLight.color = Color.red;

                        if (!hasPlayed)
                        {
                            audioSource.Stop();
                            audioSource.Play();
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

            if (distanceToPlayer > detectionRange)
            {
                // Время истекло или игрок вышел из дальности обнаружения
                isChasing = false;
                navMeshAgent.speed = 10;
                audioSource.Stop();
                pointLight.color = Color.yellow;
                if (!seedsFound) TargetUpdate();
                else navMeshAgent.SetDestination(targets.Find(n => n.name == "Target Cemetery").position);
            }
        }
        else
        {
            if (navMeshAgent.remainingDistance <= 0.1 && !navMeshAgent.pathPending)
            {
                if (!seedsFound) TargetUpdate();
                else navMeshAgent.SetDestination(targets.Find(n => n.name == "Target Cemetery").position);
            }

        }
    }

    //void TargetUpdate()
    //{
    //    i = Random.Range(0, targets.Count);
    //    navMeshAgent.SetDestination(targets[i].position);
    //    Debug.Log("New target set: " + targets[i].name);
    //}

    void TargetUpdate()
    {
        // Создаем словарь для хранения пар точек и расстояний до них
        Dictionary<Transform, float> targetDistances = new Dictionary<Transform, float>();

        // Заполняем словарь и вычисляем расстояния
        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            targetDistances.Add(target, distance);
        }

        // Сортируем словарь по возрастанию расстояний
        var sortedTargets = targetDistances.OrderBy(pair => pair.Value);

        // Получаем первые пять ближайших точек
        List<Transform> closestTargets = sortedTargets.Select(pair => pair.Key).Take(6).ToList();

        // Удаляем предыдущие цели из списка ближайших
        if (prevTargets.Any())
        {
            foreach (Transform targetTransform in prevTargets)
            {
                closestTargets.Remove(targetTransform);
            }
        }

        // Выбираем случайную точку из оставшихся
        if (closestTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, closestTargets.Count);
            Transform selectedTarget = closestTargets[randomIndex];

            // Обновляем список предыдущих целей
            UpdatePrevTargets(selectedTarget);

            navMeshAgent.SetDestination(selectedTarget.position);
            Debug.Log("New target set: " + selectedTarget.name);
        }
    }

    void UpdatePrevTargets(Transform newTarget)
    {
        // Если в списке предыдущих целей уже есть 3 элемента, удаляем самый старый
        if (prevTargets.Count == 4)
        {
            prevTargets.RemoveAt(0);
        }
        prevTargets.Add(newTarget);
    }

    void OnTriggerEnter()
    {
        // Проверяем, столкнулись ли мы с коллайдером игрока
        if (distanceToPlayer < 2.5f && playerAlive)
        {
            audioSource.Stop(); audioSource.PlayOneShot(attacked);
            playerAlive = false;
            SceneTransition.SwitchToScene("Death Scene");
        }
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

