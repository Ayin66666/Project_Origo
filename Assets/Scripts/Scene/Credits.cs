using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] private Text[] title;
    [SerializeField] private Text[] review;
    [SerializeField] private Text usedAsset;
    [SerializeField] private Text thankYou;
    public bool isTitleFadeIn;
    public bool isreviewFadeIn;
    public bool isreviewFadeOut;
    public bool isUsedAssetFadeIn;
    public bool isUsedAssetScroll;
    public bool isallCreditFadeOut;
    public bool isThankYouFadeIn;
    public bool isThankYouFadeOut;
    void Awake()
    {
        for (int i = 0; i < title.Length; i++)
        {
            title[i].color = new Color(255, 255, 255, 0);
        }
        for (int i = 0; i < review.Length; i++)
        {
            review[i].color = new Color(255, 255, 255, 0);
        }
        usedAsset.color = new Color(255, 255, 255, 0);
        thankYou.color = new Color(255, 255, 255, 0);

        StartCoroutine(nameof(TitleFadeIn));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TitleFadeIn()
    {
        float fadeCount = 0;
        isTitleFadeIn = true;

        while(fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            for (int i = 0; i < title.Length; i++)
            {
                title[i].color = new Color(255, 255, 255, fadeCount);
            }
        }

        isTitleFadeIn = false;
        StartCoroutine(nameof(ReviewFadeInOut));
        StartCoroutine(nameof(UsedAssetFadeIn));
    }

    IEnumerator ReviewFadeInOut()
    {
        // 1번째 후기 
        float fadeCount = 0;
        isreviewFadeIn = true;

        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[0].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeIn = false;

        yield return new WaitForSeconds(10f);

        isreviewFadeOut = true;
        fadeCount = 1;

        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[0].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeOut = false;

        // 2번째 후기
        isreviewFadeIn = true;
        fadeCount = 0;

        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[1].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeIn = false;

        yield return new WaitForSeconds(10f);

        isreviewFadeOut = true;
        fadeCount = 1;

        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[1].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeOut = false;

        // 3번째 후기
        isreviewFadeIn = true;
        fadeCount = 0;

        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[2].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeIn = false;

        yield return new WaitForSeconds(10f);

        isreviewFadeOut = true;
        fadeCount = 1;

        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            review[2].color = new Color(255, 255, 255, fadeCount);
        }

        isreviewFadeOut = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(nameof(AllCreditFadeOut));
    }

    IEnumerator UsedAssetFadeIn()
    {
        float fadeCount = 0;
        isUsedAssetFadeIn = true;

        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            usedAsset.color = new Color(255, 255, 255, fadeCount);
        }

        isUsedAssetFadeIn = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(nameof(UsedAssetScroll));
    }

    IEnumerator UsedAssetScroll()
    {
        float moveTime = 0;
        isUsedAssetScroll = true;

        while (moveTime < 20f)
        {
            moveTime += Time.deltaTime;
            usedAsset.transform.position += Vector3.up * 20 * Time.deltaTime;
            yield return null;
        }

        isUsedAssetScroll = false;
    }

    IEnumerator AllCreditFadeOut()
    {
        float fadeCount = 1;
        isallCreditFadeOut = true;

        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            for (int i = 0; i < title.Length; i++)
            {
                title[i].color = new Color(255, 255, 255, fadeCount);
            }
            usedAsset.color = new Color(255, 255, 255, fadeCount);
        }

        isallCreditFadeOut = false;
        StartCoroutine(nameof(ThankYouFadeInOut));
    }

    IEnumerator ThankYouFadeInOut()
    {
        float fadeCount = 0;
        isThankYouFadeIn = true;

        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            thankYou.color = new Color(255, 255, 255, fadeCount);
        }

        isThankYouFadeIn = false;

        yield return new WaitForSeconds(3f);
        isThankYouFadeOut = true;
        fadeCount = 1;

        while (fadeCount > 0)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            thankYou.color = new Color(255, 255, 255, fadeCount);
        }
        isThankYouFadeOut = false;

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            // 시작화면으로 전환
            yield return null;
        }
        SceneLorder.LoadScene("Start_Scene");
    }
}
