//#define USE_CROUCH
#define MOUSE_SMOOTHING
#define USE_CROUCH
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 3.0f;
    public Animator visualanime;
    public GameObject slidingObj;

    private const float FRICTION = 5.0f;
    private const float DECC_SPEED = 3.5f;
    public float accSpeed = 10.0f;
    public float accSpeedAir = 1.5f;
    public float JUMP_VEL = 9f;
    public float JUMP_ACC = 1.42f;
    public float GRAVITY_ADD = 17.0f;
    public float CAMERA_OFFSET = 0.72f;
    public float PLAYER_HEIGHT = 1.8f;

#if MOUSE_SMOOTHING
    [RangeAttribute(0.0f, 1.0f)] public float mouseSmoothing = 0.1f;
    public Transform visual;
    private Quaternion targetRotation;
#endif
    private CharacterController _controller;
    private Vector3 _moveInput;
    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    private Vector3 _vectorDown;
    private Vector3 _surfaceNormal;
    private Vector3 _cameraOffset;
    private Quaternion _playerRotation;
    private RaycastHit _hitSurface;
    Vector3 _visualdir = Vector3.forward;
    private float _frameTime = 0.0f, _rotationInput, _lookInput, _lookY = 0.0f, _moveSpeed, _dot, _velAdd, _velMulti, _speed, _speedMulti;
#if USE_CROUCH
	private float crouch_value = 0;
	private float crouch_value_s = 0;
	private Vector3 center_offset;
#endif

    void Start()
    {
        _playerRotation = transform.rotation;
        _controller = GetComponent<CharacterController>();
        _controller.skinWidth = 0.03f;
        _controller.minMoveDistance = 0; // This is required for CharacterController.isGrounded to always work.
        _moveVector = new Vector3(0, -0.5f, 0);
        _vectorDown = new Vector3(0, -1.0f, 0);
        _surfaceNormal = new Vector3(0, 1.0f, 0);
        _moveInput = new Vector3(0, 0, 0);
        _cameraOffset = new Vector3(0, CAMERA_OFFSET, 0);
#if USE_CROUCH
		center_offset = new Vector3(0, 0, 0);
#endif
        if (!Application.isEditor)
        {
            Cursor.visible = false;
        }
    }

    void Update()
    {
        _frameTime = Time.deltaTime;


        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.z = Input.GetAxisRaw("Vertical");

        _rotationInput = Input.GetAxis("Mouse X") * rotationSpeed;
        _lookInput = Input.GetAxis("Mouse Y") * rotationSpeed * 0.9f; // Make vertical mouse look less sensitive.
        _lookY -= _lookInput;
        _lookY = Mathf.Clamp(_lookY, -90.0f, 90.0f);
        _playerRotation *= Quaternion.Euler(0, _rotationInput, 0);

        //if ((!Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl)))
        //{
        //    _moveDirection = _playerRotation * _moveInput;
        //}

        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            _visualdir = _moveDirection;
        }
        //visual.rotation = Quaternion.Slerp(visual.rotation, Quaternion.LookRotation(_visualdir, transform.up) *
        //    Quaternion.Euler(0, Input.GetKey(KeyCode.LeftControl) == true ? 90 : 0, 0), Time.deltaTime * 15);

        //visualanime.SetBool("Running", Input.GetButton("Horizontal") || Input.GetButton("Vertical"));
        //visualanime.SetBool("Sliding", Input.GetKey(KeyCode.LeftControl));
        //visualanime.SetBool("OnAir", !_controller.isGrounded);
        //slidingObj.SetActive(Input.GetKey(KeyCode.LeftControl));

        if (_controller.isGrounded)
        {
            if (Physics.Raycast(transform.position, _vectorDown, out _hitSurface, _controller.height / 1.5f))
            {
                _surfaceNormal = _hitSurface.normal;
                _moveDirection = ProjectOnPlane(_moveDirection, _surfaceNormal); // Stick to the ground on slopes.
            }
            _moveDirection.Normalize();
#if USE_CROUCH
            //_moveSpeed = _moveDirection.magnitude * (moveSpeed - crouch_value_s * 3.0f);
            _moveSpeed = _moveDirection.magnitude * moveSpeed * (Input.GetKey(KeyCode.LeftControl) == true ? 3 : 1);
            //print(_moveSpeed);
#else
            _moveSpeed = _moveDirection.magnitude * moveSpeed * (Input.GetKey(KeyCode.LeftControl) == true ? 3 : 1);
#endif
            _dot = _moveVector.x * _moveDirection.x + _moveVector.y * _moveDirection.y + _moveVector.z * _moveDirection.z;
            _speed = (float)System.Math.Sqrt(_moveVector.x * _moveVector.x + _moveVector.z * _moveVector.z);
            _speedMulti = _speed - (_speed < DECC_SPEED ? DECC_SPEED : _speed) * FRICTION * _frameTime;
            if (_speedMulti < 0) _speedMulti = 0;
            if (_speed > 0) _speedMulti /= _speed;
            _moveVector *= _speedMulti;
            _velAdd = _moveSpeed - _dot;
            _velMulti = accSpeed * _frameTime * _moveSpeed;
            if (_velMulti > _velAdd) _velMulti = _velAdd;
            _moveVector += _moveDirection * _velMulti;
            if (_moveVector.y > -0.5f) _moveVector.y = -0.5f; // Make sure there is always a little gravity to keep the character on the ground.
            if (Input.GetButton("Jump"))
            {
                if (_surfaceNormal.y > 0.5f) // Do not jump on high slopes.
                {
                    _moveVector *= JUMP_ACC;
                    _moveVector.y = JUMP_VEL;
                }
            }
        }
        else // In Air
        {
            _moveDirection.Normalize();
            _moveSpeed = _moveDirection.magnitude * moveSpeed;
            _dot = _moveVector.x * _moveDirection.x + _moveVector.y * _moveDirection.y + _moveVector.z * _moveDirection.z;
            _velAdd = _moveSpeed - _dot;
            _velMulti = accSpeedAir * _frameTime * _moveSpeed;
            if (_velMulti > _velAdd) _velMulti = _velAdd;
            if (_velMulti > 0) _moveVector += _moveDirection * _velMulti;
            _moveVector.y -= GRAVITY_ADD * _frameTime;
        }
