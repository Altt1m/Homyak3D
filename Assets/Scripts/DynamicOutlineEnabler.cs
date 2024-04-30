using UnityEngine;

public class DynamicOutlineEnabler : MonoBehaviour
{
    public GameObject player; // ������ �� ������ ���������
    public float maxDistance = 5f; // ������������ ���������� ��� ���������� �������

    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        // ����� ������ ���������, ���� ������ �� ���� �� ������
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
                outline.enabled = true; // �������� �������, ���� ���������� ������ ��� ����� �������������
            }
            else
            {
                outline.enabled = false; // ��������� �������, ���� ���������� ������ �������������
            }
        }
    }
}
