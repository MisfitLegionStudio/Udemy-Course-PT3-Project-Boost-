using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip _mainEngine;
    [SerializeField] AudioClip _sideEngine;

    [SerializeField] ParticleSystem _leftfrontBooster;
    [SerializeField] ParticleSystem _rightfrontBooster;
    [SerializeField] ParticleSystem _mainBooster;

    Rigidbody _rb;
    AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _rb =  GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }

        else
        {
            StopThrusting();
        }

    }
    void StartThrusting()
    {
        _rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_mainEngine);
        }

        if (!_mainBooster.isPlaying)
        {
            _mainBooster.Play();
        }
    }

    void StopThrusting()
    {
        _audioSource.Stop();
        _mainBooster.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            StartLeftThrust();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            StartRightThrust();
        }

        else
        {
            StopSideThrusts();
        }
    }
    void StartLeftThrust()
    {
        ApplyRotation(rotationThrust);

        if (!_rightfrontBooster.isPlaying)
        {
            _rightfrontBooster.Play();
        }

    }
    void StartRightThrust()
    {
        ApplyRotation(-rotationThrust);

        if (!_leftfrontBooster.isPlaying)
        {
            _leftfrontBooster.Play();
        }
        
    }
    void StopSideThrusts()
    {
        _rightfrontBooster.Stop();
        _leftfrontBooster.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        _rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        _rb.freezeRotation = false; // unfreezing rotation so physics system can take over
    }

}
