using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float _crashDelay = 1f;
    [SerializeField] float _levelDelay = 1f;
    [SerializeField] AudioClip _crashAudio;
    [SerializeField] AudioClip _finishAudio;

    [SerializeField] ParticleSystem _crashParticles;
    [SerializeField] ParticleSystem _finishParticles;

    AudioSource _audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false; 


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // Toggle Collision
        }
    }

    void OnCollisionEnter(Collision other)
    {

        if (isTransitioning || collisionDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Hey im a friend!");
                break;

            case "Finish":
                StartNextLevel();
                break;

            default:
                StartCrashSequence();
                break;
        }

    }


    void StartCrashSequence()
    {
        _crashParticles.Play();
        isTransitioning = true;
        _audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", _crashDelay);
        _audioSource.PlayOneShot(_crashAudio);


    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }



    void StartNextLevel()
    {
        _finishParticles.Play();
        isTransitioning = true;
        _audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", _levelDelay);
        _audioSource.PlayOneShot(_finishAudio);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}

