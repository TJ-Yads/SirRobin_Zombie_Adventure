using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour//game controller managed the games spawn system along with music and stats
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public Vector3 spawnValues;
    public float sphereRadius;
    public float sphereRadiusOuter;
    public Transform sphere;
    private float counter = 0;
    public Transform []ZombieSpawns;
    private int rangeValue;
    private Transform SpawnPosition;
    public GameObject PauseScreen;
    public GameObject PauseBackground;
    public GameObject BasicTextSet;
    public GameObject AbilityImages;

    // game objects for the weapon selecters
    public GameObject WeaponSelecter;

    private PlayerController playercontroller;
    
    //game objects for zombies
    public Transform zombieRotation;
    public GameObject basicZombie;
    public GameObject TankZombie;
    public GameObject RunnerZombie;
    public GameObject ElementalZombie;
    public GameObject Exploder;
    public GameObject Gunner;
    public GameObject Goliath;
    public int basicZombieCount;
    public int tankZombieCount;
    public int runnerZombieCount;
    public int elementalZombieCount;
    public int exploderZombieCount;
    public int gunnerZombieCount;
    public int goliathZombieCount;
    public int ZomsAlive;
    public int ZomsLeft;
    public int ZomsKilled = 0;

    //values for spawning zombies
    public float startWait;
    public float spawnWait;
    public float spawnWaitHeavy;
    public float spawnWaitRunner;
    public float spawnWaitExp;
    public float spawnWaitEle;
    public float spawnWaitGuns;
    public float spawnWaitGolths;
    public float waveWait;
    public int waveTotal = 1;

    //text values
    public Text WaveTracker;
    public Text DeathText;
    public Text kills;
    public GameObject WeaponText;
    public GameObject DeathTextSet;
    private bool paused;
    private bool menu;
    private bool CountBooster;
    public bool Death;
    //audio values
    public AudioSource ClickSound;
    public AudioSource Song1;
    public AudioSource SongIntro;
    public AudioSource Song2;
    public AudioSource RoundAudio1;
    public AudioSource RoundAudio2;
    // Use this for initialization
    void Start()
    {
        Death = false;
        StartCoroutine(SpawnBasicZombies());
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        paused = false;
        menu = false;
        CountBooster = false;
        ZomsLeft = basicZombieCount;
        Time.timeScale = 1;
        StartCoroutine(IntroSong());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Menu"))//allows the player to pause the game
        {
            if (menu == true)
            {
                ContinueGame();
            }
            else PauseGame();
        }
        if (Input.GetButtonDown("Pause"))
        {
            if (paused == true)
            {
                FullResume();
            }
            else FullPause();
        }
        if (CountBooster == true)
        {
            counter += Time.deltaTime;
        }
    }
    public IEnumerator SpawnBasicZombies()//primary loop for the spawn system, this loop spawns normal zombies and starts the extra zombie variants based on wave total
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            while (ZomsAlive <= 35)//loop to prevent more then 35 normal zombies from being alive at the same time
            {
                for (int i = 0; i < basicZombieCount; i++)
                {
                    SpawnPointRange();//run a spawn range function to spawn an enemy nearby the player
                    //check range based on player position and a sphere radius
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer >= (sphereRadius * sphereRadius) && DistanceFromPlayer <= (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(basicZombie, spawnPosition, zombieRotation.rotation);
                        Instantiate(basicZombie, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;//it spawning failed then roll loop backward by 1
                    yield return new WaitForSeconds(spawnWait);
                }
                while (ZomsLeft > 0)//a waiting loop that prevents the next wave until the player kills the last zombie
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                WaveIncrease();
                basicZombieCount += 2;
                ZomsLeft += basicZombieCount;
                //if the wave counter is equal to the below values then start the coroutine to spawn new enemy types
                if (waveTotal == 4)
                {
                    StartCoroutine(SpawnRunnerZombies());
                }
                if (waveTotal == 7)
                {
                    StartCoroutine(SpawnTankZombies());
                }
                if (waveTotal == 10)
                {
                    StartCoroutine(SpawnBombZombies());
                }
                if (waveTotal == 13)
                {
                    StartCoroutine(SpawnEleZombies());
                }
                if (waveTotal == 17)
                {
                    StartCoroutine(SpawnGunners());
                }
                if (waveTotal == 20)
                {
                    StartCoroutine(SpawnGoliaths());
                }
            }
        }
    }
    //spawn coroutines use simplified versions of the above loop
    public IEnumerator SpawnTankZombies()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += tankZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < tankZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(TankZombie, spawnPosition, zombieRotation.rotation);
                        Instantiate(TankZombie, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitHeavy);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                tankZombieCount += 1;
                ZomsLeft += tankZombieCount;
            }
        }
    }
    public IEnumerator SpawnRunnerZombies()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += runnerZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < runnerZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(RunnerZombie, spawnPosition, zombieRotation.rotation);
                        Instantiate(RunnerZombie, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitRunner);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                runnerZombieCount += 1;
                ZomsLeft += runnerZombieCount;
            }
        }
    }
    public IEnumerator SpawnEleZombies()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += elementalZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < elementalZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(ElementalZombie, spawnPosition, zombieRotation.rotation);
                        Instantiate(ElementalZombie, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitEle);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                elementalZombieCount += 1;
                ZomsLeft += elementalZombieCount;
            }
        }
    }
    public IEnumerator SpawnBombZombies()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += exploderZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < exploderZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(Exploder, spawnPosition, zombieRotation.rotation);
                        Instantiate(Exploder, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitExp);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                exploderZombieCount += 1;
                ZomsLeft += exploderZombieCount;
            }
        }
    }
    public IEnumerator SpawnGunners()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += gunnerZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < gunnerZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(Gunner, spawnPosition, zombieRotation.rotation);
                        Instantiate(Gunner, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitGuns);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                gunnerZombieCount += 1;
                ZomsLeft += gunnerZombieCount;
            }
        }
    }
    public IEnumerator SpawnGoliaths()
    {
        yield return new WaitForSeconds(startWait);
        ZomsLeft += goliathZombieCount;
        while (true)
        {
            while (ZomsAlive <= 35)
            {
                for (int i = 0; i < goliathZombieCount; i++)
                {
                    SpawnPointRange();
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, Random.Range(-spawnValues.z, spawnValues.z));
                    float DistanceFromPlayer = Vector3.SqrMagnitude(spawnPosition - sphere.position);
                    if (DistanceFromPlayer > (sphereRadius * sphereRadius) && DistanceFromPlayer < (sphereRadiusOuter * sphereRadiusOuter))
                    {
                        //Instantiate(Goliath, spawnPosition, zombieRotation.rotation);
                        Instantiate(Goliath, SpawnPosition.position, zombieRotation.rotation);
                        ZomsAlive += 1;
                    }
                    else i--;
                    yield return new WaitForSeconds(spawnWaitGolths);
                }
                while (ZomsLeft > 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(waveWait);
                goliathZombieCount += 1;
                ZomsLeft += goliathZombieCount;
            }
        }
    }

    public IEnumerator IntroSong()//play the games song loop
    {
        SongIntro.Play();
        yield return new WaitForSeconds(60f);
        Song2.Play();
    }
    public IEnumerator RoundAudio()//upon beating a round play a small audio file
    {
        RoundAudio1.Play();
        RoundAudio2.Play();
        yield return new WaitForSeconds(.5f);
        StopCoroutine(RoundAudio());
    }
    public void PauseGame()
    {
        WeaponSelecter.SetActive(true);
        WeaponText.SetActive(true);
        menu = true;
        ClickSound.Play();
    }
    private void ContinueGame()
    {
        WeaponSelecter.SetActive(false);
        WeaponText.SetActive(false);
        menu = false;
        ClickSound.Play();
    }
    private void FullPause()
    {
        Time.timeScale = 0;
        paused = true;
        PauseScreen.SetActive(true);
        PauseBackground.SetActive(true);
        BasicTextSet.SetActive(false);
        AbilityImages.SetActive(false);
        ClickSound.Play();
    }
    private void FullResume()
    {
        Time.timeScale = 1;
        paused = false;
        PauseScreen.SetActive(false);
        PauseBackground.SetActive(false);
        BasicTextSet.SetActive(true);
        AbilityImages.SetActive(true);
        ClickSound.Play();
    }
    public void WaveIncrease()
    {
        StartCoroutine(RoundAudio());
        waveTotal += 1;
        WaveTracker.text = "Wave: " + waveTotal;
        counter = 0;
        CountBooster = false;
    }
    public void YouDied()
    {
        Death = true;
        DeathTextSet.SetActive(true);
        kills.text = ("You killed " + ZomsKilled + (" Zombies."));
        DeathText.text = ("You survived " + waveTotal + (" Waves."));
        Song1.Play();
    }
    public void SpawnPointRange()//find area in which the zombie will attempt to spawn at
    {
        rangeValue = Random.Range(0, 18);
        if (rangeValue == 0)
        {
            SpawnPosition = ZombieSpawns[0];
        }
        else if (rangeValue == 1)
        {
            SpawnPosition = ZombieSpawns[1];
        }
        else if (rangeValue == 2)
        {
            SpawnPosition = ZombieSpawns[2];
        }
        else if (rangeValue == 3)
        {
            SpawnPosition = ZombieSpawns[3];
        }
        else if (rangeValue == 4)
        {
            SpawnPosition = ZombieSpawns[4];
        }
        else if (rangeValue == 5)
        {
            SpawnPosition = ZombieSpawns[5];
        }
        else if (rangeValue == 6)
        {
            SpawnPosition = ZombieSpawns[6];
        }
        else if (rangeValue == 7)
        {
            SpawnPosition = ZombieSpawns[7];
        }
        else if (rangeValue == 8)
        {
            SpawnPosition = ZombieSpawns[8];
        }
        else if (rangeValue == 9)
        {
            SpawnPosition = ZombieSpawns[9];
        }
        else if (rangeValue == 10)
        {
            SpawnPosition = ZombieSpawns[10];
        }
        else if (rangeValue == 11)
        {
            SpawnPosition = ZombieSpawns[11];
        }
        else if (rangeValue == 12)
        {
            SpawnPosition = ZombieSpawns[12];
        }
        else if (rangeValue == 13)
        {
            SpawnPosition = ZombieSpawns[13];
        }
        else if (rangeValue == 14)
        {
            SpawnPosition = ZombieSpawns[14];
        }
        else if (rangeValue == 15)
        {
            SpawnPosition = ZombieSpawns[15];

        }
        else if (rangeValue == 16)
        {
            SpawnPosition = ZombieSpawns[16];
        }
        else if (rangeValue == 17)
        {
            SpawnPosition = ZombieSpawns[17];
        }
        else if (rangeValue == 18)
        {
            SpawnPosition = ZombieSpawns[18];
        }
        else if (rangeValue == 19)
        {
            SpawnPosition = ZombieSpawns[19];
        }
        else if (rangeValue == 20)
        {
            SpawnPosition = ZombieSpawns[20];
        }
    }
}
