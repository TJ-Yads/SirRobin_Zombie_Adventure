using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    //player controller that allows movement, abilites, firing and item purchases
    //genral game values
    private float health = 100;
    private float speed = 9.5f;
    private Rigidbody rb;
    public int score;
    private float counter = 0;
    private float AudioCooldown = 0;
    private float RocketCounter = 1;
    private float AbilityCounter = 0;
    private float boostedFire = 1;
    public GameObject ZomTargeter;
    private int maxHealth = 100;
    private int healthCost = 500;
    public float dmgBonus = 1;
    private int buffCost = 500;
    private float EnemyBuff = 1;

    // classes used for weapons
    public GameObject PistolShot;
    public GameObject shotgunShot;
    public GameObject ARifleShot;
    public GameObject RShotgunShot;
    public GameObject RocketShot;
    public GameObject SniperShot;
    public GameObject RailgunShot;
    public GameObject BlasterShot;
    public GameObject SweeperShot;
    public GameObject UnknownShot;
    public GameObject MuzzleFlash;
    public Transform shotSpawn;
    public Transform shotSpawn2;
    public Transform shotSpawn3;
    public Transform shotSpawn4;
    public Transform shotSpawn5;
    public GameObject[] WeaponVisual;

    //classes used for fire rates
    public float pistolRate;
    public float shotgunRate;
    public float ARifleRate;
    public float RShotgunRate;
    public float RocketRate;
    public float SniperRate;
    public float RailgunRate;
    public float BlasterRate;
    public float SweeperRate;
    public float UnknownRate;
    public float healRate;
    public float FBlastRate;
    public float FBoosterRate;
    public float EBombRate;
    public float SBombRate;
    public float NukeRate;
    private float nextFire;
    private float nextAbility;
    private float AbilityReduction = 1;

    //classes used for abilities
    public GameObject Healing;
    public GameObject FireBomb;
    public GameObject ElementalBomb;
    public GameObject SlowingBomb;
    public GameObject NukeBomb;
    public Transform Ability;
    public GameObject[] AbilityVisual;

    //text values
    public Text healthTracker;
    public Text scoreTracker;
    public Text HealthCost;
    public Text DMGCost;
    public Text AbilityTimer;

    //values used for audio
    public AudioSource ShotgunSound;
    public AudioSource SniperSound;
    public AudioSource GunshotSound;
    public AudioSource LasersSound;
    public AudioSource SweeperSound;
    public AudioSource UnknownSound;
    public AudioSource RocketSound;
    public AudioSource HealSound;
    public AudioSource FireBoosterSound;
    public AudioSource ClickSound;
    public GameObject ExplosionAudio;

    //boolean values
    private bool pistol;
    private bool shotgun;
    private bool ARifle;
    private bool Rocket;
    private bool Sniper;
    private bool RShotgun;
    private bool Railgun;
    private bool Blaster;
    private bool Sweeper;
    private bool unknown;
    private bool heal;
    private bool FireBlast;
    private bool AttackBooster;
    private bool SlowingZone;
    private bool ElementalBlast;
    private bool Nuke;
    private GameController gameController;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        pistol = true;
        shotgun = false;
        ARifle = false;
        Rocket = false;
        Sniper = false;
        RShotgun = false;
        Railgun = false;
        Blaster = false;
        Sweeper = false;
        unknown = false;
        heal = true;
        FireBlast = false;
        AttackBooster = false;
        SlowingZone = false;
        ElementalBlast = false;
        Nuke = false;
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        //healths the player after sometime
        counter += Time.deltaTime;
        if (counter >= 3)
        {
            health += 25 * dmgBonus;
            counter = 0;
        }
        if (health <= 0)
        {
            health = 0;
            gameObject.SetActive(false);
            ZomTargeter.SetActive(true);
            gameController.YouDied();
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthTracker.text = "Health: " + Mathf.Round(health);
        scoreTracker.text = "$" + score;

        float movehorizontal = Input.GetAxis("Horizontal");
        float movevertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(movehorizontal, 0.0f, movevertical);
        rb.velocity = (movement * speed);

        //code set that allows for buying weapons
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
            {
                return;
            }
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);//used for buying new weapons or abilites
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Pistol" && score >= 50 && pistol == false)//each weapon/ability button uses a tag and if the tag is pressed and the player doesnt own it while also having enough money then it buys the item
                {
                    WeaponDisabler();//turns off old weapons
                    pistol = true;//enable the new weapon
                    WeaponVisual[0].SetActive(true);//change UI to new weapon
                    score -= 50;//reduce score or money amount
                    ClickSound.Play();//play audio
                }
                if (hitInfo.transform.gameObject.tag == "Shotgun" && score >= 100 && shotgun == false)
                {
                    WeaponDisabler();
                    shotgun = true;
                    WeaponVisual[1].SetActive(true);
                    score -= 100;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "ARifle" && score >= 500 && ARifle == false)
                {
                    WeaponDisabler();
                    ARifle = true;
                    WeaponVisual[2].SetActive(true);
                    score -= 500;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Rocket" && score >= 3000 && Rocket == false)
                {
                    WeaponDisabler();
                    Rocket = true;
                    WeaponVisual[5].SetActive(true);
                    score -= 3000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Sniper" && score >= 3000 && Sniper == false)
                {
                    WeaponDisabler();
                    Sniper = true;
                    WeaponVisual[4].SetActive(true);
                    score -= 3000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Railgun" && score >= 8000 && Railgun == false)
                {
                    WeaponDisabler();
                    Railgun = true;
                    WeaponVisual[6].SetActive(true);
                    score -= 8000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "RShotgun" && score >= 2000 && RShotgun == false)
                {
                    WeaponDisabler();
                    RShotgun = true;
                    WeaponVisual[3].SetActive(true);
                    score -= 2000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Blaster" && score >= 10000 && Blaster == false)
                {
                    WeaponDisabler();
                    Blaster = true;
                    WeaponVisual[7].SetActive(true);
                    score -= 10000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Sweeper" && score >= 15000 && Sweeper == false)
                {
                    WeaponDisabler();
                    Sweeper = true;
                    WeaponVisual[8].SetActive(true);
                    score -= 15000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Heal" && score >= 500 && heal == false)
                {
                    AbilityDisabler();
                    AbilityVisual[0].SetActive(true);
                    heal = true;
                    score -= 500;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "FireGrab" && score >= 1000 && FireBlast == false)
                {
                    AbilityDisabler();
                    AbilityVisual[1].SetActive(true);
                    FireBlast = true;
                    score -= 1000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "SlowGrab" && score >= 2000 && SlowingZone== false)
                {
                    AbilityDisabler();
                    AbilityVisual[2].SetActive(true);
                    SlowingZone = true;
                    score -= 2000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "EleGrab" && score >= 8000 && ElementalBlast == false)
                {
                    AbilityDisabler();
                    AbilityVisual[4].SetActive(true);
                    ElementalBlast = true;
                    score -= 8000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "NukeGrab" && score >= 15000 && Nuke == false)
                {
                    AbilityDisabler();
                    AbilityVisual[5].SetActive(true);
                    Nuke = true;
                    score -= 15000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "FBoostGrab" && score >= 5000 && AttackBooster == false)
                {
                    AbilityDisabler();
                    AbilityVisual[3].SetActive(true);
                    AttackBooster = true;
                    score -= 5000;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "HealthBoost" && score >= healthCost)//variant of item purchases that increase in cost everytime you buy it
                {
                    maxHealth += 50;
                    AbilityReduction -= .03f; 
                    score -= healthCost;
                    healthCost = healthCost * 2;
                    HealthCost.text = "$" + healthCost;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "DamageBoost" && score >= buffCost)
                {
                    dmgBonus += .2f;
                    score -= buffCost;
                    buffCost = buffCost * 2;
                    DMGCost.text = "$" + buffCost;
                    ClickSound.Play();
                }
                if (hitInfo.transform.gameObject.tag == "Unknown" && score >= 100000 && unknown == false)
                {
                    WeaponDisabler();
                    unknown = true;
                    WeaponVisual[9].SetActive(true);
                    score -= 100000;
                }
            }
        }
        //code set the allows for guns to fire
        AudioCooldown += Time.deltaTime;
        if (Input.GetButton("Fire1")&& Time.time > (nextFire * boostedFire))//while firing it will update you attack speed based on boostedFire
        {
            if (pistol)//check weapon type
            {
                nextFire = Time.time + pistolRate;//find attack delay value
                Instantiate(PistolShot, shotSpawn.position, shotSpawn.rotation);//create bullet object
                StartCoroutine(MuzzleFlasher());//spawn muzzle flash
                if (AudioCooldown > .1)//play audio if not in cooldown
                {
                    GunshotSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (shotgun)
            {
                nextFire = Time.time + shotgunRate;
                Instantiate(shotgunShot, shotSpawn.position, shotSpawn.rotation);
                Instantiate(shotgunShot, shotSpawn2.position, shotSpawn2.rotation);
                Instantiate(shotgunShot, shotSpawn3.position, shotSpawn3.rotation);
                Instantiate(shotgunShot, shotSpawn4.position, shotSpawn4.rotation);
                Instantiate(shotgunShot, shotSpawn5.position, shotSpawn5.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    ShotgunSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (ARifle)
            {
                nextFire = Time.time + ARifleRate;
                Instantiate(ARifleShot, shotSpawn.position, shotSpawn.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    GunshotSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (Rocket)
            {
                nextFire = Time.time + RocketRate;
                Instantiate(RocketShot, shotSpawn.position, shotSpawn.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    RocketSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (Sniper)
            {
                nextFire = Time.time + SniperRate;
                Instantiate(SniperShot, shotSpawn.position, shotSpawn.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    SniperSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (RShotgun)
            {
                nextFire = Time.time + RShotgunRate;
                Instantiate(RShotgunShot, shotSpawn.position, shotSpawn.rotation);
                Instantiate(RShotgunShot, shotSpawn2.position, shotSpawn2.rotation);
                Instantiate(RShotgunShot, shotSpawn3.position, shotSpawn3.rotation);
                Instantiate(RShotgunShot, shotSpawn4.position, shotSpawn4.rotation);
                Instantiate(RShotgunShot, shotSpawn5.position, shotSpawn5.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    ShotgunSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (Railgun)
            {
                nextFire = Time.time + RailgunRate;
                Instantiate(RailgunShot, shotSpawn.position, shotSpawn.rotation);
                if (AudioCooldown > .1)
                {
                    LasersSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (Blaster)
            {
                nextFire = Time.time + BlasterRate;
                Instantiate(BlasterShot, shotSpawn.position, shotSpawn.rotation);
                if (AudioCooldown > .1)
                {
                    UnknownSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (Sweeper)
            {
                nextFire = Time.time + SweeperRate;
                Instantiate(SweeperShot, shotSpawn.position, shotSpawn.rotation);
                Instantiate(SweeperShot, shotSpawn2.position, shotSpawn2.rotation);
                Instantiate(SweeperShot, shotSpawn3.position, shotSpawn3.rotation);
                Instantiate(SweeperShot, shotSpawn4.position, shotSpawn4.rotation);
                Instantiate(SweeperShot, shotSpawn5.position, shotSpawn5.rotation);
                StartCoroutine(MuzzleFlasher());
                if (AudioCooldown > .1)
                {
                    SweeperSound.Play();
                    AudioCooldown = 0;
                }
            }
            if (unknown)
            {
                nextFire = Time.time + UnknownRate;
                Instantiate(UnknownShot, shotSpawn.position, shotSpawn.rotation);
                Instantiate(UnknownShot, shotSpawn2.position, shotSpawn2.rotation);
                Instantiate(UnknownShot, shotSpawn3.position, shotSpawn3.rotation);
                Instantiate(UnknownShot, shotSpawn4.position, shotSpawn4.rotation);
                Instantiate(UnknownShot, shotSpawn5.position, shotSpawn5.rotation);
                if (AudioCooldown > .1)
                {
                    UnknownSound.Play();
                    AudioCooldown = 0;
                }
            }
        }
        //code set the allows for abilities
        if (Input.GetButton("Fire2") && Time.time > nextAbility)//use a player ability
        {
            if (heal)//heal the player
            {
                nextAbility = Time.time + healRate * AbilityReduction;
                health += 50 * dmgBonus;
                Instantiate(Healing, Ability.position, Ability.rotation);
                AbilityCounter = healRate * AbilityReduction;
                HealSound.Play();
            }
            if (FireBlast)//create a large blast and heal
            {
                nextAbility = Time.time + FBlastRate * AbilityReduction;
                Instantiate(FireBomb, Ability.position, Ability.rotation);
                Instantiate(ExplosionAudio, Ability.position, Ability.rotation);
                health += 65 * dmgBonus;
                AbilityCounter = FBlastRate * AbilityReduction;
            }
            if (AttackBooster)//increase fire rate and heal
            {
                nextAbility = Time.time + FBoosterRate * AbilityReduction;
                StartCoroutine(FireRateBooster());
                health += 110 * dmgBonus;
                AbilityCounter = FBoosterRate * AbilityReduction;
                FireBoosterSound.Play();
            }
            if (SlowingZone)//create a large slowing zone and heal
            {
                nextAbility = Time.time + SBombRate * AbilityReduction;
                Instantiate(SlowingBomb, Ability.position, Ability.rotation);
                health += 120 * dmgBonus;
                AbilityCounter = SBombRate * AbilityReduction;
            }
            if (ElementalBlast)//create a large damage blast and heal
            {
                nextAbility = Time.time + EBombRate * AbilityReduction;
                Instantiate(ElementalBomb, Ability.position, Ability.rotation);
                Instantiate(ExplosionAudio, Ability.position, Ability.rotation);
                health += 140 * dmgBonus;
                AbilityCounter = EBombRate * AbilityReduction;
            }
            if (Nuke)//create a very large damage blast and heal
            {
                nextAbility = Time.time + NukeRate * AbilityReduction;
                Instantiate(NukeBomb, Ability.position, Ability.rotation);
                Instantiate(ExplosionAudio, Ability.position, Ability.rotation);
                health += 170 * dmgBonus;
                AbilityCounter = NukeRate * AbilityReduction;
            }
        }
        //tracks player ability cooldown
        AbilityCounter -= Time.deltaTime;
        AbilityTimer.text = "" + Mathf.Round(AbilityCounter);
        if (AbilityCounter <= 0)
        {
            AbilityCounter = 0;
            AbilityTimer.text = "Ability ready";
        }
        EnemyBuff = 1 + gameController.waveTotal * .02f;//update enemy buff which is the damage the deal to the player
    }
    private void OnTriggerEnter(Collider other)//upon touching an enemy take damage based on enemy type and enemyBuff
    {
        counter = 0;
        if (other.tag == "BasicZom")
        {
            health -= 8 * EnemyBuff;
        }
        if (other.tag == "MedZom")
        {
            health -= 12 * EnemyBuff;
        }
        if (other.tag == "EleZom")
        {
            health -= 18 * EnemyBuff;
        }
        if (other.tag == "BombZom")
        {
            health -= 20 * EnemyBuff;
        }
        if (other.tag == "GolthZom")
        {
            health -= 30 * EnemyBuff;
        }
        if (other.tag == "GunZom")
        {
            Destroy(other.gameObject);
            health -= 18 * EnemyBuff;
        }
        if (other.tag == "RocketBomb" && RocketCounter >= 1)
        {
            RocketCounter = 0;
            health -= 25;
            StartCoroutine(RocketCounterCo());
        }
    }
    private void OnTriggerStay(Collider other)//while inside damage over time zones take various effects
    {
        if (other.tag == "ZomFire")//take damage over time
        {
            counter += Time.deltaTime;
            if (counter >= 1)
            {
                health -= 5 * EnemyBuff;
                counter = 0;
            }
        }
        if (other.tag == "ZomReduce")//apply slow to self
        {
            counter += Time.deltaTime;
            if (counter >= 1)
            {
                StartCoroutine(Slowed());
                counter = 0;
            }
        }
        if (other.tag == "SpawnPit")//apply sludge to self
        {
                StartCoroutine(Sludge());
        }
    }
    public IEnumerator FireRateBooster()//increase fire rate for 3 seconds
    {
        boostedFire = .5f;
        yield return new WaitForSeconds(3f);
        boostedFire = 1;
        StopCoroutine(FireRateBooster());
    }
    public IEnumerator RocketCounterCo()//prevent bombs from hurting more then once in a second
    {
        yield return new WaitForSeconds(.75f);
        RocketCounter = 1;
        StopCoroutine(RocketCounterCo());
    }
    public IEnumerator Slowed()//reduce movement speed for 4 seconds
    {
        speed = 6.5f;
        yield return new WaitForSeconds(4f);
        speed = 9.5f;
        StopCoroutine(Slowed());
    }
    public IEnumerator Sludge()//reduce movement speed for 6 seconds
    {
        speed = 8;
        yield return new WaitForSeconds(6f);
        speed = 9.5f;
        StopCoroutine(Slowed());
    }
    public IEnumerator MuzzleFlasher()//create a muzzle flash when firing 
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        MuzzleFlash.SetActive(false);
        StopCoroutine(MuzzleFlasher());
    }
    public void WeaponDisabler()//turn off weapon when buying a new weapon
    {
        pistol = false;
        shotgun = false;
        ARifle = false;
        Rocket = false;
        Sniper = false;
        RShotgun = false;
        Railgun = false;
        Blaster = false;
        Sweeper = false;
        unknown = false;
        WeaponVisual[0].SetActive(false);
        WeaponVisual[1].SetActive(false);
        WeaponVisual[2].SetActive(false);
        WeaponVisual[3].SetActive(false);
        WeaponVisual[4].SetActive(false);
        WeaponVisual[5].SetActive(false);
        WeaponVisual[6].SetActive(false);
        WeaponVisual[7].SetActive(false);
        WeaponVisual[8].SetActive(false);
        WeaponVisual[9].SetActive(false);
    }
    public void AbilityDisabler()//turn off abilites when buying a new ability
    {
        heal = false;
        FireBlast = false;
        AttackBooster = false;
        SlowingZone = false;
        ElementalBlast = false;
        Nuke = false;
        AbilityVisual[0].SetActive(false);
        AbilityVisual[1].SetActive(false);
        AbilityVisual[2].SetActive(false);
        AbilityVisual[3].SetActive(false);
        AbilityVisual[4].SetActive(false);
        AbilityVisual[5].SetActive(false);
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
    }
}
