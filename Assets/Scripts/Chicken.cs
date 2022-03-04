using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _camTransform;
    [SerializeField] private Animator _followTarget;

    // [SerializeField] LayerMask _aimLayerMask;

    private bool _flipped;
    private bool _canFlip = true;
    private bool _isMoving;
    private CharacterController charac;
    private float _turnSmoothVelocity;
    private Vector3 _direction;

    void FixedUpdate() => Move();   
    
    private void Move() {

        // AimTowardMouse();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(_direction.magnitude >= 0.1f) {
            // if(!_isMoving) {
                _anim.Play("Run In Place");
                //  _isMoving = true;
            // }
            // else _anim.Play("Idle");
           
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
            float moveAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0f, moveAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            GetComponent<Rigidbody>().MovePosition(transform.position + moveDirection.normalized * Time.fixedDeltaTime * 3f);
        } 
        else {
            // if(_hasBox) 
                _anim.Play("Idle");
                // _isMoving = false;
            // else _anim.Play("Breathing Idle");
        }

        if(Input.GetKey("space"))
            _anim.Play("Eat");

        if(Input.GetKey("r") && _canFlip)
            FlipGravity();
    }

    private void FlipGravity() {
        StartCoroutine(FlipCoroutine());

        if(_flipped) {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            _followTarget.Play("FlipBack");

            _flipped = false;
        }
        else {
            Physics.gravity = new Vector3(0, 9.81f, 0);
            _followTarget.Play("FlipAnim");
            _flipped = true;
        }
    }
    
    private IEnumerator FlipCoroutine() {
        _canFlip = false;
        yield return new WaitForSeconds(2f);
        _canFlip = true;
    }

    // private void AimTowardMouse() {
    //     Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

    //     if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _aimLayerMask)) {
    //         var direction = hitInfo.point - transform.position;
    //         direction.y = 0f;
    //         direction.Normalize();
    //         transform.forward = direction;
    //     }
    // }

    private void OnTriggerEnter(Collider other) {
        // if(other.CompareTag("item"))
            // other.gameObject.GetComponent<Rigidbody>().AddForce(0, 10, 0);
    }
}
