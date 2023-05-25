using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    public float speed = 30f;
    public float slowDownRate = 0.01f;
    public float detectingDistance = 0.1f;
    public float destroyDelay = 5f;

    private Rigidbody _rb;
    private bool _stopped;
    
    private Action _onComplete;
    private Vector3 _targetPosition;

    public void Setup(Transform target, Action onHit, Action onComplete)
    {
        _onComplete = onComplete;

        Vector3 position = transform.position;
        _targetPosition = position + (target.position - position).normalized * 16f;

        LTDescr runAnim = LeanTween.move(gameObject, _targetPosition, .45f).setEaseInCubic();

        runAnim.setOnComplete(() =>
        {
            onHit?.Invoke();
            
            if (GetComponent<Rigidbody>() != null)
            {
                _rb = GetComponent<Rigidbody>();
                StartCoroutine(SlowDown());
            }
            else
            {
                Debug.Log("Ground Slash got no RigidBody");
            }
        });
    }

    private void FixedUpdate()
    {
        if (!_stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, detectingDistance))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
        }
    }

    private IEnumerator SlowDown()
    {
        while (destroyDelay > 0)
        {
            _rb.velocity = Vector3.Lerp(Vector3.zero, _rb.velocity, destroyDelay);
            destroyDelay -= slowDownRate;
            yield return new WaitForSeconds(.1f);
        }
        
        _stopped = true;
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        _onComplete?.Invoke();
    }
}
