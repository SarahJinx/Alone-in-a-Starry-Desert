using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI References")]
    public Slider sensitivitySlider;
    public Slider fovSlider;

    [Header("Camera Reference")]
    public Camera mainCamera;
    public ThirdPersonCamera cameraController;

    private void Start()
    {
        // Load saved settings or default values
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 60f);

        ApplySettings();
    }

    public void ApplySettings()
    {
        if (cameraController)
            cameraController.sensitivity = sensitivitySlider.value;

        if (mainCamera)
            mainCamera.fieldOfView = fovSlider.value;

        // Save preferences
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.SetFloat("FOV", fovSlider.value);
        PlayerPrefs.Save();
    }

    public void OnSensitivityChanged(float value)
    {
        ApplySettings();
    }

    public void OnFOVChanged(float value)
    {
        ApplySettings();
    }
}
