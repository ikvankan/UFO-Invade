using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies ;
    public float ScorePerSecond = 0.5f;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster, WeaponType.spread, WeaponType.shield
    };

    private BoundsCheck bndCheck;

    public void ShipDestroyed(Enemy e)
    { // c
      // Сгенерировать бонус с заданной вероятностью
        if (Random.value <= e.powerUpDropChance)
        { // d
          // Выбрать тип бонуса
          // Выбрать один из элементов в powerUpFrequency
            int ndx = Random.Range(0, powerUpFrequency.Length); // e
            WeaponType puType = powerUpFrequency[ndx];
            // Создать экземпляр PowerUp
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            // Установить соответствующий тип WeaponType
            pu.SetType(puType);
            pu.transform.position = e.transform.position;
        }
    }

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
        Invoke("AddScore", ScorePerSecond);
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        float enemyPadding = enemyDefaultPadding;
        if(go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if(WEAP_DICT.ContainsKey(wt))
        {
            return WEAP_DICT[wt];
        }
        return new WeaponDefinition();
    }

    // Start is called before the first frame update
    void AddScore()
    {
        ScoreCount.score += 1;
        Invoke("AddScore", 1f/ScorePerSecond);
    }
    
}
