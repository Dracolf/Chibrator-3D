using System.Collections.Generic;
using UnityEngine;

public class LeurreController : MonoBehaviour
{
    private static readonly List<LeurreController> activeLeurres = new();

    public float health = 500f;

    private ZiziInfosDisplay infos;
    private bool isDead;

    private void Start()
    {
        activeLeurres.Add(this);

        infos = GetComponentInChildren<ZiziInfosDisplay>();

        if (infos != null)
        {
            infos.SetMaxHealth(health);
            infos.SetHealth(health);
        }

        RefreshAllEnemyTargets();
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }

        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        activeLeurres.Remove(this);

        RefreshAllEnemyTargets();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (activeLeurres.Remove(this))
        {
            RefreshAllEnemyTargets();
        }
    }

    public static Transform GetCurrentTarget()
    {
        CleanNullLeurres();

        if (activeLeurres.Count > 0)
        {
            return activeLeurres[0].transform;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();

        if (player != null)
        {
            return player.transform;
        }

        return null;
    }

    private static void CleanNullLeurres()
    {
        for (int i = activeLeurres.Count - 1; i >= 0; i--)
        {
            if (activeLeurres[i] == null)
            {
                activeLeurres.RemoveAt(i);
            }
        }
    }

    public static void RefreshAllEnemyTargets()
    {
        ZiziController[] zizis = FindObjectsByType<ZiziController>(FindObjectsSortMode.None);
        BossController[] bosses = FindObjectsByType<BossController>(FindObjectsSortMode.None);
        TerroristeController[] terroristes = FindObjectsByType<TerroristeController>(FindObjectsSortMode.None);
        RastaController[] rastas = FindObjectsByType<RastaController>(FindObjectsSortMode.None);

        foreach (ZiziController zizi in zizis)
        {
            zizi.RefreshTarget();
        }

        foreach (BossController boss in bosses)
        {
            boss.RefreshTarget();
        }

        foreach (TerroristeController terroriste in terroristes)
        {
            terroriste.RefreshTarget();
        }

        foreach (RastaController rasta in rastas)
        {
            rasta.RefreshTarget();
        }
    }
}