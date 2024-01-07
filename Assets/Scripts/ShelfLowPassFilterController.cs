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

    // ��������� ���������������� �������
    private float lowPassCutoffFrequency = 5000f; // ³����������� �� �����������
    private AudioLowPassFilter lowPassFilter;

    public float compassSmooth = 3f; // ���������� ������� ����� � ������ compassSmooth ��� ��������� ������������� ���������
    private float m_lastMagneticHeading = 0f; // ���������� �������� ����� � ������ m_lastMagneticHeading ��� ���������� ������������ �����������

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        Input.location.Start();
        Input.compass.enabled = true;

        // ������ ��� �������� ��������� AudioLowPassFilter
        lowPassFilter = audioSource.GetComponent<AudioLowPassFilter>();
        if (lowPassFilter == null)
        {
            lowPassFilter = audioSource.gameObject.AddComponent<AudioLowPassFilter>();
        }

        // ������������ �������� ��������� ��� ���������������� �������
        lowPassFilter.cutoffFrequency = lowPassCutoffFrequency;
        lowPassFilter.enabled = false; // ����������� �� �������������

        // ������ ������� �������� �������
        m_lastMagneticHeading = Input.compass.magneticHeading; // �������� ��������� �������� ��������� ���������
    }

    void Update()
    {
        if (Input.compass.enabled)
        {
            try
            {
                UpdateSoundSourcePosition();

                // ��������� ��������� ������� �� ����� ��������� ���������
                lowPassCutoffFrequency = 5000f + (Input.compass.magneticHeading / 180f) * 2000f;
                lowPassFilter.cutoffFrequency = lowPassCutoffFrequency;

                // ����������� ������, ���� �� ���������
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

        // �������� �������� ������� ������� ������ ���������
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
