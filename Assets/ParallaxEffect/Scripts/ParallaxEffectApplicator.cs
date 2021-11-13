using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ParallaxEffectApplicator : MonoBehaviour
{
    public const string MotionSourceName = nameof(motionSource);
    public const string TargetCameraName = nameof(targetCamera);
    public const string SelfSpeedName = nameof(selfSpeed);
    public const string SelfDirectionName = nameof(selfDirection);
    public const string ForceRootLookAtCameraName = nameof(forceRootLookAtCamera);
    public const string LayersRootName = nameof(layersRoot);
    public const string ParallaxLayersName = nameof(parallaxLayers);
    
    public MotionSource SelectedMotionSource => motionSource;
    
    [SerializeField] private MotionSource motionSource = MotionSource.Camera;
    [SerializeField] private Camera targetCamera = null;
    [SerializeField, Range(0f, 1f)] private float selfSpeed = 1f;
    [SerializeField] private MovementDirection selfDirection = MovementDirection.Left;

    [Space]
    [SerializeField] private bool forceRootLookAtCamera = true;
    [SerializeField] private RectTransform layersRoot = null;

    [Space]
    [SerializeField] private ParallaxLayer[] parallaxLayers = null;

    private readonly Dictionary<MovementDirection, Vector3> movementDirectionMap = new Dictionary<MovementDirection, Vector3>
    {
        { MovementDirection.Left, Vector3.left },
        { MovementDirection.Right, Vector3.right }
    };
    
    private float initialCameraVerticalPosition = 0f;
    private float lastCameraHorizontalPosition = 0f;
    private Quaternion lastCameraRotation = Quaternion.identity;

    public enum MotionSource
    {
        Self,
        Camera
    }

    private enum MovementDirection
    {
        Left,
        Right
    }

    private void Start()
    {
        Transform cameraTransform = targetCamera.transform;
        
        initialCameraVerticalPosition = cameraTransform.position.y;
        lastCameraHorizontalPosition = cameraTransform.position.x;
        lastCameraRotation = cameraTransform.rotation;
        
        AddSideLayersTo(parallaxLayers);
    }

    private void Update()
    {
        if (forceRootLookAtCamera)
        {
            ForceLayersRootLookAtCamera();
        }
        
        WrapLayersInBounds(parallaxLayers);
        ApplyParallaxTo(parallaxLayers);
    }

    private void ForceLayersRootLookAtCamera()
    {
        Transform cameraTransform = targetCamera.transform;

        if (cameraTransform.rotation == lastCameraRotation)
        {
            return;
        }

        layersRoot.position = cameraTransform.position + cameraTransform.forward + cameraTransform.TransformDirection(-initialCameraVerticalPosition * Vector3.up);
        layersRoot.rotation = Quaternion.LookRotation(cameraTransform.forward, cameraTransform.up);
        
        lastCameraRotation = cameraTransform.rotation;
    }

    private void ApplyParallaxTo(ParallaxLayer[] layers)
    {
        float currentCameraPosition = targetCamera.transform.position.x;
        
        Vector3 deltaDistance = motionSource == MotionSource.Camera
            ? Vector3.right * (currentCameraPosition - lastCameraHorizontalPosition)
            : selfSpeed * movementDirectionMap[selfDirection] * Time.deltaTime;
        lastCameraHorizontalPosition = currentCameraPosition;
        
        foreach (var layer in layers)
        {
            layer.TranslateWithParallax(deltaDistance, motionSource);
        }
    }

    private void WrapLayersInBounds(ParallaxLayer[] layers)
    {
        foreach (var layer in layers)
        {
            float signDistance = layer.RectTransform.InverseTransformPoint(targetCamera.transform.position).x;
            
            Vector3 direction = Vector3.right * Mathf.Sign(signDistance);
            float distance = Mathf.Abs(signDistance);
            float layerRelativeWidth = layer.RectTransform.rect.width;
            
            if (distance > layerRelativeWidth)
            {
                layer.Translate(direction * layerRelativeWidth);
            }
        }
    }

    private void AddSideLayersTo(ParallaxLayer[] layers)
    {
        foreach (var layer in layers)
        {
            RectTransform layerRectTransform = layer.RectTransform;
            
            string layerName = layerRectTransform.gameObject.name;
            Vector3 horizontalOffset = Vector3.right * layerRectTransform.rect.width;

            Instantiate(layerRectTransform, layerRectTransform.position - horizontalOffset, Quaternion.identity, layerRectTransform)
                .gameObject.name = $"{layerName} - Left";
            Instantiate(layerRectTransform, layerRectTransform.position + horizontalOffset, Quaternion.identity, layerRectTransform)
                .gameObject.name = $"{layerName} - Right";
        }
    }
}
