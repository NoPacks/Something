using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public int EnemyT;
    MeshRenderer mRenderer;
    public MeshRenderer[] Eyes;
    
    void Start()
    {
        mRenderer = GetComponent<MeshRenderer>();
        EnemyT = Random.Range(1, 4);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EnemyT = Random.Range(1, 4);
            
        }

        switch (EnemyT)
        {
            case 1: IceEnemyStats();
                break;
            case 2: FireEnemyStats();
                break;
            case 3: PlantEnemyStats();
                break;
        }
    }

    public void IceEnemyStats()
    {
        mRenderer.material.color = Color.blue;
    }

    public void FireEnemyStats()
    {
        mRenderer.material.color = Color.red;
    }

    public void PlantEnemyStats()
    {
        mRenderer.material.color = Color.green;
    }
}
