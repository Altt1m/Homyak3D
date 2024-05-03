using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Обобщенный базовый класс для одиночек (Singleton).
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Приватное поле для хранения единственного экземпляра класса.
    private static T m_Instance;

    // Статическое свойство для доступа к единственному экземпляру.
    public static T Instance
    {
        get
        {
            return m_Instance;
        }
    }

    // Метод, вызываемый при инициализации объекта.
    protected void Awake()
    {
        // Если уже есть экземпляр и он не равен текущему объекту, уничтожаем текущий объект.
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // В противном случае сохраняем текущий объект как единственный экземпляр.
            m_Instance = GetComponent<T>();
        }
    }
}

