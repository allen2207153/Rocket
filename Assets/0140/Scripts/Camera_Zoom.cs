using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Camera_Zoom : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private float zoomfactor;

    private float defaultFov;
    private Camera mainCamera;
    private bool isZooming = false;

    private void Start()
    {
        mainCamera = Camera.main;
        camera = mainCamera.transform;
        defaultFov = mainCamera.fieldOfView;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (!isZooming)
            {
                CameraZoom(mainCamera, zoomfactor, 0.2f);
                isZooming = true;
            }
        }
        else if (isZooming)
        {
            CameraZoom(mainCamera, 1f, 0.2f);
            isZooming = false;
        }
    }
    public void CameraZoom(Camera camera, float zoomfactor, float duration)
    {
        camera.DOFieldOfView(defaultFov / zoomfactor, duration);
    }
}
