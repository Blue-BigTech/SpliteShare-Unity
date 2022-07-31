using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Net;

public class Main : MonoBehaviour
{
    int m_nUniverse = 1;
    long m_nTotalBtc = 0;
    long m_nMiningPool = 1000;
    float m_fAbleFunds = 0.0f;
    long m_nUnsold = 0;
    float m_fDemand = 100.0f;
    int m_nLevel = 1;
    float m_fMarketingCost = 100.0f;
    float m_fPriceCoin = 0.08f;
    int m_nPoolCost = 15;
    int m_nCurrentMore = 1000;
    int m_nAutoMiner = 0;
    float m_fMinerCost = 5.0f;
    int m_nTrust = 0;
    long m_nTrustAtCoin = 1;
    int m_nProcessors = 0;
    int m_nMemory = 0;
    int m_nOps = 0;
    int m_nCreat = 0;
    long m_nCoinsPerSecond = 0;
    string m_sBTCPrice = "0.00";

    public Text m_tTotalBtc;
    public Text m_tBtcPrice;
    public Text m_tMiningPool;
    public Text m_tAbleFunds;
    public Text m_tUnsold;
    public Text m_tDemand;
    public Text m_tMarketingCost;
    public Text m_tPriceCoin;
    public Text m_tMarketingLevel;
    public Text m_tPoolCost;
    public Text m_tAutoMiner;
    public Text m_tMinerCost;
    public Text m_tTrust;
    public Text m_tTrustCoin;
    public Text m_tProcessors;
    public Text m_tMemory;
    public Text m_tOps;
    public Text m_tCreat;
    public Text m_tCoinsPerSecond;
    public Text[] m_Logos;

    float m_fTimer = 0.0f;
    float m_fBtcTimer = 0.0f;
    float m_fAutoBoost = 1.0f;
    List<string> m_ListLogs = new List<string>();
    List<long> m_ListTrustAt = new List<long>();

    // Start is called before the first frame update
    void Start()
    {
        m_ListLogs.Add(".");
        m_ListLogs.Add(".");
        m_ListLogs.Add(".");
        m_ListLogs.Add("Welcome to SPLIT SHARES!");
        ShowLogs();
        m_ListTrustAt.Add(1000);
        m_ListTrustAt.Add(1000);
        m_ListTrustAt.Add(2000);
    }

    void ShowLogs()
    {
        for (int i = 0; i < 4; i++)
            m_Logos[i].text = m_ListLogs[i];
    }

    void AddLog(string _log)
    {
        m_ListLogs.Add(_log);
        m_ListLogs.RemoveAt(0);
        ShowLogs();
    }

    void EventPerSecond()
    {
        SellBitcoin();
        AutoMining();
    }

    void GetBTCPrice() 
    {
        string uri = String.Format("https://blockchain.info/tobtc?currency=USD&value={0}", 1);

        WebClient client = new WebClient();
        client.UseDefaultCredentials = true;
        string data = client.DownloadString(uri);

        double result = Convert.ToDouble(data);
        double btcprice = Math.Round((1.0f / result), 2);
        m_sBTCPrice = btcprice.ToString();
        Debug.Log("BTC = " + m_sBTCPrice);
    }

    void AutoMining()
    {
        m_nCoinsPerSecond = (int)(m_fAutoBoost * (float)m_nAutoMiner);
        m_nTotalBtc += m_nCoinsPerSecond;
        m_nUnsold += m_nCoinsPerSecond;
        m_nMiningPool -= m_nCoinsPerSecond;
        if (m_nCoinsPerSecond == 0) return;
        
        int nCase = UnityEngine.Random.Range(0, 3);
        switch (nCase)
        {
            case 0:
                AddLog("New Bitcoin!");
                break;
            case 1:
                AddLog("Yeah New Bitcoin!");
                break;
            case 2:
                AddLog("Wow New Bitcoin!");
                break;
        }
    }

    void SellBitcoin()
    {
        if (m_nUnsold == 0) return;
        int n_SellCnt = (int)Math.Floor(m_nLevel * Math.Pow(m_fDemand/100.0f, 2.0f)) + m_nLevel + UnityEngine.Random.Range(0, 2);
        if (m_nUnsold < n_SellCnt)
            n_SellCnt = (int)m_nUnsold;
        m_nUnsold -= n_SellCnt;
        m_fAbleFunds += (float)n_SellCnt * m_fPriceCoin;
    }

