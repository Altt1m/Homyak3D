using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Image barFillImage;
    public float maxStamina = PlayerMovement.MaxStamina;
    public float currentStamina;
    public Color greenColor;
    public Color redColor;

    void Update()
    {
        // ��������� ���������� ������ �������
        UpdateStaminaBar();
        currentStamina = 100f;
    }

    void UpdateStaminaBar()
    {
        currentStamina = PlayerMovement.Stamina;

        // ��������� ������� ���������� �������
        float fillPercentage = currentStamina / maxStamina;

        // ��������� �������� �� �������� � �������� � ����� ������ �������
        barFillImage.color = Color.Lerp(redColor, greenColor, fillPercentage);
    }
}
