using System.Collections;
using UnityEngine;

public class MagicCircleController : MonoBehaviour
{
    public float activationTime = 5f; // Tempo de ativação em segundos
    public float paralyzeDuration = 3f; // Duração da paralisação dos inimigos em segundos

    private bool activated = false;
    private Coroutine activationCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quando o jogador entrar no círculo mágico, permitir a ativação ao pressionar E
            if (Input.GetKeyDown(KeyCode.E) && !activated)
            {
                activated = true;
                activationCoroutine = StartCoroutine(ActivateMagicCircle());
            }
        }
    }

    IEnumerator ActivateMagicCircle()
    {
        // Ativar círculo mágico
        Debug.Log("Magic Circle Activated!");

        // Paralisar Pai e Mãe que entrarem no círculo mágico
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<Collider>().bounds.extents.magnitude);
        foreach (Collider collider in colliders)
        {
            PaiController pai = collider.GetComponent<PaiController>();
            MaeController mae = collider.GetComponent<MaeController>();

            if (pai != null)
            {
                // Paralisa o Pai por um período de tempo
                pai.Paralyze(paralyzeDuration);
            }
            else if (mae != null)
            {
                // Paralisa a Mãe por um período de tempo
                mae.Paralyze(paralyzeDuration);
            }
        }

        // Aguardar tempo de ativação
        yield return new WaitForSeconds(activationTime);

        // Desativar círculo mágico após o tempo de ativação
        DeactivateMagicCircle();
    }

    void DeactivateMagicCircle()
    {
        activated = false;
        StopCoroutine(activationCoroutine);
        activationCoroutine = null;

        // Desativar efeito do círculo mágico
        Debug.Log("Magic Circle Deactivated!");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quando o jogador sair do círculo mágico, cancelar a ativação
            if (activated)
            {
                DeactivateMagicCircle();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, GetComponent<Collider>().bounds.extents.magnitude);
    }
}
