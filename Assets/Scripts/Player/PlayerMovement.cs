using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed;
    [Space]
    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraJoint;
    [SerializeField] private float rotationYMax;
    [SerializeField] private float rotationYMin;
    [Space]
    [Header("Dash Settings")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float _speedDash;

    private PlayerInputMap _inputSettings;
    private CharacterController _characterController;
    private Transform _character;
    private Vector2 _move;
    private Vector2 _look;
    private bool _canMove;
    private void Awake()
    {
        _inputSettings = new PlayerInputMap();
        _inputSettings.Player.Enable();
        _canMove = true;
        _character = transform.Find("Character");
        _characterController = GetComponentInChildren<CharacterController>();
        _inputSettings.Player.Move.canceled += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Look.performed += Looking;
        _inputSettings.Player.Dash.performed += Dash;
    }

    private void Update()
    {
        if (_canMove)
        {
            Movement();
        }
        else
        {
            DashMovement();
        }
    }

    private void Movement()
    {
        if (!_move.Equals(Vector2.zero))
        {
            float targetRotation = GetTargetRotation();
            Vector3 targetDirection = GetTargetDirection(targetRotation);
            _characterController.Move((targetDirection.normalized *_speed + 
                                       new Vector3(_move.x, 0, _move.y))*Time.deltaTime);
            _character.rotation = Quaternion.Euler(0,targetRotation,0);
        }
    }

    private void Looking(InputAction.CallbackContext ctx)
    {
        _look = ctx.ReadValue<Vector2>();
        Vector3 rotationCamera = _cameraJoint.rotation.eulerAngles;
        float X = rotationCamera.x + _look.y;
        float Y = rotationCamera.y + _look.x;
        if (Y <= rotationYMax && Y >= rotationYMin)
        {
            _cameraJoint.SetPositionAndRotation(_cameraJoint.position, Quaternion.Euler(new Vector3(X, Y)));
        }
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        _canMove = false;
        StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashDistance/_speedDash);
        _canMove = true;
    }

    private Vector3 GetTargetDirection( float targetRotation)
    {
        return Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
    }

    private float GetTargetRotation()
    {
        Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;
        return Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
               _cameraJoint.transform.eulerAngles.y;
    }

    private void DashMovement()
    {
        Vector3 targetDirection = GetTargetDirection(GetTargetRotation());
        _characterController.Move(targetDirection.normalized * _speedDash * Time.deltaTime);
    }
}
