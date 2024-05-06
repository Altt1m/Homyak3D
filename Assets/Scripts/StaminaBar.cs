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
        // Обновляем заполнение полосы стамины
        UpdateStaminaBar();
        currentStamina = 100f;
    }

    void UpdateStaminaBar()
    {
        currentStamina = PlayerMovement.Stamina;

        // Вычисляем процент заполнения стамины
        float fillPercentage = currentStamina / maxStamina;

        // Применяем градиент от зеленого к красному к цвету полосы стамины
        barFillImage.color = Color.Lerp(redColor, greenColor, fillPercentage);
    }
}
