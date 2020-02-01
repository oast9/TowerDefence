using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves; //массив разных типов волн
    public Transform spawnPoint; //Исходная точка где враги будут появляться
    public Text waveCountdownText; //Отображение времени до следующей волны
    public Text surWaveText; //Отображение текущей волны
    public Text enemiesAliveText;
    public Text minionsUpgradeText;
    public Text surrentMinionsLvText;
    public float timeBetweenWaves = 5f; //максимальная задержка между волнами
    private float countdown = 10f; //время ожидания следующей волны
    //public int wavesCount = 0; //общее количество волн
    public static int enemiesAlive = 0; //всего врагов на карте в текущий момент
    // public static int healthMult = 1; //множитель жизни врагов
    // public int startHealthMult = 1; //множитель жизни врагов на старт
    public int minionsLv = 1;
    public int minionsUpgradeLv2Cost = 200;
    public int minionsUpgradeLv3Cost = 300;
    public int playerControl;
    private PlayerStats playerStats;
    public PhaseManager phaseManager;
    public int roadToGo; //по какой дороге идти

    void Start() {
        if (playerControl == 1) {
            minionsUpgradeText.text = "$" + minionsUpgradeLv2Cost.ToString();
            surrentMinionsLvText.text = minionsLv.ToString();
        }
        phaseManager = PhaseManager.instance;
        playerStats = GameObject.FindGameObjectWithTag($"Player{playerControl}").GetComponent<PlayerStats>();
        minionsLv = 1;
        //healthMult = startHealthMult;
    }
    
    void Update()
    {
        enemiesAliveText.text = enemiesAlive.ToString();
        if (enemiesAlive > 0) {
            return;
        }
 
        if (countdown <= 0f && PlayerStats.role == 1 && playerControl == 1) {
            StartCoroutine(StandartWave()); //запуск функции с паузой
            // StartCoroutine(HeavyWave());
            // StartCoroutine(FastWave());
            // if (wavesCount % 5 == 0) //Каждую 5 волну спавн боссов
            //     StartCoroutine(BossWave());
            // if (wavesCount % 10 == 0)
            //     healthMult++; //увеличение живучести врагов каждые 10 волн
            
            countdown = timeBetweenWaves;
           // wavesCount++;
            PlayerStats.Waves++;
            surWaveText.text = PlayerStats.Waves.ToString();
            return;
        }
        //Ход второго игрока с рандомным выбором дороги
        else if (countdown <= 0f && PlayerStats.role == 2 && playerControl == 2) {
            StartCoroutine(StandartWave());
            PlayerStats.Waves++;
            roadToGo = Random.Range(1,4);
            surWaveText.text = PlayerStats.Waves.ToString();
            return;
        }
        
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity); //Проверка, что откат не будет меньше нуля

        waveCountdownText.text = string.Format("{0:00.00}",countdown); //отображение числа в виде в секунд с милисекундами    

 
    }
    IEnumerator StandartWave() {
                
        Wave wave = waves[0];
        for (int i = 0; i < wave.count; i++) {
            if (minionsLv == 1)
                SpawnEnemy(wave.enemyLv1);
            else if (minionsLv == 2)
                SpawnEnemy(wave.enemyLv2);
            else if (minionsLv == 3)
                SpawnEnemy(wave.enemyLv3);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        minionsLv = 1;
        
        if (playerControl == 1) {
            minionsUpgradeText.text = "$" + minionsUpgradeLv2Cost.ToString();
            surrentMinionsLvText.text = minionsLv.ToString();
        }
        wave.count++;
    }

    public void SpawnUpgrade() {
        if (minionsLv == 1) {
            if (PlayerStats.Money < minionsUpgradeLv2Cost) {
                Debug.Log("Недостаточно средств для улучшения");
                return;
            }
            else {
                minionsLv++;
                if (playerControl == 1) {
                    surrentMinionsLvText.text = minionsLv.ToString();
                    minionsUpgradeText.text = "$" + minionsUpgradeLv3Cost.ToString();
                    PlayerStats.Money -= minionsUpgradeLv2Cost;
                }
            }

        }
        else if (minionsLv == 2) {
            if (PlayerStats.Money < minionsUpgradeLv3Cost) {
                Debug.Log("Недостаточно средств для улучшения");
                return;
            }
            else {
                minionsLv++;
                if (playerControl == 1) {
                    surrentMinionsLvText.text = minionsLv.ToString();
                    minionsUpgradeText.text = "МАКСИМУМ";
                    PlayerStats.Money -= minionsUpgradeLv3Cost;
                }
            }

        }
    }

    // //Создание волн враго по типам
    // IEnumerator HeavyWave() {
                
    //     Wave wave = waves[1];
    //     for (int i = 0; i < wave.count; i++) {
    //         SpawnEnemy(wave.enemy);
    //         yield return new WaitForSeconds(1f / wave.rate);
    //     }
    //     // if (wavesCount > 5 && wavesCount % 3 == 0) //наращивание количества врагов с волнами
    //     //     wave.count++;
    // }

    // IEnumerator FastWave() {
                
    //     Wave wave = waves[2];
    //     for (int i = 0; i < wave.count; i++) {
    //         SpawnEnemy(wave.enemy);
    //         yield return new WaitForSeconds(1f / wave.rate);
    //     }
    //     // if (wavesCount > 5 && wavesCount % 3 == 0)
    //     // wave.count += 2;
    // }
    // IEnumerator BossWave() {
                
    //     Wave wave = waves[3];
    //     for (int i = 0; i < wave.count; i++) {
    //         SpawnEnemy(wave.enemy);
    //         yield return new WaitForSeconds(1f / wave.rate);
    //     }
    //     wave.count++;
    // }
    
    void SpawnEnemy(GameObject enemy) {
        GameObject temp = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        temp.GetComponent<Enemy>().playerControl = playerControl;
        temp.GetComponent<Enemy>().roadToGo = roadToGo;
        enemiesAlive++;
    }
}
