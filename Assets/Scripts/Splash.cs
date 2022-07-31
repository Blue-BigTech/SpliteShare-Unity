using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    //bool m_bLoading = false;
    float m_fCoolDown = 3.0f;
    //float m_fTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        //m_fTimer += Time.deltaTime;
        //if(m_bLoading)
        //{
        //}
        m_fCoolDown -= Time.deltaTime;
        if(m_fCoolDown <= 0.0f)
            SceneManager.LoadScene("02_Main");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("02_Main");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            //m_ProgBar.GetComponent<Image>().fillAmount = asyncOperation.progress;
            //string str = "Loading progress: " + (asyncOperation.progress * 100) + "%";
            //Debug.Log(str);
            if (asyncOperation.progress >= 0.9f)
            {
                //    m_ProgBar.GetComponent<Image>().fillAmount = 1.0f;
                //    //m_Text.text = "Press the space bar to continue";
                //    str = "Press the space bar to continue";
                //    Debug.Log(str);
                //    m_Alert.SetActive(true);
                //    //Wait to you press the space key to activate the Scene
                //    if (Input.GetKeyDown(KeyCode.Space))
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
