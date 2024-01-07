using System; 
using TMPro; 
using UnityEngine;
using UnityEngine.UI; 

public class MagnetometerController : MonoBehaviour 
{
    public float compassSmooth = 3f; // ���������� ������� ����� � ������ compassSmooth ��� ��������� ������������� ���������
    private float m_lastMagneticHeading = 0f; // ���������� �������� ����� � ������ m_lastMagneticHeading ��� ���������� ������������ �����������

    public Button enableMagnetometer; // ���������� ������� ��������� �� ��'��� Button � ������ enableMagnetometer
    public TextMeshProUGUI magnetometerValue; // ���������� ������� ��������� �� ��'��� TextMeshProUGUI � ������ magnetometerValue

    private TextMeshProUGUI enableMagnetometerButtonText; // ���������� �������� ��������� �� ��'��� TextMeshProUGUI ��� ������ ������

    void Start() // ����� Start() ����������� ��� ����������� �������
    {
        Debug.Log("SCRIPT IS WORKING"); // ������� ����������� � ������, �� ������ ������

        // ��� ���� ��� ���������� ��� ������������� �� ����������� �������� ������, ��������� ������ ���������������,
        // ��� Unity ����� ���������� ������� ���������:
        Input.location.Start(); // ��������� ������ ��������������� ��� ��������� ���� ������ ��������� �������

        // ��������� ������.
        Input.compass.enabled = true; // �������� ������ ������� ��� ������� �� ����� ����������

        enableMagnetometer.onClick.AddListener(ToggleMagnetometer); // ������ ������ ���� �� ������ enableMagnetometer, ���� ������� ����� ToggleMagnetometer() ��� ����

        enableMagnetometerButtonText = enableMagnetometer.GetComponentInChildren<TextMeshProUGUI>(); // �������� ��������� �� ��'��� TextMeshProUGUI � ����� ������ enableMagnetometer

        enableMagnetometerButtonText.text = "Enabled"; // ������������ ����� ������ enableMagnetometer �� "Enabled"

        // ������ ������� �������� �������
        m_lastMagneticHeading = Input.compass.magneticHeading; // �������� ��������� �������� ��������� ���������
    }

    // ����� Update() ����������� ����� ����
    private void Update()
    {
        try // ���� try-catch ��� ������� �������� �������
        {
            // ����������, �� �������� ����������
            if (Input.compass.enabled) // ����������, �� ����������� ������
            {
                // ��������� �������� ���� UI �������� ��������� ��������� ���������
                magnetometerValue.text = Input.compass.magneticHeading.ToString("0."); // ³��������� �������� ������������ � ���� ���������� ��������

                // ������ ������������ �� �������� ���������� �� �������� ����������
                Quaternion targetRotation = Quaternion.Euler(0, Input.compass.magneticHeading, 0); // ���������� ������� ��������� �� ����� ��������� ���������
                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, compassSmooth * Time.deltaTime); // ������ �������� ��'��� � �������� ��������� ���������

                // ����������, �� �������� ��������� ��������� ������� ��������
                if (Math.Abs(Input.compass.magneticHeading - m_lastMagneticHeading) > compassSmooth) // ����������, �� �������� ������� ������� ��������
                {
                    // ��������� ������ �������� ��������� ���������
                    m_lastMagneticHeading = Input.compass.magneticHeading; // ��������� ��������� �������� �����������
                }
            }
        }
        catch (Exception ex) // ������ ����-�� �������, �� ������ ���������
        {
            // ���������� ������� ���
        }
    }

    // ����� ToggleMagnetometer() ��� ��������� ��� ��������� �����������
    private void ToggleMagnetometer()
    {
        if (Input.compass.enabled) // ���� ������ ����� ���������
        {
            DisableMagnetometer(); // �������� ����������
        }
        else // ���� ������ ����� ���������
        {
            EnableMagnetometer(); // �������� ����������
        }
    }

    // ����� EnableMagnetometer() ��� ��������� �����������
    private void EnableMagnetometer()
    {
        Input.compass.enabled = true; // �������� ������ �������
        enableMagnetometerButtonText.text = "Enabled"; // ������������ ����� ������ �� "Enabled"
    }

    // ����� DisableMagnetometer() ��� ��������� �����������
    private void DisableMagnetometer()
    {
        Input.compass.enabled = false; // �������� ������ �������
        enableMagnetometerButtonText.text = "Disabled"; // ������������ ����� ������ �� "Disabled"
    }
}
