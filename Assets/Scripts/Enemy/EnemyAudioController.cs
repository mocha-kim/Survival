using System.Collections;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField]
    private float maxTime = 6.0f;
    [SerializeField]
    private float minTime = 1.0f;

    [SerializeField]
    private AudioClip[] growlSound;
    private AudioSource audioSource;

    [SerializeField]
    private float soundDistance = 20.0f;
    private bool isPlaying = false;

    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!enemyController.IsDead)
        {
            if (enemyController.GetDistance <= soundDistance)
            {
                if (!isPlaying)
                {
                    StartCoroutine(PlayRandomTimeSound());
                }
            }
        }
    }

    IEnumerator PlayRandomTimeSound()
    {
        float coolTime;
        WaitForSeconds wait = new(0.25f);

        isPlaying = true;
        coolTime = Random.Range(minTime, maxTime);
        while (coolTime > 0)
        {
            if (enemyController.GetDistance > soundDistance)
                yield break;
            coolTime -= Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1.0f;
        audioSource.PlayOneShot(growlSound[Random.Range(0, growlSound.Length)]);
        yield return wait;
        isPlaying = false;
        yield break;
    }
}
