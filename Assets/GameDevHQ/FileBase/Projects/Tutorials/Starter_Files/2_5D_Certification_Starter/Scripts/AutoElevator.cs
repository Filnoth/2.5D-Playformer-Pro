using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoElevator : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    private bool _switch = false;
    private float _speed = 10.0f;
    private bool _reachedDest = false;
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        if (_switch == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        }

        else if (_switch == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }

        if (transform.position == _targetA.position && _reachedDest == false)
        {
            StartCoroutine(ElevatorMovement());
            _reachedDest = true;
        }
        else if (transform.position == _targetB.position && _reachedDest == true)
        {
            StartCoroutine(ElevatorMovement());
            _reachedDest = false;
        }

    }

    private IEnumerator ElevatorMovement()
    {
        yield return new WaitForSeconds(5.0f);
        _switch = !_switch;
    }
}
