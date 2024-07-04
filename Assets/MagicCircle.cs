using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicCircle : MonoBehaviour
{
    public float effectDuration = 10f;
    public float reactivationDelay = 30f;
    private bool isActive = false;
    private bool isInCooldown = false; // Adicionado para verificar se está em cooldown
    private static Queue<MagicCircle> activeCircles = new Queue<MagicCircle>();
    private static int maxActiveCircles = 3;

    public Material activeMaterial; // Material para o estado ativo
    public Material inactiveMaterial; // Material para o estado inativo
    private Renderer circleRenderer;
    public Text cooldownText; // Referência para o texto de cooldown na UI

    void Start()
    {
        circleRenderer = GetComponent<Renderer>(); // Obtém o Renderer do objeto
        SetInactiveVisuals(); // Define a aparência inicial como inativa
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isActive)
            {
                DeactivateCircle();
            }
            else if (!isInCooldown)
            {
                ActivateCircle();
            }
        }
    }

    void ActivateCircle()
    {
        if (activeCircles.Count >= maxActiveCircles)
        {
            activeCircles.Dequeue().DeactivateCircle();
        }
        isActive = true;
        activeCircles.Enqueue(this);
        SetActiveVisuals();
    }

    void DeactivateCircle()
    {
        isActive = false;
        isInCooldown = true; // Define que o círculo está em cooldown
        SetInactiveVisuals();
        StartCoroutine(ReactivationCooldown());
    }

    void SetActiveVisuals()
    {
        if (circleRenderer != null && activeMaterial != null)
        {
            circleRenderer.material = activeMaterial; // Aplica o material ativo
        }
    }

    void SetInactiveVisuals()
    {
        if (circleRenderer != null && inactiveMaterial != null)
        {
            circleRenderer.material = inactiveMaterial; // Aplica o material inativo
        }
    }

    IEnumerator ReactivationCooldown()
    {
        float cooldown = reactivationDelay;
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            UpdateCooldownText(cooldown);
            yield return null;
        }
        UpdateCooldownText(0);
        isInCooldown = false; // Define que o círculo não está mais em cooldown
    }

    void UpdateCooldownText(float cooldown)
    {
        if (cooldownText != null)
        {
            cooldownText.text = cooldown > 0 ? $"Cooldown: {cooldown:F1}s" : "";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("EnemyFather") || other.CompareTag("EnemyMother") || other.CompareTag("ImaginaryFriend"))
        {
            if (other.CompareTag("EnemyFather") || other.CompareTag("EnemyMother"))
            {
                StartCoroutine(ParalyzeEnemy(other.GetComponent<Rigidbody>(), effectDuration));
            }
            else if (other.CompareTag("ImaginaryFriend"))
            {
                StartCoroutine(SlowDownEnemy(other.GetComponent<ImaginaryFriend>(), effectDuration));
            }
            DeactivateCircle();
        }
    }

    IEnumerator ParalyzeEnemy(Rigidbody enemyRb, float duration)
    {
        Vector3 originalVelocity = enemyRb.velocity;
        enemyRb.velocity = Vector3.zero;
        enemyRb.isKinematic = true;
        yield return new WaitForSeconds(duration);
        enemyRb.isKinematic = false;
        enemyRb.velocity = originalVelocity;
    }

    IEnumerator SlowDownEnemy(ImaginaryFriend enemyScript, float duration)
    {
        float originalSpeed = enemyScript.chaseSpeed;
        enemyScript.chaseSpeed /= 2;
        yield return new WaitForSeconds(duration);
        enemyScript.chaseSpeed = originalSpeed;
    }
}
