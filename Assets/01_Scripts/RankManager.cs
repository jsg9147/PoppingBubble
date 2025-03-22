using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankManager : MonoBehaviour
{
    public RankingItem rankItemPrefab;
    public GameObject rankingPopup;

    public RankingItem myRankItem;

    public Transform rankContent;

    List<Rank> rankingList = new List<Rank>();

    async void Start()
    {
        await GPGSManager.Instance.LoadRankingData();
    }
    public void OpenRankingPopup()
    {
        if (rankingList.Count == 0)
        {
            GetRankData();
        }
        rankingPopup.SetActive(true);
    }
    public void CloseRankingPopup()
    {
        rankingPopup.SetActive(false);
    }

    void AddRankItem(Rank rank, int rankIndex)
    {
        RankingItem rankItem = Instantiate(rankItemPrefab, rankContent);
        rankItem.SetRankingText(rankIndex);
        rankItem.nameText.text = rank.name;
        rankItem.SetScore(rank.score);
    }

    void SetMyRank(Rank rank, int rankIndex)
    {
        myRankItem.SetRankingText(rankIndex);
        myRankItem.nameText.text = rank.name;
        myRankItem.SetScore(rank.score);
        myRankItem.gameObject.SetActive(true);
    }

    void GetRankData()
    {
        rankingList = GPGSManager.Instance.rankingList;
        for (int i = 0; i < rankingList.Count; i++)
        {
            AddRankItem(rankingList[i], i + 1);
            if (rankingList[i].uid == GPGSManager.Instance.Token)
            {
                SetMyRank(rankingList[i], i + 1);
            }
        }
    }
}

public class Rank
{
    public string name;
    public int score;
    public long timestamp;

    public string uid;

    public Rank(string name, int score)
    {
        this.name = name;
        this.score = score;
        this.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public Rank(string name, int score, string uid)
    {
        this.name = name;
        this.score = score;
        this.uid = uid;
    }
}
