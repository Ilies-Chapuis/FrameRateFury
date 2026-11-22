using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private int maxHp = 3;
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Références")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int CurrentHp { get; private set; }
    public int MaxHp => maxHp;
    public bool IsInvincible { get; private set; }

    // Écouté par le GameManager (pas par le HUD, qui passe par GameEvents)
    public event Action OnDied;

    private WaitForSeconds blinkWait;

    private void Awake()
    {
        CurrentHp = maxHp;
        // Créé une seule fois : pas d'allocation à chaque coup reçu
        blinkWait = new WaitForSeconds(blinkInterval);
    }

    private void Start()
    {
        // Valeur de départ pour le HUD (il s'est abonné dans son OnEnable)
        GameEvents.RaiseHealthChanged(CurrentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {
        if (IsInvincible || CurrentHp <= 0) return;

        CurrentHp = Mathf.Max(0, CurrentHp - amount);
        GameEvents.RaiseHealthChanged(CurrentHp, maxHp);

        if (CurrentHp == 0)
        {
            OnDied?.Invoke();
            return;
        }

        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        IsInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return blinkWait;
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
        IsInvincible = false;
    }

    [ContextMenu("Test : prendre 1 dégât")]
    private void TestDamage() => TakeDamage(1);
}
