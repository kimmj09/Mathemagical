using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MagicSystem.UI
{
    public class PneumaTuningSelector : MonoBehaviour
    {
        [Header("UI Components")]
        public Slider spectrumSlider;         // 0.0 (극단적 집중) ~ 1.0 (극단적 확산)
        public TextMeshProUGUI valueText;     // 현재 수치 표기 (예: "집중 80%", "확산 40%")

        [Header("Tuning Value")]
        [Range(0f, 1f)]
        public float currentSpectrum = 0.5f;  // 기본값 0.5 (균형)

        public float CurrentSpectrum => currentSpectrum;

        private void Awake()
        {
            if (spectrumSlider != null)
            {
                spectrumSlider.minValue = 0f;
                spectrumSlider.maxValue = 1f;
                spectrumSlider.value = currentSpectrum;
                spectrumSlider.onValueChanged.AddListener(OnSliderValueChanged);
            }
            UpdateUI(currentSpectrum);
        }

        private void OnSliderValueChanged(float value)
        {
            currentSpectrum = value;
            UpdateUI(value);
        }

        private void UpdateUI(float value)
        {
            if (valueText == null) return;

            if (value < 0.4f)
            {
                float focusPercent = (0.5f - value) * 200f;
                valueText.text = $"[집중 특화] +{focusPercent:F0}%";
            }
            else if (value > 0.6f)
            {
                float spreadPercent = (value - 0.5f) * 200f;
                valueText.text = $"[확산 특화] +{spreadPercent:F0}%";
            }
            else
            {
                valueText.text = "[균형 튜닝]";
            }
        }
    }
}