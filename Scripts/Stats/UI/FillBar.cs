using UnityEngine;
using UnityEngine.UI;
public class FillBar : MonoBehaviour {
    Image progressBar;

    public void ChangeFill(float maxValue, float currentValue) {
        if (progressBar == null) {
            progressBar = GetComponent<Image>();
        }
        float fillAmount = 0;
        if (currentValue != 0) {
            fillAmount = currentValue / maxValue; // Calcula el porcentaje de llenado
        }
        progressBar.fillAmount = fillAmount; // Asigna el porcentaje a la barra de progreso
    }

}
