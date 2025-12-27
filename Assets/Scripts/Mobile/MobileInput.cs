using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { get; private set; }

    [HideInInspector] public float Horizontal;
    [HideInInspector] public float Vertical;
    [HideInInspector] public float LookX;
    [HideInInspector] public float LookY;
    [HideInInspector] public bool IsTouchingField;
    [HideInInspector] public bool Sprint;
    
    private bool _jumpPressed;
    private bool _jumpConsumed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PressJump()
    {
        _jumpPressed = true;
        _jumpConsumed = false;
    }

    public bool GetJumpDown()
    {
        if (_jumpPressed && !_jumpConsumed)
        {
            _jumpConsumed = true;
            // Reset immediately to prevent multi-jump until pressed again
            _jumpPressed = false; 
            return true;
        }
        return false;
    }
}
