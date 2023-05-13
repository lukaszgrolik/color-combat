using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    TopDown,
    Isometric,
    Perspective,
}

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; public Camera MainCamera => mainCamera;
    [SerializeField] private Camera camTopDown;
    [SerializeField] private Camera camIso;
    [SerializeField] private Camera camPersp;

    public event System.Action beforeSwitch;
    public event System.Action<Camera> switched;

    public void Activate(CameraMode mode)
    {
        if (mode == CameraMode.TopDown) ApplyCamera(camTopDown);
        else if (mode == CameraMode.Isometric) ApplyCamera(camIso);
        else if (mode == CameraMode.Perspective) ApplyCamera(camPersp);
    }

    void ApplyCamera(Camera cam)
    {
        beforeSwitch?.Invoke();

        mainCamera.orthographic = cam.orthographic;
        mainCamera.transform.position = mainCamera.transform.position.With(y: cam.transform.position.y);
        mainCamera.transform.rotation = cam.transform.rotation;

        if (cam.orthographic)
        {

        }
        else
        {
            mainCamera.fieldOfView = cam.fieldOfView;
        }

        switched?.Invoke(mainCamera);
    }
}
