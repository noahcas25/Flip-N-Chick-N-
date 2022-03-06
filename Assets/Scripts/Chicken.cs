using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _camTransform;
    [SerializeField] private Transform _followTarget;

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private bool _flipped;
    private bool _canFlip = true;
    private bool _isMoving;
    private float _turnSmoothVelocity;
    private Vector3 _direction;

    private void Awake() {
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
    }

    void FixedUpdate() => Move();   
    
    private void Move() {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(_direction.magnitude >= 0.1f) {
            _anim.Play("Run In Place");
           
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
            float moveAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0f, moveAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            GetComponent<Rigidbody>().MovePosition(transform.position + moveDirection.normalized * Time.fixedDeltaTime * 3f);
        } 
        else
             _anim.Play("Idle");

        if(Input.GetKey("r") && _canFlip)
            FlipGravity();
    }

    private void FlipGravity() {
        StartCoroutine(FlipCoroutine());

        if(_flipped) {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            StartCoroutine(FlipDelay());     
            _flipped = false;
        }
        else {
            Physics.gravity = new Vector3(0, 9.81f, 0);
             StartCoroutine(FlipBackDelay());
            _flipped = true;
        }
    }

     private IEnumerator FlipDelay() {
        yield return new WaitForSeconds(0.3f);
        _followTarget.Rotate(0, 0, 180); 
     }

      private IEnumerator FlipBackDelay() {
        yield return new WaitForSeconds(0.3f);
        _followTarget.Rotate(0, 0, -180); 
     }
        
    private IEnumerator FlipCoroutine() {
        _canFlip = false;
        _anim.enabled = false;
        yield return new WaitForSeconds(.75f);
        _canFlip = true;
        _anim.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Obstacle")) {
            Physics.gravity = new Vector3(0, -9.81f, 0);
            if(_flipped) {
                _followTarget.Rotate(0, 0, -180);
                _flipped = false;
            }

            transform.rotation = respawnRotation;
            transform.position = respawnPosition;
        }
        // if(other.CompareTag("Egg"))
            // Increment Limit
        
    }
}
