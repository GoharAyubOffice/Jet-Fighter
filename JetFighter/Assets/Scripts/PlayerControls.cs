using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Speed and ClampedRange Setting")]

    [Tooltip("Speed of Jet")]
    [SerializeField] float controlSpeed = 10f;

    [Tooltip("Horizontal Clamped Range of Screen")]
    [SerializeField] float xclampedRange = 5f;

    [Tooltip("Verticle Clamped Range of Screen")]
    [SerializeField] float yclampedRange = 5f;

    [Header("Screen Position Based Tuning")]
    [SerializeField] float positionPitchfactor = -2f;
    [SerializeField] float positionyawfactor = 3.5f;

    [Header("Playeer Input Based Tuning")]
    [SerializeField] float controlPitchfactor = 15f;
    [SerializeField] float controlrollfactor = -15f;

    [Header("Laser Gamobject for Particle system")]
    [Tooltip("Add Player Lasers Here")]
    [SerializeField] GameObject[] lasers;

    float horizontal, vertical;

    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }
    void ProcessRotation()
    {

        float pitchduetoposition = transform.localPosition.y * positionPitchfactor;
        float pitchduetocontrolfactor = vertical * controlPitchfactor;

        float yawduetoposition = transform.localPosition.x * positionyawfactor;

        float pitch = pitchduetoposition + pitchduetocontrolfactor;
        float yaw = yawduetoposition;
        float roll = horizontal * controlrollfactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        float xoffset = horizontal * controlSpeed * Time.deltaTime;
        float rawXpos = transform.localPosition.x + xoffset;
        float clampedxPos = Mathf.Clamp(rawXpos, -xclampedRange, xclampedRange);

        float yoffset = vertical * controlSpeed * Time.deltaTime;
        float rawYpos = transform.localPosition.y + yoffset;
        float clampedYposition = Mathf.Clamp(rawYpos, -yclampedRange, yclampedRange);

        transform.localPosition = new Vector3(clampedxPos, clampedYposition, transform.localPosition.z);
    }
    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {

            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}