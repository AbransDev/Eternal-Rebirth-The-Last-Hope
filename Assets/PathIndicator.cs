using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathIndicator : MonoBehaviour
{
    public static PathIndicator Instance;
    private void Awake()
    {
        Instance = this;
    }
    public Transform target; // G�rev hedefi
    public GameObject dotPrefab; // Nokta prefab'�
    public float dotSpacing = 1.0f; // Nokta aral���
    public float stopDistance = 1.0f; // Nokta olu�turmay� durdurma mesafesi
    private List<GameObject> dots = new List<GameObject>();

    void Start()
    {
        if (target != null)
        {
            CreatePath();
        }
    }

    void Update()
    {
        if (target != null)
        {
            UpdatePath();
        }
    }

    void CreatePath()
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (target.position - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, target.position);
        int numberOfDots = Mathf.FloorToInt(distance / dotSpacing);

        for (int i = 0; i < numberOfDots; i++)
        {
            Vector3 dotPosition = startPosition + direction * (i * dotSpacing);
            GameObject dotInstance = Instantiate(dotPrefab, dotPosition, Quaternion.identity);
            dots.Add(dotInstance);
        }
    }

    void UpdatePath()
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = (target.position - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, target.position);

        // E�er hedefe yeterince yak�nsan�z, noktalar� olu�turmay� durdurun ve mevcut noktalar� temizleyin
        if (distance <= stopDistance)
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
            dots.Clear();
            return;
        }

        int numberOfDots = Mathf.FloorToInt(distance / dotSpacing);

        // Mevcut noktalar� g�ncelleme
        for (int i = 0; i < dots.Count; i++)
        {
            if (i < numberOfDots)
            {
                Vector3 dotPosition = startPosition + direction * (i * dotSpacing);
                dots[i].transform.position = dotPosition;
            }
            else
            {
                // Gereksiz noktalar� kald�rma
                Destroy(dots[i]);
                dots.RemoveAt(i);
                i--;
            }
        }

        // Yeni noktalar ekleme
        for (int i = dots.Count; i < numberOfDots; i++)
        {
            Vector3 dotPosition = startPosition + direction * (i * dotSpacing);
            GameObject dotInstance = Instantiate(dotPrefab, dotPosition, Quaternion.identity);
            dots.Add(dotInstance);
        }
    }
}