#if USE_CROUCH
		if (Input.GetKey(KeyCode.LeftControl))
		{
			if (crouch_value < 1.0f)
			{
				crouch_value += _frameTime * 5.7f;
				crouch_value_s = Mathf.Clamp01(crouch_value);
				center_offset.y = crouch_value_s * -0.5f;
				_controller.height = PLAYER_HEIGHT - crouch_value_s;
				_controller.center = center_offset;
				_cameraOffset.y = CAMERA_OFFSET - crouch_value_s * 0.9f;
			}
		}
		else
		{
			if (crouch_value > 0)
			{
				RaycastHit hit_up;
				if (!Physics.SphereCast(playerCamera.transform.position + _vectorDown * 0.25f, 0.3f, new Vector3(0, 1.0f, 0), out hit_up, 0.3f)) // Check if there is a space for player to raise
				{
					crouch_value -= _frameTime * 5.0f;
					crouch_value_s = Mathf.Clamp01(crouch_value);
					center_offset.y = crouch_value_s * -0.5f;
					_controller.height = PLAYER_HEIGHT - crouch_value_s;
					_controller.center = center_offset;
					_cameraOffset.y = CAMERA_OFFSET - crouch_value_s * 0.9f;
				}
			}
		}
#endif
        _controller.Move(_moveVector * _frameTime);
#if MOUSE_SMOOTHING
        targetRotation = _playerRotation * Quaternion.Euler(_lookY, 0, 0);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetRotation, _frameTime * (1.0f - mouseSmoothing) * 50.0f);
#else
		playerCamera.transform.rotation = player_rotation * Quaternion.Euler(look_y, 0, 0);
#endif
        playerCamera.transform.position = transform.position + _cameraOffset;
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    Vector3 ProjectOnPlane(Vector3 vector, Vector3 normal)
    {
        return vector - normal * (vector.x * normal.x + vector.y * normal.y + vector.z * normal.z);
    }
}