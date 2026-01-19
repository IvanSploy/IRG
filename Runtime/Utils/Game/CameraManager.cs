using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public Camera Camera { private set; get; }
    
    [SerializeField] private Animator _animator;
    private static readonly int _cameraMode = Animator.StringToHash("Mode");
    
    public static CameraManager Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
        if(!_animator) _animator = GetComponentInChildren<Animator>();
        if(!Camera) Camera = GetComponent<Camera>();
    }
    

    public void SetCameraMode(int mode)
    {
        _animator?.SetInteger(_cameraMode, mode);
    }
}
