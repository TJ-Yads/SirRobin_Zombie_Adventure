using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour {
    //script used for enemies to follow and attack the player
    //values for zombie health and points
    public float health;
    private float healthModify = 1;
    public float trueHealth;
    private float DmgReduction = 1;
    public int scoreValue;
    private float RocketCounter = 1;
    //values for targeting the player
    private Transform Player;
    public float MoveSpeed;
    private float SpeedModify = 1;
    private float SpeedIncrease = 0;
    public float MaxDist;
    public float MinDist;
    //values for attacking
    public float AttackSpeed;
    private float nextHit;
    public GameObject HitAttack;
    public Transform HitSpawn;
    public GameObject DeathEffect;
    public Transform DeathSpawn;
    public GameObject DeathSound;
    public AudioSource RandomNoise;
    private float AudioCounter = 0;

    private PlayerController playercontroller;
    private GameController gameController;
    private float counter = 0;
    // Use this for initialization
    void Start () {
        GameObject playerControllerObject = GameObject.FindWithTag("Player");
        playercontroller = playerControllerObject.GetComponent<PlayerController>();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
        SpeedIncrease += .025f * gameController.waveTotal;
        healthModify += .05f * gameController.waveTotal;
        MoveSpeed = SpeedIncrease + MoveSpeed;
        if (MoveSpeed >= 10.5)
        {
            MoveSpeed = 10.5f;
        }
        trueHealth = health * healthModify;
        Player = GameObject.FindWithTag("Player").transform;
        RandomNoiseMethod();
    }

    private void FixedUpdate()
    {
        if (gameController.Death == true)//kill the enemy through a game controller setting
        {
            Destroy(gameObject);
        }
        if (trueHealth <= 0)//kill enemy when health is 0 or lower and update game controller data
        {
            Destroy(gameObject);
            playercontroller.AddScore(scoreValue);
            gameController.ZomsAlive -= 1;
            gameController.ZomsLeft -= 1;
            gameController.ZomsKilled += 1;
            Instantiate(DeathEffect, DeathSpawn.position, DeathSpawn.rotation);
            Instantiate(DeathSound, DeathSpawn.position, DeathSpawn.rotation);
        }

        Vector3 Target = Player.transform.position;
        float TrueSpeed = MoveSpeed * SpeedModify * Time.deltaTime;
        if (Vector3.Distance(transform.position, Player.position) >= MinDist)//move toward the player while out of attack range
        {
            transform.position = Vector3.MoveTowards(transform.position, Target, TrueSpeed);
            //transform.position += transform.forward * MoveSpeed * Time.deltaTime * SpeedModify;
        }
        if (Vector3.Distance(transform.position, Player.position) <= MinDist && Time.time > nextHit)//attack the player while in range
        {
            nextHit = Time.time + AttackSpeed;
            Instantiate(HitAttack, HitSpawn.position, HitSpawn.rotation);
        }
        transform.LookAt(Player);
        AudioCounter += Time.deltaTime;
        if (AudioCounter >= 7)//cause a noise from the zombie every 7 seconds
        {
            RandomNoiseMethod();
            AudioCounter = 0;
        }
    }
    private void OnTriggerEnter(Collider other)//take damage from player attacks based on projectile and player damage value, divide it by and damage reduction
    {
        if(other.tag == "PistolBolt")
        {
            trueHealth -= 1 * playercontroller.dmgBonus * DmgReduction;
        }
        if (other.tag == "RifleBolt")
        {
            trueHealth -= 2 * playercontroller.dmgBonus * DmgReduction;
        }
        if (other.tag == "FireBomb")
        {
            trueHealth -= 75 * playercontroller.dmgBonus * DmgReduction;
        }
        if (other.tag == "RocketBolt")
        {
            trueHealth -= 10 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Stunned());
        }
        if (other.tag == "RocketBomb" && RocketCounter >= 1)
        {
            trueHealth -= 22 * playercontroller.dmgBonus * DmgReduction;
            RocketCounter = 0;
            StartCoroutine(RocketCounterCo());
        }
        if (other.tag == "SniperBolt")
        {
            trueHealth -= 45 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Slowed());
        }
        if (other.tag == "RailgunBolt")
        {
            trueHealth -= 25 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Stunned());
        }
        if (other.tag == "SweeperBolt")
        {
            trueHealth -= 15 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Slowed());
        }
        if (other.tag == "UnknownBolt")
        {
            trueHealth -= 20 * playercontroller.dmgBonus;
            StartCoroutine(Slowed());
        }
        if (other.tag == "BlasterBolt")
        {
            trueHealth -= 25 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Slowed());
        }
        if (other.tag == "BlasterBomb")
        {
            trueHealth -= 20 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Slowed());
        }
        if (other.tag == "RShotgunBolt")
        {
            trueHealth -= 2 * playercontroller.dmgBonus * DmgReduction;
        }
        if (other.tag == "EBomb")
        {
            trueHealth -= 125 * playercontroller.dmgBonus * DmgReduction;
        }
        if (other.tag == "Nuke")
        {
            trueHealth -= 200 * playercontroller.dmgBonus * DmgReduction;
            StartCoroutine(Stunned());
        }
    }
    private void OnTriggerStay(Collider other)//while in certain AOE zones take certain effects
    {
        if (other.tag == "SlowZone")//slow movement speed
        {
            StartCoroutine(Slowed());
        }
        if (other.tag == "NukeZone")//take damage over time
        {
            StartCoroutine(Slowed());
            counter += Time.deltaTime;
            if (counter >= 1)
            {
                trueHealth -= 8 * playercontroller.dmgBonus * DmgReduction;
                counter = 0;
            }
        }
        if (other.tag == "ZomFire")//heal over time
        {
            counter += Time.deltaTime;
            if (counter >= 1.5)
            {
                trueHealth += 10;
                counter = 0;
            }
        }
        if (other.tag == "ZomReduce")//take reduced damage
        {
            StartCoroutine(DMGReduction());
        }
    }
    public IEnumerator Stunned()//prevent movement for 2s
    {
        SpeedModify = .01f;
        yield return new WaitForSeconds(2f);
        SpeedModify = 1;
        StopCoroutine(Stunned());
    }
    public IEnumerator Slowed()//reduce move speed for 4s
    {
        SpeedModify = .66f;
        yield return new WaitForSeconds(4f);
        SpeedModify = 1;
        StopCoroutine(Slowed());
    }
    public IEnumerator DMGReduction()//reduce damage taken for 5s
    {
        DmgReduction = .5f;
        yield return new WaitForSeconds(5f);
        DmgReduction = 1;
        StopCoroutine(DMGReduction());
    }
    public IEnumerator RocketCounterCo()//prevent bombs from hurting multiple times in a second
    {
        yield return new WaitForSeconds(.75f);
        RocketCounter = 1;
        StopCoroutine(RocketCounterCo());
    }
    private void RandomNoiseMethod()//cause random noise with a 1 in 9 chance
    {
        int RangeValue = Random.Range(0, 9);
        if (RangeValue ==1)
        {
            RandomNoise.Play();
        }
    }
}
