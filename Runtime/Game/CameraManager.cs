using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Transform> _cinematicCameras;
    
    private static readonly int CameraMode = Animator.StringToHash("Mode");
    public static bool IsFocusedInPlayer = true;

    private int _cinematicCameraIndex;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        if(!_animator) _animator = GetComponentInChildren<Animator>();
    }
    
    public void SetCinematicCamera(Transform cinematicParent)
    {
        _cinematicCameras[_cinematicCameraIndex].position = cinematicParent.position;
        _cinematicCameras[_cinematicCameraIndex].rotation = cinematicParent.rotation;
        _animator?.SetInteger(CameraMode, _cinematicCameraIndex + 1);
        _cinematicCameraIndex++;
        if (_cinematicCameraIndex > 1) _cinematicCameraIndex = 0;
        IsFocusedInPlayer = false;
    }

    public void SetPlayerCamera()
    {
        _animator?.SetInteger(CameraMode, 0);
        IsFocusedInPlayer = true;
    }
}
