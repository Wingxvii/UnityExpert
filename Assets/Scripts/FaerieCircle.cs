using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FaerieCircle : MonoBehaviour
{
    private float faerieSpeed;
    public int grenadeStock = 1;
    public float cullRadius = 5f;

    private float radius = 1f;
    private ParticleSystem faerieParticles;
    private ParticleSystem circleParticles;
    private int remainingGrenades;
    private Transform faerie;
    private Light faerieGlow;
    private Vector3 moveVector = Vector3.zero;
    public float moveTimer = 0f;

	void Start ()
    {
        PopulateParticleSystemCache();

        faerieGlow = this.GetComponentInChildren<Light>();

        remainingGrenades = grenadeStock;
        faerieSpeed = 1f;
    }

    private void PopulateParticleSystemCache()
    {
        ParticleSystem[] pSystems = this.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < pSystems.Length; i++)
        {
            ParticleSystem.MainModule pMain = pSystems[i].main;
            if (pSystems[i].shape.shapeType == ParticleSystemShapeType.Circle)
            {
                circleParticles = pSystems[i];
                radius = pSystems[i].shape.radius;
            }
            else
            {
                faerie = pSystems[i].gameObject.transform;
                faerieParticles = pSystems[i];
            }
        }
    }

    void OnStateChanged(CullingGroupEvent cullEvent)
    {
        if (cullEvent.isVisible)
        {
            faerieParticles.Play(true);
        }
        else
        {
            faerieParticles.Pause();
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Enemy" && coll.attachedRigidbody.isKinematic)
        {
            MakeAngry();
        }
    }

    void Update()
    {
        if (moveTimer > 0f)
        {
            moveTimer -= Time.deltaTime;
            MoveFaerie(Time.deltaTime * moveVector);
        }
        else
        {
            moveTimer = faerieSpeed;
            moveVector = GetRandomVector();
        }
    }

    private void ActivateFaerie(bool activate)
    {
        GameObject faerieGO = faerie.gameObject;
        if (faerieGO.activeInHierarchy != activate)
        {
            faerieGO.SetActive(activate);
        }
    }

    public void SetMood(bool angry)
    {
        if (!angry)
            SpawnGrenade();
    }

    private void SpawnGrenade()
    {
        if (remainingGrenades < 1)
        {
            return;
        }
        remainingGrenades--;
        PoolManager.Pull("Grenade", this.transform.position, Quaternion.identity);
    }

    public void MakeAngry()
    {
        this.GetComponent<Animator>().SetInteger("Anger", 11);
    }

    private void MoveFaerie(Vector3 delta)
    {
        faerie.localPosition += delta;
    }

    private Vector3 GetRandomVector()
    {
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * radius;
        randomPoint += radius * Vector3.up;
        return (randomPoint - faerie.localPosition) / faerieSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, cullRadius);
    }
}
