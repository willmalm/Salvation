using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();

            return _rigidbody;
        }
    }

    private Animator _animator;

    public Animator Animator
    {
        get
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();

            return _animator;
        }
    }

    private NavMeshAgent _navigation;

    public NavMeshAgent Navigation
    {
        get
        {
            if (_navigation == null)
                _navigation = GetComponent<NavMeshAgent>();

            return _navigation;
        }
    }

    [Header("Patrol")]
    public PatrolRoute patrolRoute;
    public bool loopPatrol;

    [Header("Combat")]
    public GameObject currentTarget;

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnEnable()
    {
        EventManager.Subscribe("PauseGame", Disable);
        EventManager.Subscribe("ContinueGame", Enable);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe("PauseGame", Disable);
        EventManager.Unsubscribe("ContinueGame", Enable);
    }

    private void Disable(object argument)
    {
        Animator.speed = 0;
        Rigidbody.isKinematic = true;
        Navigation.enabled = false;
    }

    private void Enable(object argument)
    {
        Animator.speed = 1;
        Rigidbody.isKinematic = false;
        Navigation.enabled = true;
    }
}
