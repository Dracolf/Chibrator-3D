using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [Header("API")]
    [SerializeField]
    private ChibratorApiClient apiClient;

    [Header("Current Player")]
    [SerializeField]
    private string playerPseudoPrefsKey = "pseudo";

    [SerializeField]
    private string fallbackPseudo = "Joueur";

    [Header("Top 5 Texts")]
    [SerializeField]
    private TextMeshProUGUI[] topPlayerTexts;

    [Header("Top 5 Medals")]
    [SerializeField]
    private Image[] topMedalImages;

    [Header("Top 5 Rank Numbers")]
    [SerializeField]
    private TextMeshProUGUI[] topRankTexts;

    [Header("Current Player UI")]
    [SerializeField]
    private TextMeshProUGUI currentPlayerText;

    [SerializeField]
    private Image currentPlayerMedalImage;

    [SerializeField]
    private TextMeshProUGUI currentPlayerRankText;

    [Header("Medal Sprites")]
    [SerializeField]
    private Sprite goldMedalSprite;

    [SerializeField]
    private Sprite silverMedalSprite;

    [SerializeField]
    private Sprite bronzeMedalSprite;

    private string currentPseudo;

    private void Start()
    {
        RefreshLeaderboard();
    }

    public void RefreshLeaderboard()
    {
        if (apiClient == null)
        {
            apiClient = FindAnyObjectByType<ChibratorApiClient>();
        }

        if (apiClient == null)
        {
            Debug.LogError("Aucun ChibratorApiClient trouvé dans la scène.");
            return;
        }

        ClearTop5();
        ClearCurrentPlayer();

        apiClient.GetLeaderboard(OnLeaderboardReceived);

        currentPseudo = GetCurrentPseudo();

        if (!string.IsNullOrWhiteSpace(currentPseudo))
        {
            apiClient.GetPlayerStats(currentPseudo, OnCurrentPlayerReceived);
        }
        else
        {
            SetPlaceholderCurrentPlayer();
        }
    }

    private string GetCurrentPseudo()
    {
        string pseudo = PlayerPrefs.GetString(playerPseudoPrefsKey, "");

        if (string.IsNullOrWhiteSpace(pseudo))
        {
            pseudo = fallbackPseudo;
        }

        return pseudo;
    }

    private void OnLeaderboardReceived(bool success, string response)
    {
        if (!success)
        {
            Debug.LogError("Erreur récupération leaderboard : " + response);
            return;
        }

        LeaderboardListResponse leaderboardResponse = JsonUtility.FromJson<LeaderboardListResponse>(response);

        if (leaderboardResponse == null || leaderboardResponse.data == null)
        {
            Debug.LogError("Réponse leaderboard invalide : " + response);
            return;
        }

        int count = Mathf.Min(leaderboardResponse.data.Length, topPlayerTexts.Length);

        for (int i = 0; i < count; i++)
        {
            LeaderboardPlayer player = leaderboardResponse.data[i];

            int rank = GetValidRank(player, i);

            SetPlayerRow(
                text: topPlayerTexts[i],
                medalImage: GetArrayElement(topMedalImages, i),
                rankText: GetArrayElement(topRankTexts, i),
                player: player,
                rank: rank
            );
        }
    }

    private void OnCurrentPlayerReceived(bool success, string response)
    {
        if (!success)
        {
            Debug.LogWarning("Joueur actuel introuvable, affichage par défaut : " + response);
            SetPlaceholderCurrentPlayer();
            return;
        }

        LeaderboardPlayerResponse playerResponse = JsonUtility.FromJson<LeaderboardPlayerResponse>(response);

        if (playerResponse == null || playerResponse.data == null)
        {
            Debug.LogWarning("Réponse joueur invalide, affichage par défaut : " + response);
            SetPlaceholderCurrentPlayer();
            return;
        }

        LeaderboardPlayer player = playerResponse.data;

        SetPlayerRow(
            text: currentPlayerText,
            medalImage: currentPlayerMedalImage,
            rankText: currentPlayerRankText,
            player: player,
            rank: player.rank
        );
    }

    private void SetPlayerRow(
        TextMeshProUGUI text,
        Image medalImage,
        TextMeshProUGUI rankText,
        LeaderboardPlayer player,
        int rank
    )
    {
        if (text != null)
        {
            text.gameObject.SetActive(true);
            text.text = $"{player.pseudo} ({player.score})";
        }

        Sprite medalSprite = GetMedalSprite(rank);

        if (medalImage != null)
        {
            bool hasMedal = medalSprite != null;

            medalImage.gameObject.SetActive(hasMedal);

            if (hasMedal)
            {
                medalImage.sprite = medalSprite;
            }
        }

        if (rankText != null)
        {
            bool shouldShowRankNumber = medalSprite == null;

            rankText.gameObject.SetActive(shouldShowRankNumber);
            rankText.text = shouldShowRankNumber ? $"{rank}." : "";
        }
    }

    private void SetPlaceholderCurrentPlayer()
    {
        if (currentPlayerText != null)
        {
            currentPlayerText.gameObject.SetActive(true);
            currentPlayerText.text = $"{currentPseudo} (0)";
        }

        if (currentPlayerMedalImage != null)
        {
            currentPlayerMedalImage.gameObject.SetActive(false);
        }

        if (currentPlayerRankText != null)
        {
            currentPlayerRankText.gameObject.SetActive(true);
            currentPlayerRankText.text = "?.";
        }
    }

    private Sprite GetMedalSprite(int rank)
    {
        if (rank == 1)
        {
            return goldMedalSprite;
        }

        if (rank == 2)
        {
            return silverMedalSprite;
        }

        if (rank == 3)
        {
            return bronzeMedalSprite;
        }

        return null;
    }

    private int GetValidRank(LeaderboardPlayer player, int index)
    {
        if (player.rank > 0)
        {
            return player.rank;
        }

        return index + 1;
    }

    private void ClearTop5()
    {
        for (int i = 0; i < topPlayerTexts.Length; i++)
        {
            if (topPlayerTexts[i] != null)
            {
                topPlayerTexts[i].gameObject.SetActive(true);
                topPlayerTexts[i].text = "? (?)";
            }

            Image medalImage = GetArrayElement(topMedalImages, i);

            if (medalImage != null)
            {
                medalImage.gameObject.SetActive(false);
            }

            TextMeshProUGUI rankText = GetArrayElement(topRankTexts, i);

            if (rankText != null)
            {
                rankText.gameObject.SetActive(true);
                rankText.text = "?.";
            }
        }
    }

    private void ClearCurrentPlayer()
    {
        if (currentPlayerText != null)
        {
            currentPlayerText.text = "";
            currentPlayerText.gameObject.SetActive(false);
        }

        if (currentPlayerMedalImage != null)
        {
            currentPlayerMedalImage.gameObject.SetActive(false);
        }

        if (currentPlayerRankText != null)
        {
            currentPlayerRankText.text = "";
            currentPlayerRankText.gameObject.SetActive(false);
        }
    }

    private T GetArrayElement<T>(T[] array, int index) where T : class
    {
        if (array == null || index < 0 || index >= array.Length)
        {
            return null;
        }

        return array[index];
    }

    [Serializable]
    private class LeaderboardListResponse
    {
        public int status_code;
        public string status_message;
        public LeaderboardPlayer[] data;
    }

    [Serializable]
    private class LeaderboardPlayerResponse
    {
        public int status_code;
        public string status_message;
        public LeaderboardPlayer data;
    }

    [Serializable]
    private class LeaderboardPlayer
    {
        public string pseudo;
        public int score;
        public string recordDate;
        public int rank;
    }
}