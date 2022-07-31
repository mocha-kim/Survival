using System.Collections;
using UnityEngine;

/*
 * PlayerAttackCollision is player's attack collision
 * This can give an attack signal to the enemy
 */

public class EnemyAttackCollision : MonoBehaviour
{
    // Enemy Database
    [SerializeField]
    private EnemyDatabase database;
    [SerializeField]
    private int enemyID;

    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnHit(database.data[enemyID].damage);
        }
    }

    private IEnumerator AutoDisable()
    {
        // after 0.1s -> disappear object
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

    public void SetEnemyInfo(int ID)
    {
        enemyID = ID;
    }
}
