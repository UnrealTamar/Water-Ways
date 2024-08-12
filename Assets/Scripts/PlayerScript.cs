using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class FootStep
{
    [Header("Foot Step")]
    public Transform LeftfootAnchor;
    public Transform RightfootAnchor;
    public GameObject footStepMark;
    public AudioSource footStepAudioSource;
    public AudioClip footStepAudio;


}
public class PlayerScript : MonoBehaviour
{
    const string RUN = "Run";
    const string DAMAGE = "Damaged";

    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;

    bool isOnAcidSteam = false;

    public FootStep footStep;

    [Header("Quest Functions")]
    public Quest quest;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        Time.timeScale = 1f;
        
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        // Check for left mouse button or touch input
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 inputPosition = Input.GetMouseButtonDown(0) ? Input.mousePosition : Input.GetTouch(0).position;
            ClickToMove(inputPosition);
        }

        if (quest != null && quest.isActive == true)
        {
            UpdateUI();
        }

        FaceTarget();
        SetAnimation();

    }

    void ClickToMove(Vector2 inputPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(inputPosition);

        if (Physics.Raycast(ray, out hit, 100, clickableLayers))
        {
            agent.destination = hit.point;

            if (clickEffect != null)
            {
                Instantiate(clickEffect, hit.point + new Vector3(0, .1f, 0), clickEffect.transform.rotation);
            }
        }
    }

    private void SetAnimation()
    {
        if (isOnAcidSteam)
        {
            // Play DAMAGE animation
            animator.SetBool(RUN, false);
            animator.SetBool(DAMAGE, true);
        }
        else
        {
            // Play RUN animation
            if (agent.velocity == Vector3.zero)
            {
                animator.SetBool(RUN, false);
            }
            else
            {
                animator.Play("Run");
                animator.SetBool(RUN, true);
            }

            // Reset DAMAGE animation
            animator.SetBool(DAMAGE, false);
        }
    }

    private void FaceTarget()
    {
        if (agent.velocity != Vector3.zero)
        {
            Vector3 dir = (agent.steeringTarget - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mission"))
        {
            other.GetComponent<NpcController>().OnTriggerEnter?.Invoke();
            other.GetComponent<NpcController>().LookAtTarget(transform);

        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mission"))
        {
            other.GetComponent<NpcController>().OnTriggerExit?.Invoke();

        }
        if (other.CompareTag("AcidSteam"))
        {
            Debug.Log("Exited " + other.name);
            isOnAcidSteam = false;
        }
    }


    public void UpdateUI()
    {
        scoreText.text = $"{quest.goal.currentAmount}/{quest.goal.requiredAmount}";
    }


    //Animator Event System
    void RightFootStep()
    {
        footStep.footStepAudioSource.PlayOneShot(footStep.footStepAudio);
        Instantiate(footStep.footStepMark, footStep.RightfootAnchor.position, footStep.RightfootAnchor.rotation);
    }
    void LeftFootStep()
    {
        footStep.footStepAudioSource.PlayOneShot(footStep.footStepAudio);
        Instantiate(footStep.footStepMark, footStep.LeftfootAnchor.position, footStep.LeftfootAnchor.rotation);
    }




}





