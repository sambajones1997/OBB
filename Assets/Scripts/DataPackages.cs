using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePackage
{
    public int mDamage;
    public GameObject mDamageDealer;

    public DamagePackage(int damage, GameObject dmgDealer)
    {
        mDamage = damage;
        mDamageDealer = dmgDealer;
    }

}

public class ScorePackage
{
    public int mScore;
    public bool mPlayerKill;

    public ScorePackage(int score, bool playerKill = true)
    {
        mScore = score;
        mPlayerKill = playerKill;
    }
}

