using UnityEngine;
/*
    This class replaces the default system for enemy squashing because it's inaccurate
    - By John Wang
*/

public class EnemySquasher : MonoBehaviour
{

    public PlayerCharacter parentCharacter;

    public void Start()
    {
        parentCharacter = this.GetComponentInParent<PlayerCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == GameplayConstants.TAG_Enemy)
        {
                Enemy enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy != null && enemy.transform.position.y < this.GetComponent<CircleCollider2D>().transform.position.y)
                {
                    parentCharacter.enemyScore += enemy.Squash();
                    parentCharacter.hud.UpdateEnemies(parentCharacter.enemyScore);
                }
            }
        }
    }

