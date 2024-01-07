using System; 
using TMPro; 
using UnityEngine;
using UnityEngine.UI; 

public class MagnetometerController : MonoBehaviour 
{
    public float compassSmooth = 3f; // Оголошуючи публічну змінну з назвою compassSmooth для управління згладжуванням обертання
    private float m_lastMagneticHeading = 0f; // Оголошуючи приватну змінну з назвою m_lastMagneticHeading для збереження попереднього магнітометру

    public Button enableMagnetometer; // Оголошуючи публічну посилання на об'єкт Button з назвою enableMagnetometer
    public TextMeshProUGUI magnetometerValue; // Оголошуючи публічну посилання на об'єкт TextMeshProUGUI з назвою magnetometerValue

    private TextMeshProUGUI enableMagnetometerButtonText; // Оголошуючи приватне посилання на об'єкт TextMeshProUGUI для тексту кнопки

    void Start() // Метод Start() викликається при завантаженні скрипта
    {
        Debug.Log("SCRIPT IS WORKING"); // Друкуємо повідомлення в консолі, що скрипт працює

        // Для того щоб магнітометр був ініціалізований до справжнього північного полюса, запускаємо службу місцезнаходження,
        // щоб Unity могло коригувати локальні відхилення:
        Input.location.Start(); // Запускаємо службу місцезнаходження для отримання більш точних вимірювань компаса

        // Запускаємо компас.
        Input.compass.enabled = true; // Активуємо сенсор компаса для доступу до даних магнітомера

        enableMagnetometer.onClick.AddListener(ToggleMagnetometer); // Додаємо слухач подій до кнопки enableMagnetometer, який викликає метод ToggleMagnetometer() при кліку

        enableMagnetometerButtonText = enableMagnetometer.GetComponentInChildren<TextMeshProUGUI>(); // Отримуємо посилання на об'єкт TextMeshProUGUI в межах кнопки enableMagnetometer

        enableMagnetometerButtonText.text = "Enabled"; // Встановлюємо текст кнопки enableMagnetometer на "Enabled"

        // Кешуємо поточне значення компаса
        m_lastMagneticHeading = Input.compass.magneticHeading; // Зберігаємо початкове значення магнітного заголовку
    }

    // Метод Update() викликається кожен кадр
    private void Update()
    {
        try // Блок try-catch для обробки можливих винятків
        {
            // Перевіряємо, чи увімкнено магнітометр
            if (Input.compass.enabled) // Перевіряємо, чи активований компас
            {
                // Оновлюємо текстове поле UI поточним значенням магнітного заголовку
                magnetometerValue.text = Input.compass.magneticHeading.ToString("0."); // Відображаємо значення магнітнометру з однією десятковою частиною

                // Плавно інтерполюємо між поточним обертанням та цільовим обертанням
                Quaternion targetRotation = Quaternion.Euler(0, Input.compass.magneticHeading, 0); // Обчислюємо цільове обертання на основі магнітного заголовку
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, compassSmooth * Time.deltaTime); // Плавно обертаємо об'єкт в напрямку цільового обертання

                // Перевіряємо, чи значення магнітного заголовку значуще змінилося
                if (Math.Abs(Input.compass.magneticHeading - m_lastMagneticHeading) > compassSmooth) // Перевіряємо, чи значення компаса значуще змінилося
                {
                    // Оновлюємо останнє значення магнітного заголовку
                    m_lastMagneticHeading = Input.compass.magneticHeading; // Оновлюємо збережене значення магнітометру
                }
            }
        }
        catch (Exception ex) // Ловимо будь-які винятки, які можуть виникнути
        {
            // Обробляємо винятки тут
        }
    }

    // Метод ToggleMagnetometer() для увімкнення або вимкнення магнітометра
    private void ToggleMagnetometer()
    {
        if (Input.compass.enabled) // Якщо компас зараз увімкнений
        {
            DisableMagnetometer(); // Вимикаємо магнітометр
        }
        else // Якщо компас зараз вимкнений
        {
            EnableMagnetometer(); // Увімкнемо магнітометр
        }
    }

    // Метод EnableMagnetometer() для увімкнення магнітометра
    private void EnableMagnetometer()
    {
        Input.compass.enabled = true; // Активуємо сенсор компаса
        enableMagnetometerButtonText.text = "Enabled"; // Встановлюємо текст кнопки на "Enabled"
    }

    // Метод DisableMagnetometer() для вимкнення магнітометра
    private void DisableMagnetometer()
    {
        Input.compass.enabled = false; // Вимикаємо сенсор компаса
        enableMagnetometerButtonText.text = "Disabled"; // Встановлюємо текст кнопки на "Disabled"
    }
}