    void AddTrust()
    {
        m_nTrustAtCoin = m_ListTrustAt[2];
        if (m_nTotalBtc >= m_nTrustAtCoin)
        {
            m_ListTrustAt[2] = m_ListTrustAt[0] + m_ListTrustAt[1];
            m_nTrustAtCoin = m_ListTrustAt[2];
            m_ListTrustAt[0] = m_ListTrustAt[1];
            m_ListTrustAt[1] = m_ListTrustAt[2];
            m_nTrust++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AddTrust();

        m_fTimer += Time.deltaTime;
        m_fBtcTimer += Time.deltaTime;
        if (m_fTimer > 1.0f)
        {
            m_fTimer = 0.0f;
            EventPerSecond();
        }
        if (m_fBtcTimer > 0.5f)
        {
            m_fBtcTimer = 0.0f;
            GetBTCPrice();
        }
        m_tTotalBtc.text = API_GetStringFromLong(m_nTotalBtc);
        m_tMiningPool.text = API_GetStringFromLong(m_nMiningPool) + " Mining Pool";
        m_tAbleFunds.text = "Available Funds : $" + API_GetStringFromFloat(m_fAbleFunds);
        m_tUnsold.text = "Unsold Bitcoin : " + API_GetStringFromLong(m_nUnsold);
        m_tDemand.text = "Public Demand : " + API_GetStringFromFloat(m_fDemand) + "%";
        m_tPriceCoin.text = "Price Per Coin : $" + API_GetStringFromFloat(m_fPriceCoin);
        m_tMarketingLevel.text = "Marketing level : " + API_GetStringFromLong(m_nLevel);
        m_tMarketingCost.text = "Cost : $" + API_GetStringFromFloat(m_fMarketingCost);
        m_tPoolCost.text = "Cost : $" + API_GetStringFromLong(m_nPoolCost, true);
        m_tAutoMiner.text = API_GetStringFromLong(m_nAutoMiner);
        m_tMinerCost.text = "Cost : $" + API_GetStringFromFloat(m_fMinerCost);
        m_tTrust.text = "Trust : " + API_GetStringFromLong(m_nTrust);
        m_tTrustCoin.text = "+1 Trust at : " + API_GetStringFromLong(m_nTrustAtCoin) + " coins";
        m_tProcessors.text = API_GetStringFromLong(m_nProcessors);
        m_tMemory.text = API_GetStringFromLong(m_nMemory);
        m_tOps.text = "Operations : " + API_GetStringFromLong(m_nOps) + "/" + API_GetStringFromLong(m_nMemory * 1000);
        m_tCreat.text = "Creativity : " + API_GetStringFromLong(m_nCreat);
        m_tCoinsPerSecond.text = "Coins per second : " + API_GetStringFromLong(m_nCoinsPerSecond);
        m_tBtcPrice.text = "$" + m_sBTCPrice;
    }

    public void MiningManual()
    {
        if (m_nAutoMiner > 0)
            m_nCoinsPerSecond++;
        else
            m_nCoinsPerSecond = 1;
        m_nTotalBtc += 1;
        m_nMiningPool -= 1;
        m_nUnsold += 1;
        int nCase = UnityEngine.Random.Range(0, 3);
        switch (nCase)
        {
            case 0:
                AddLog("New Bitcoin!");
                break;
            case 1:
                AddLog("Yeah New Bitcoin!");
                break;
            case 2:
                AddLog("Wow New Bitcoin!");
                break;
        }
    }

    public void LevelUp()
    {
        if (m_fAbleFunds < m_fMarketingCost) return;
        m_fAbleFunds -= m_fMarketingCost;
        m_nLevel++;
        m_fMarketingCost *= 2.0f;
        AddLog("Marketing Level UP!");
    }

    public void LowerPrice()
    {
        if (m_fPriceCoin > 0.02)
            m_fPriceCoin -= 0.01f;
        CalcDemand();
    }

    public void RaisePrice()
    {
        m_fPriceCoin += 0.01f;
        CalcDemand();
    }

    public void CalcDemand()
    {
        m_fDemand = (1.0f + 0.1f * ((float)m_nUniverse - 1.0f)) * (float)(Math.Pow(1.1f, (double)m_nLevel)) * 90.91f * (0.08f / m_fPriceCoin);
    }

    public void BuyMore()
    {
        if (m_fAbleFunds < m_nPoolCost) return;
        m_fAbleFunds -= m_nPoolCost;
        m_nMiningPool += m_nCurrentMore;
    }

    public void AddMiner()
    {
        if (m_fAbleFunds < m_fMinerCost) return;
        m_fAbleFunds -= m_fMinerCost;
        m_nAutoMiner++;
        m_fMinerCost = (float)Math.Pow(1.1f, (float)m_nAutoMiner) + 5.0f;
        AddLog("Add miner");
    }

    public void AddProcessor()
    {
        if (m_nTrust > m_nProcessors + m_nMemory)
            m_nProcessors++;
    }

    public void AddMemory()
    {
        if (m_nTrust > m_nProcessors + m_nMemory)
            m_nMemory++;
    }

    protected string API_GetStringFromLong(long _num, bool bf = false)
    {
        string res = "";
        res = _num.ToString();
        //add ,
        if (bf) res += ".00";
        return res;
    }

    protected string API_GetStringFromFloat(float _num)
    {
        string res = "";
        
        res = Math.Round(_num, 2).ToString();
        if (!res.Contains("."))
            res += ".00";
        return res;
    }
}
