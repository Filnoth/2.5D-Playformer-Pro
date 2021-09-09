using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _anim;
    [SerializeField]
    private float _speed = 12.0f;
    private Vector3 _direction;
    [SerializeField]
    private float _jumpHeight = 10.0f;
    [SerializeField]
    private int _grav = 1;
    private bool _jumping = false;
    private bool _onLedge = false;
    private LedgeDetector _activeLedge;

    private bool _rolling = false;
    [SerializeField]
    private float _rollSpeed = 5f;
    [SerializeField]
    private Vector3 _colliderCenterSm;
    private Vector3 _colliderCenter;
    [SerializeField]
    private float _colliderHeightSm;
    private float _colliderHeight;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();

        _colliderCenter = _controller.center;
        _colliderHeight = _controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        CalcMovement();

        if (_onLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("Climb");
            }
        }
    }

    void CalcMovement()
    {
        if (_controller.isGrounded == true)
        {
            if (_jumping == true)
            {
                _jumping = false;
                _anim.SetBool("Jump", _jumping);
            }

            float h = Input.GetAxisRaw("Horizontal");
            _direction = new Vector3(0, 0, h) * _speed;
            _anim.SetFloat("Speed", Mathf.Abs(h));

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.Space) && _rolling == false)
            {
                _direction.y += _jumpHeight;
                _jumping = true;
                _anim.SetBool("Jump", _jumping);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && _rolling == false)
            {
                _anim.SetTrigger("Roll");
                _rolling = true;
                _controller.center = _colliderCenterSm;
                _controller.height = _colliderHeightSm;
            }

            if (_rolling == true)
            {
                if (Input.GetAxis("Horizontal") < 0)
                {
                    _direction.z -= _rollSpeed;
                }

                else if  (Input.GetAxis("Horizontal") > 0)
                {
                    _direction.z += _rollSpeed;
                }
            }
        }

        _direction.y -= _grav * Time.deltaTime;

        _controller.Move(_direction * Time.deltaTime);
    }

    public void LedgeGrab(Vector3 handpos, LedgeDetector currentLedge)
    {
        _controller.enabled = false;
        _anim.SetBool("LedgeGrab", true);
        _anim.SetBool("Jump", false);
        _anim.SetFloat("Speed", 0.0f);
        _onLedge = true;
        transform.position = handpos;
        _activeLedge = currentLedge;
    }

    public void ClimbComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _anim.SetBool("LedgeGrab", false);
        _controller.enabled = true;
    }

    public void RollEnds()
    {
        _rolling = false;
        _controller.center = _colliderCenter;
        _controller.height = _colliderHeight;
    }
}
