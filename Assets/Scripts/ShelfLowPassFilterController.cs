using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ShelfLowPassFilterController : MonoBehaviour
{
    public TextMeshProUGUI log;
    public TextMeshProUGUI magnetometerValue;
    public Toggle filterToggle;
    public GameObject soundSourceSphere;
    public GameObject ourObjects;
    private AudioSource audioSource;

    private float lastMagneticHeading = 0f;
    private float rotationSpeed = 30f;

    // Параметри низькочастотного фільтра
    private float lowPassCutoffFrequency = 5000f; // Відрегульвуємо за необхідності
    private AudioLowPassFilter lowPassFilter;

    public float compassSmooth = 3f; // Оголошуючи публічну змінну з назвою compassSmooth для управління згладжуванням обертання
    private float m_lastMagneticHeading = 0f; // Оголошуючи приватну змінну з назвою m_lastMagneticHeading для збереження попереднього магнітометру

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Input.location.Start();
        Input.compass.enabled = true;

        // Додаємо або отримуємо компонент AudioLowPassFilter
        lowPassFilter = audioSource.GetComponent<AudioLowPassFilter>();
        if (lowPassFilter == null)
        {
            lowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
        }

        // Встановлюємо початкові параметри для низькочастотного фільтра
        lowPassFilter.cutoffFrequency = lowPassCutoffFrequency;
        lowPassFilter.enabled = false; // Заблоковано за замовчуванням

        // Кешуємо поточне значення компаса
        m_lastMagneticHeading = Input.compass.magneticHeading; // Зберігаємо початкове значення магнітного заголовку
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            try
            {
                UpdateSoundSourcePosition();

                // Оновлюємо параметри фільтра на основі магнітного заголовку
                lowPassCutoffFrequency = 5000f + (Input.compass.magneticHeading / 180f) * 2000f;
                lowPassFilter.cutoffFrequency = lowPassCutoffFrequency;

                // Застосовуємо фільтр, якщо він увімкнений
                if (filterToggle.isOn)
                {
                    EnableLowPassFilter();
                }
                else
                {
                    DisableLowPassFilter();
                }
            }
            catch (Exception e)
            {
                log.text = e.Message;
            }
        }
    }

    void UpdateSoundSourcePosition()
    {
        magnetometerValue.text = Input.compass.magneticHeading.ToString("0.");
        float rotationAmount = Input.compass.magneticHeading - lastMagneticHeading;
        Vector3 rotationAxis = Vector3.up;
        Vector3 rotationCenter = ourObjects.transform.position;

        // Обертаємо звуковий джерело навколо центру обертання
        soundSourceSphere.transform.RotateAround(rotationCenter, rotationAxis, rotationAmount * rotationSpeed * Time.deltaTime);

        lastMagneticHeading = Input.compass.magneticHeading;
    }

    void EnableLowPassFilter()
    {
        lowPassFilter.enabled = true;
    }

    void DisableLowPassFilter()
    {
        lowPassFilter.enabled = false;
    }
}
