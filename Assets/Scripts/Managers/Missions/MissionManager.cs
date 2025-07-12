using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public bool canLevelUp = false;

    private List<Mission> quests; // Görevlerin listesi
    public int currentQuestIndex = 0; // Mevcut görev indeksi

    public TMP_Text missionText; // TMP text bileşeni
    public float fadeDuration = 1f; // Yazının silinme ve görünme süresi
    public Color completedColor = Color.green; // Görev tamamlandığında yazının rengi
    private Color originalColor; // Yazının orijinal rengi

    private void Awake()
    {
        Instance = this;
        quests = new List<Mission>
        {
            new Mission(1, "Collect 3 woods."),
            new Mission(2, "Collect 3 stones."),
            new Mission(3, "Reach to the target point"),
            new Mission(4, "Kill 3 Monsters (Right click for attack.)"),
            new Mission(5, "You heard a mysterious sound!"),
            new Mission(6, "There are some waiting to awaken beneath the ground."),

            new Mission(6, "On your journey to create a new world, everyone is counting on you. Be very careful!")

        };

        // Yazının orijinal rengini kaydet
        originalColor = missionText.color;

        // Mevcut görevi göster
        UpdateMissionText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CompleteCurrentQuest();
            ExecuteLevelUpOnGreenBoxes();
        }
        Debug.Log("Current Mission Index: " + currentQuestIndex); // Mevcut görev indeksini yazdır
    }

    public void nextMission()
    {
        CompleteCurrentQuest();
        ExecuteLevelUpOnGreenBoxes();

    }

    private void ExecuteLevelUpOnGreenBoxes()
    {
        for(int indexer=1; indexer<=2; indexer++)
        {
            GameObject[] greenBoxes = GameObject.FindGameObjectsWithTag("GreenBox");
            foreach (GameObject greenBox in greenBoxes)
            {
                GreenBox greenBoxScript = greenBox.GetComponent<GreenBox>();
                if (greenBoxScript != null)
                {
                    greenBoxScript.levelUp();
                }
            }
        
        }
    }

    private void CompleteCurrentQuest()
    {
        canLevelUp = true;

        if (currentQuestIndex < quests.Count)
        {
            StartCoroutine(CompleteQuestSequence());
        }

        currentQuestIndex++;
        if (currentQuestIndex >= quests.Count)
        {
            // Tüm görevler tamamlandı
            Debug.Log("Tüm görevler tamamlandı!");
            StartCoroutine(FadeText("Tüm görevler tamamlandı!"));
        }
    }

    private IEnumerator CompleteQuestSequence()
    {
        // Görevin tamamlandığını göster - Yeşil renk
        missionText.color = completedColor;
        yield return new WaitForSeconds(1f);

        // Üstü çizili yap
        missionText.fontStyle |= FontStyles.Strikethrough;
        yield return new WaitForSeconds(1f);

        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, 0f);

        // Yeni görevi göster
        if (currentQuestIndex < quests.Count)
        {
            missionText.text = quests[currentQuestIndex].Description;
            // Varsayılan rengi ve stilini geri yükle
            missionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            missionText.fontStyle = FontStyles.Normal;

            // Fade in
            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            missionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        }
    }

    private void UpdateMissionText()
    {
        if (currentQuestIndex < quests.Count)
        {
            StartCoroutine(FadeText(quests[currentQuestIndex].Description));
        }
    }

    private IEnumerator FadeText(string newText)
    {
        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, 0f);

        // Update text
        missionText.text = newText;

        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            missionText.color = new Color(missionText.color.r, missionText.color.g, missionText.color.b, Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        missionText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
}

// Görev sınıfı
[System.Serializable]
public class Mission
{
    public int ID { get; private set; }
    public string Description { get; private set; }

    public Mission(int id, string description)
    {
        ID = id;
        Description = description;
    }
}
