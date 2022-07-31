using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private MovementCharactersSettings _dataMovement;
    [Space]
    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraJoint;
    private PlayerInputMap _inputSettings;
    private CharacterController _characterController;
    private Transform _character;
    private Vector2 _move;
    private Vector2 _look;
    private bool _canMove;
    private Collider _collider;
    private void Awake()
    {
        _inputSettings = new PlayerInputMap();
        _inputSettings.Player.Enable();
        _canMove = true;
        _character = transform.Find("Character");
        _collider = _character.GetComponent<Collider>();
        _collider.isTrigger = _canMove;
        _characterController = GetComponentInChildren<CharacterController>();
        _inputSettings.Player.Move.canceled += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _inputSettings.Player.Look.performed += Looking;
        _inputSettings.Player.Dash.performed += Dash;
    }

    private void Update()
    {
        if (hasAuthority)
        {
            _collider.isTrigger = _canMove;
            if (_canMove)
            {
                Movement();
            }
            else
            {
                DashMovement();
            }
        }
    }

    private void Movement()
    {
        if (!_move.Equals(Vector2.zero))
        {
            Vector3 targetDirection = GetTargetDirection(GetTargetRotation());
            _characterController.Move((targetDirection.normalized *_dataMovement.Speed + 
                                       new Vector3(_move.x, 0, _move.y))*Time.deltaTime);
        }
    }

    private void Looking(InputAction.CallbackContext ctx)
    {
        _look = ctx.ReadValue<Vector2>();
        Vector3 rotationCamera = _cameraJoint.localRotation.eulerAngles;
        float X = rotationCamera.x + _look.y;
        float Y = rotationCamera.y + _look.x;
        _cameraJoint.SetPositionAndRotation(_cameraJoint.position, Quaternion.Euler(new Vector3(X, Y)));
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        _canMove = false;
        StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(_dataMovement.DistanceDash/_dataMovement.DashSpeed);
        _canMove = true;
    }

    private Vector3 GetTargetDirection( float targetRotation)
    {
        return Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
    }

    private float GetTargetRotation()
    {
        Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;
        float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
               _cameraJoint.transform.eulerAngles.y;
        _character.rotation = Quaternion.Euler(0,targetRotation,0);
        return targetRotation;
    }

    private void DashMovement()
    {
        Vector3 targetDirection = GetTargetDirection(GetTargetRotation());
        _characterController.Move(targetDirection.normalized * _dataMovement.DashSpeed * Time.deltaTime);
    }
}
