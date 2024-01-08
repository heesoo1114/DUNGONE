using System.Collections;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    private Transform barRectTransfom;

    private Coroutine scaleCoroutine = null;

    private float barValue = 1f;

    [SerializeField] private float scaleAnimaionSpeed = 0.75f;

    private void Awake()
    {
        barRectTransfom = transform.Find("Bar").GetComponent<Transform>();
    }

    public void SetValue(float health)
    {
        barValue = health * 0.01f;
        UpdateBarUI(barValue);
    }

    public void UpdateBarUI(float value)
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleCor(value, scaleAnimaionSpeed));
    }

    private IEnumerator ScaleCor(float targetScaleX, float animSpeed)
    {
        float moveTime = 0f;
        float timeValue = 0f;
        float endTimeValue = 1 / animSpeed;
        Vector3 targetScaleVector = new Vector3(targetScaleX, 1, 1);

        Debug.Log(targetScaleVector);

        while (true)
        {
            moveTime += Time.deltaTime;
            timeValue = moveTime / animSpeed;

            barRectTransfom.localScale = Vector3.Lerp(barRectTransfom.localScale, targetScaleVector, timeValue);

            if (timeValue > 0.9f)
            {
                break;
            }

            yield return null;
        }

        barRectTransfom.localScale = targetScaleVector;
        scaleCoroutine = null;
    }
}
