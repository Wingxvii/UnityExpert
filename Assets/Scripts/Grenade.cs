using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Nightmare
{
    public class Grenade : MonoBehaviour
    {
        public float explosiveForce = 500f;
        public int explosiveDamage = 50;
        public float explosiveRadius = 2f;
        public float timeOut = 3f;

        bool isPickup;
        Rigidbody rb;
        ParticleSystem ps;
        MeshRenderer mr;
        float timer = 0f;
        float destroyWait;

        void Awake()
        {
            rb = this.GetComponent<Rigidbody>();
            mr = this.GetComponent<MeshRenderer>();
            ps = this.GetComponentInChildren<ParticleSystem>();
            
            ParticleSystem.MainModule pMain = ps.main;
            destroyWait = Mathf.Max(pMain.startLifetime.constantMin, pMain.startLifetime.constantMax);
        }

        void OnEnable()
        {
            timer = 0f;
            mr.enabled = true;
            ps.Stop();
            isPickup = true;
        }

        void Update()
        {            
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    Explode();
                }
            }
        }

        void OnTriggerEnter(Collider coll)
        {
            if (isPickup)
            {
                if (coll.tag == "Player")
                {
                    Disable();
                    PlayerShooting shooting = FindObjectOfType<PlayerShooting>();
                    if (shooting != null)
                        shooting.CollectGrenade();
                }
            }
            else
            {
                if (coll.tag == "Enemy")
                {
                    Explode();
                }
            }
        }

        public void Shoot(Vector3 force)
        {
            if (timer > 0f)
                return;

            isPickup = false;
            mr.enabled = true;
            timer = timeOut;
            rb.AddForce(force);
        }

        private void Explode()
        {
            timer = -1;
            ps.Play();
            mr.enabled = false;

            Collider[] colls = Physics.OverlapSphere(this.transform.position, explosiveRadius);
            for (int i = 0; i < colls.Length; i++)
            {                
                if (colls[i].tag == "Enemy" && !colls[i].isTrigger)
                {
                    EnemyHealth victim = colls[i].GetComponent<EnemyHealth>();
                    if (victim != null)
                    {
                        victim.TakeDamage(explosiveDamage);
                    }
                }
            }

            StartCoroutine("TimedDisable");
        }

        private IEnumerator TimedDisable()
        {
            yield return new WaitForSeconds(destroyWait);
            Disable();
        }

        private void Disable()
        {
            timer = -1;
            isPickup = false;
            this.gameObject.SetActive(false);
        }
    }
}

