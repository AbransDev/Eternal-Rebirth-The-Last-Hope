using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Resources")]
    public float food = 0f;
    public float water = 0f;
    public float wood = 0f;
    public float stone = 0f;
    public int population = 0;

    [Header("Resource Texts")]
    public TMP_Text foodText;
    public TMP_Text waterText;
    public TMP_Text woodText;
    public TMP_Text stoneText;
    public TMP_Text populationText;
    public TMP_Text timerText; // Kronometre için TMP_Text bileşeni

    public float foodGelir = 0f;
    public float foodGider = 0f;
    public float waterGelir = 0f;
    public float waterGider = 0f;
    public float woodGelir = 0f;
    public float woodGider = 0f;
    public float stoneGelir = 0f;
    public float stoneGider = 0f;

    private float timer = 0f; // Kronometre değeri
    private bool isBlinking = false; // Yanıp sönme durumu

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        updateStats();
        StartCoroutine(UpdateResourcesOverTime());
        StartCoroutine(CheckResourcesAndDecreasePlayerHealth());
    }

    private void Update()
    {
        // Kronometreyi güncelle
        timer += Time.deltaTime;
        UpdateTimerText();

        // GreenBox nesnelerinin sayısına göre giderleri güncelle
        UpdateResourceExpenses();
    }

    private void UpdateTimerText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        timerText.text = timeText;
    }

    private void UpdateResourceExpenses()
    {
        GameObject[] greenBoxes = GameObject.FindGameObjectsWithTag("GreenBox");
        int greenBoxCount = greenBoxes.Length;

        foodGider = Mathf.Round(greenBoxCount * 0.002f * 10f) / 10f;
        waterGider = Mathf.Round(greenBoxCount * 0.002f * 10f) / 10f;
        woodGider = Mathf.Round(greenBoxCount * 0.004f * 10f) / 10f;
        stoneGider = Mathf.Round(greenBoxCount * 0.004f * 10f) / 10f;

        // Güncellenmiş değerleri ekrana yazdır
        updateStats();
    }

    private IEnumerator UpdateResourcesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            // Deduct resources
            DeductResource(ref food, foodGider);
            DeductResource(ref water, waterGider);
            DeductResource(ref wood, woodGider);
            DeductResource(ref stone, stoneGider);

            // Add incomes
            AddResource(ref food, foodGelir + (population * 5));
            AddResource(ref water, waterGelir + (population * 5));
            AddResource(ref wood, woodGelir + (population * 5));
            AddResource(ref stone, stoneGelir + (population * 5));

            updateStats();
        }
    }

    private IEnumerator CheckResourcesAndDecreasePlayerHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (food <= 0 || water <= 0 || wood <= 0 || stone <= 0)
            {
                PlayerHealth.Instance.TakeDamage(1);
            }
        }
    }

    private void DeductResource(ref float resource, float gider)
    {
        resource = Mathf.Max(0, resource - gider);
    }

    private void AddResource(ref float resource, float gelir)
    {
        resource += gelir;
    }

    private IEnumerator BlinkText(TMP_Text text)
    {
        while (true)
        {
            text.color = text.color == Color.red ? Color.clear : Color.red;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void updateStats()
    {
        foodText.text = food.ToString("F1") + " (<color=green>+" + (foodGelir + (population * 5)).ToString("F1") + "</color> / <color=red>-" + foodGider.ToString("F1") + "</color>)";
        waterText.text = water.ToString("F1") + " (<color=green>+" + (waterGelir + (population * 5)).ToString("F1") + "</color> / <color=red>-" + waterGider.ToString("F1") + "</color>)";
        woodText.text = wood.ToString("F1") + " (<color=green>+" + (woodGelir + (population * 5)).ToString("F1") + "</color> / <color=red>-" + woodGider.ToString("F1") + "</color>)";
        stoneText.text = stone.ToString("F1") + " (<color=green>+" + (stoneGelir + (population * 5)).ToString("F1") + "</color> / <color=red>-" + stoneGider.ToString("F1") + "</color>)";
        populationText.text = population.ToString();

        // Yanıp sönme kontrolü
        if (!isBlinking)
        {
            if (food == 0) StartCoroutine(BlinkText(foodText));
            if (water == 0) StartCoroutine(BlinkText(waterText));
            if (wood == 0) StartCoroutine(BlinkText(woodText));
            if (stone == 0) StartCoroutine(BlinkText(stoneText));
            isBlinking = true;
        }
    }
}
