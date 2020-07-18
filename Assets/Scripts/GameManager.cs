using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum EEnemyType { Ship,Fighter,Drone,Mine};
    public int mMaxNumberOfFighters = 3;
    public int mMaxNumberOfShips = 3;
    public int mMaxNumberOfDrones = 1;
    public int mMaxNumberOfMines = 2;

    public int mCurrentNumberOfFighters = 0;
    public int mCurrentNumberOfShips = 0;
    public int mCurrentNumberOfDrones = 0;
    public int mCurrentNumberOfMines = 0;

    float mTimeBetweenFighters = 1.0f;
    float mTimeBetweenShips = 1.0f;
    float mTimeBetweenDrones = 1.0f;
    float mTimeBetweenMines = 1.0f;

    public int mNumberOfFightersKilled = 0;
    public int mNumberOfShipsKilled = 0;
    public int mNumberOfDronesKilled = 0;
    public int mNumberOfMinesKilled = 0;

    public bool bIsPlayerAlive = false;

    public GameObject mFighterObject;
    public GameObject mShipObject;
    public GameObject mDroneObject;
    public GameObject mMineObject;

    float mBoundaryX = 135.0f;
    float mBoundaryZ = 85.0f;

    public List<GameObject> mPlanets;
    public Leaderboard mLeaderBoard;

    private void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        mTimeBetweenFighters = Random.Range(0.0f, 5.0f);
        mTimeBetweenShips = Random.Range(0.0f, 5.0f);
        mTimeBetweenDrones = Random.Range(0.0f, 5.0f);
        mTimeBetweenMines = Random.Range(0.0f, 5.0f);
        Instantiate(mPlanets[Random.Range(0, mPlanets.Count - 1)], new Vector3(0.0f, -2.0f, 0.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
    }

    private void FixedUpdate()
    {
        if (bIsPlayerAlive)
        {
            if (mCurrentNumberOfFighters < mMaxNumberOfFighters)
            {
                if ((mTimeBetweenFighters -= Time.fixedDeltaTime) <= 0.0f)
                {
                    SpawnEnemies(EEnemyType.Fighter);
                    mTimeBetweenFighters = Random.Range(0.0f, 5.0f);
                }
            }
            if (mCurrentNumberOfDrones < mMaxNumberOfDrones)
            {
                if ((mTimeBetweenDrones -= Time.fixedDeltaTime) <= 0.0f)
                {
                    SpawnEnemies(EEnemyType.Drone);
                    mTimeBetweenDrones = Random.Range(0.0f, 5.0f);
                }
            }
            if (mCurrentNumberOfMines < mMaxNumberOfMines)
            {
                if ((mTimeBetweenMines -= Time.fixedDeltaTime) <= 0.0f)
                {
                    SpawnEnemies(EEnemyType.Mine);
                    mTimeBetweenMines = Random.Range(0.0f, 5.0f);
                }
            }
            if (mCurrentNumberOfShips < mMaxNumberOfShips)
            {
                if ((mTimeBetweenShips -= Time.fixedDeltaTime) <= 0.0f)
                {
                    SpawnEnemies(EEnemyType.Ship);
                    mTimeBetweenShips = Random.Range(0.0f, 5.0f);
                }
            }
        }
    }

    public void SpawnEnemies(EEnemyType enemyType)
    {
        switch (enemyType)
        {
            case EEnemyType.Drone:
                Instantiate(mDroneObject, GetSpawnLocation(), new Quaternion());
                mCurrentNumberOfDrones++;
                break;
            case EEnemyType.Mine:
                Instantiate(mMineObject, GetSpawnLocation(), new Quaternion());
                mCurrentNumberOfMines++;
                break;
            case EEnemyType.Ship:
                Instantiate(mShipObject, GetSpawnLocation(), new Quaternion());
                mCurrentNumberOfShips++;
                break;
            case EEnemyType.Fighter:
                Instantiate(mFighterObject, GetSpawnLocation(), new Quaternion());
                mCurrentNumberOfFighters++;
                break;
        }
    }

    Vector3 GetSpawnLocation()
    {
        if (Random.Range(0,2) == 0)
        {
            // Spawn X
            if (Random.Range(0, 2) == 0) return new Vector3(-mBoundaryX, 0.0f, Random.Range(-mBoundaryZ, mBoundaryZ)); // Left X
            else return new Vector3(mBoundaryX, 0.0f, Random.Range(-mBoundaryZ, mBoundaryZ)); // Right X
        }
        else
        {
            // Spawn Z
            if (Random.Range(0, 2) == 0) return new Vector3(Random.Range(-mBoundaryX, mBoundaryX), 0.0f, mBoundaryZ); // Top Z
            else return new Vector3(Random.Range(-mBoundaryX, mBoundaryX), 0.0f, -mBoundaryZ); // Bottom Z
        }
    }

    public void DestroyedShip(EEnemyType enemyType)
    {
        int multiplyNumber = 5; // Number max is multiplied by to increase number that spawn
        switch (enemyType)
        {
            case EEnemyType.Drone:
                mNumberOfDronesKilled++;
                mCurrentNumberOfDrones--;
                if (mNumberOfDronesKilled >= multiplyNumber * mMaxNumberOfDrones)
                {
                    mMaxNumberOfDrones++;
                    mNumberOfDronesKilled = 0;
                }
                break;
            case EEnemyType.Fighter:
                mNumberOfFightersKilled++;
                mCurrentNumberOfFighters--;
                if (mNumberOfFightersKilled >= multiplyNumber * mMaxNumberOfFighters)
                {
                    mMaxNumberOfFighters++;
                    mNumberOfFightersKilled = 0;
                }
                break;
            case EEnemyType.Mine:
                mNumberOfMinesKilled++;
                mCurrentNumberOfMines--;
                if (mNumberOfMinesKilled >= multiplyNumber * mMaxNumberOfMines)
                {
                    mMaxNumberOfMines++;
                    mNumberOfMinesKilled = 0;
                }
                break;
            case EEnemyType.Ship:
                mNumberOfShipsKilled++;
                mCurrentNumberOfShips--;
                if (mNumberOfShipsKilled >= multiplyNumber * mMaxNumberOfShips)
                {
                    mMaxNumberOfShips++;
                    mNumberOfShipsKilled = 0;
                }
                break;
        }
    }

    public void PlayerHasDied(int mScore)
    {
        mLeaderBoard.gameObject.SetActive(true);
        mLeaderBoard.UpdateList(mScore);
    }
}
