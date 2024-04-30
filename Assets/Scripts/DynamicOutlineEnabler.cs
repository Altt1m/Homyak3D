using UnityEngine;

public class DynamicOutlineEnabler : MonoBehaviour
{
    public GameObject player; // Ссылка на объект персонажа
    public float maxDistance = 5f; // Максимальное расстояние для применения обводки

    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        // Найти объект персонажа, если ссылка на него не задана
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("player");
        }
    }

    void Update()
    {
        if (outline != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= maxDistance)
            {
                outline.enabled = true; // Включаем обводку, если расстояние меньше или равно максимальному
            }
            else
            {
                outline.enabled = false; // Отключаем обводку, если расстояние больше максимального
            }
        }
    }
}
