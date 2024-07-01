using System.Collections;
using UnityEngine;

public class MagicCircleController : MonoBehaviour
{
    public float activationTime = 5f; // Tempo de ativa��o em segundos
    public float paralyzeDuration = 3f; // Dura��o da paralisa��o dos inimigos em segundos

    private bool activated = false;
    private Coroutine activationCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quando o jogador entrar no c�rculo m�gico, permitir a ativa��o ao pressionar E
            if (Input.GetKeyDown(KeyCode.E) && !activated)
            {
                activated = true;
                activationCoroutine = StartCoroutine(ActivateMagicCircle());
            }
        }
    }

    IEnumerator ActivateMagicCircle()
    {
        // Ativar c�rculo m�gico
        Debug.Log("Magic Circle Activated!");

        // Paralisar Pai e M�e que entrarem no c�rculo m�gico
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<Collider>().bounds.extents.magnitude);
        foreach (Collider collider in colliders)
        {
            PaiController pai = collider.GetComponent<PaiController>();
            MaeController mae = collider.GetComponent<MaeController>();

            if (pai != null)
            {
                // Paralisa o Pai por um per�odo de tempo
                pai.Paralyze(paralyzeDuration);
            }
            else if (mae != null)
            {
                // Paralisa a M�e por um per�odo de tempo
                mae.Paralyze(paralyzeDuration);
            }
        }

        // Aguardar tempo de ativa��o
        yield return new WaitForSeconds(activationTime);

        // Desativar c�rculo m�gico ap�s o tempo de ativa��o
        DeactivateMagicCircle();
    }

    void DeactivateMagicCircle()
    {
        activated = false;
        StopCoroutine(activationCoroutine);
        activationCoroutine = null;

        // Desativar efeito do c�rculo m�gico
        Debug.Log("Magic Circle Deactivated!");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quando o jogador sair do c�rculo m�gico, cancelar a ativa��o
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
