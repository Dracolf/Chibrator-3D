using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class ChibratorApiClient : MonoBehaviour
{
    [Header("API URLs")]
    [SerializeField]
    private string authUrl = "https://chibratorapi.alwaysdata.net/auth/";

    [SerializeField]
    private string leaderboardUrl = "https://chibratorapi.alwaysdata.net/leaderboard";

    [Header("Auth Credentials")]
    [SerializeField]
    private string login = "TON_LOGIN_ICI";

    [SerializeField]
    private string password = "TON_MDP_ICI";

    private string jwtToken;

    // ------------------------------------------------------------
    // PUBLIC METHODS
    // ------------------------------------------------------------

    public void RequestJwt(Action<bool, string> onComplete = null)
    {
        StartCoroutine(RequestJwtCoroutine(onComplete));
    }

    public void GetLeaderboard(Action<bool, string> onComplete)
    {
        StartCoroutine(AuthenticatedRequest(
            requestFactory: () => UnityWebRequest.Get(leaderboardUrl),
            onComplete: onComplete
        ));
    }

    public void GetPlayerStats(string pseudo, Action<bool, string> onComplete)
    {
        string url = $"{leaderboardUrl}/{UnityWebRequest.EscapeURL(pseudo)}";

        StartCoroutine(AuthenticatedRequest(
            requestFactory: () => UnityWebRequest.Get(url),
            onComplete: onComplete
        ));
    }

    public void AddOrUpdatePlayerScore(string pseudo, int score, Action<bool, string> onComplete = null)
    {
        StartCoroutine(AddOrUpdatePlayerScoreCoroutine(pseudo, score, onComplete));
    }

    // ------------------------------------------------------------
    // AUTH
    // ------------------------------------------------------------

    private IEnumerator RequestJwtCoroutine(Action<bool, string> onComplete)
    {
        AuthRequest body = new AuthRequest
        {
            login = login,
            password = password
        };

        string jsonBody = JsonUtility.ToJson(body);

        using UnityWebRequest request = CreateJsonRequest(authUrl, "POST", jsonBody);

        yield return request.SendWebRequest();

        string responseText = request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur auth : " + request.error);
            Debug.LogError("Réponse API : " + responseText);

            onComplete?.Invoke(false, responseText);
            yield break;
        }

        string extractedToken = ExtractJwtFromResponse(responseText);

        if (string.IsNullOrWhiteSpace(extractedToken))
        {
            Debug.LogError("Impossible de trouver le JWT dans la réponse auth : " + responseText);

            onComplete?.Invoke(false, responseText);
            yield break;
        }

        jwtToken = extractedToken;

        Debug.Log("JWT récupéré avec succès");

        onComplete?.Invoke(true, responseText);
    }

    // ------------------------------------------------------------
    // LEADERBOARD
    // ------------------------------------------------------------

    private IEnumerator AddOrUpdatePlayerScoreCoroutine(string pseudo, int score, Action<bool, string> onComplete)
    {
        yield return EnsureJwtToken();

        PlayerScorePostRequest postBody = new PlayerScorePostRequest
        {
            pseudo = pseudo,
            score = score
        };

        string postJson = JsonUtility.ToJson(postBody);

        using UnityWebRequest postRequest = CreateJsonRequest(leaderboardUrl, "POST", postJson);
        AddAuthHeader(postRequest);

        yield return postRequest.SendWebRequest();

        string postResponseText = postRequest.downloadHandler.text;

        if (postRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score ajouté au leaderboard : " + postResponseText);
            onComplete?.Invoke(true, postResponseText);
            yield break;
        }

        if (ShouldFallbackToPut(postRequest.responseCode, postResponseText))
        {
            Debug.Log("Pseudo déjà utilisé, passage en PUT pour mettre à jour le score.");

            yield return UpdatePlayerScoreCoroutine(pseudo, score, onComplete);
            yield break;
        }

        if (IsTokenInvalidResponse(postRequest.responseCode, postResponseText))
        {
            Debug.LogWarning("JWT expiré ou invalide, nouvelle auth puis retry POST.");

            jwtToken = null;

            yield return EnsureJwtToken();

            using UnityWebRequest retryPostRequest = CreateJsonRequest(leaderboardUrl, "POST", postJson);
            AddAuthHeader(retryPostRequest);

            yield return retryPostRequest.SendWebRequest();

            string retryResponseText = retryPostRequest.downloadHandler.text;

            if (retryPostRequest.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, retryResponseText);
                yield break;
            }

            if (ShouldFallbackToPut(retryPostRequest.responseCode, retryResponseText))
            {
                yield return UpdatePlayerScoreCoroutine(pseudo, score, onComplete);
                yield break;
            }

            Debug.LogError("Erreur retry POST leaderboard : " + retryPostRequest.error);
            Debug.LogError("Réponse API : " + retryResponseText);

            onComplete?.Invoke(false, retryResponseText);
            yield break;
        }

        Debug.LogError("Erreur POST leaderboard : " + postRequest.error);
        Debug.LogError("Réponse API : " + postResponseText);

        onComplete?.Invoke(false, postResponseText);
    }

    private IEnumerator UpdatePlayerScoreCoroutine(string pseudo, int score, Action<bool, string> onComplete)
    {
        yield return EnsureJwtToken();

        string url = $"{leaderboardUrl}/{UnityWebRequest.EscapeURL(pseudo)}";

        PlayerScorePutRequest putBody = new PlayerScorePutRequest
        {
            score = score
        };

        string putJson = JsonUtility.ToJson(putBody);

        using UnityWebRequest putRequest = CreateJsonRequest(url, "PUT", putJson);
        AddAuthHeader(putRequest);

        yield return putRequest.SendWebRequest();

        string responseText = putRequest.downloadHandler.text;

        if (putRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Score mis à jour : " + responseText);
            onComplete?.Invoke(true, responseText);
            yield break;
        }

        if (IsTokenInvalidResponse(putRequest.responseCode, responseText))
        {
            Debug.LogWarning("JWT expiré ou invalide, nouvelle auth puis retry PUT.");

            jwtToken = null;

            yield return EnsureJwtToken();

            using UnityWebRequest retryPutRequest = CreateJsonRequest(url, "PUT", putJson);
            AddAuthHeader(retryPutRequest);

            yield return retryPutRequest.SendWebRequest();

            string retryResponseText = retryPutRequest.downloadHandler.text;

            if (retryPutRequest.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, retryResponseText);
                yield break;
            }

            Debug.LogError("Erreur retry PUT leaderboard : " + retryPutRequest.error);
            Debug.LogError("Réponse API : " + retryResponseText);

            onComplete?.Invoke(false, retryResponseText);
            yield break;
        }

        Debug.LogError("Erreur PUT leaderboard : " + putRequest.error);
        Debug.LogError("Réponse API : " + responseText);

        onComplete?.Invoke(false, responseText);
    }

    // ------------------------------------------------------------
    // AUTHENTICATED REQUEST HELPER
    // ------------------------------------------------------------

    private IEnumerator AuthenticatedRequest(Func<UnityWebRequest> requestFactory, Action<bool, string> onComplete)
    {
        yield return EnsureJwtToken();

        using UnityWebRequest request = requestFactory();
        AddAuthHeader(request);

        yield return request.SendWebRequest();

        string responseText = request.downloadHandler.text;

        if (request.result == UnityWebRequest.Result.Success)
        {
            onComplete?.Invoke(true, responseText);
            yield break;
        }

        if (IsTokenInvalidResponse(request.responseCode, responseText))
        {
            Debug.LogWarning("JWT expiré ou invalide, nouvelle auth puis retry.");

            jwtToken = null;

            yield return EnsureJwtToken();

            using UnityWebRequest retryRequest = requestFactory();
            AddAuthHeader(retryRequest);

            yield return retryRequest.SendWebRequest();

            string retryResponseText = retryRequest.downloadHandler.text;

            if (retryRequest.result == UnityWebRequest.Result.Success)
            {
                onComplete?.Invoke(true, retryResponseText);
                yield break;
            }

            Debug.LogError("Erreur retry requête authentifiée : " + retryRequest.error);
            Debug.LogError("Réponse API : " + retryResponseText);

            onComplete?.Invoke(false, retryResponseText);
            yield break;
        }

        Debug.LogError("Erreur requête authentifiée : " + request.error);
        Debug.LogError("Réponse API : " + responseText);

        onComplete?.Invoke(false, responseText);
    }

    private IEnumerator EnsureJwtToken()
    {
        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            yield break;
        }

        bool authSuccess = false;

        yield return RequestJwtCoroutine((success, response) =>
        {
            authSuccess = success;
        });

        if (!authSuccess)
        {
            Debug.LogError("Impossible de récupérer un JWT.");
        }
    }

    // ------------------------------------------------------------
    // REQUEST HELPERS
    // ------------------------------------------------------------

    private UnityWebRequest CreateJsonRequest(string url, string method, string jsonBody)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, method);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        return request;
    }

    private void AddAuthHeader(UnityWebRequest request)
    {
        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + jwtToken);
        }
    }

    private bool ShouldFallbackToPut(long responseCode, string responseText)
    {
        if (responseCode != 400)
        {
            return false;
        }

        ApiErrorResponse errorResponse = JsonUtility.FromJson<ApiErrorResponse>(responseText);

        if (errorResponse == null)
        {
            return false;
        }

        return errorResponse.status_code == 400 &&
               !string.IsNullOrWhiteSpace(errorResponse.status_message) &&
               errorResponse.status_message.Contains("déjà utilisé");
    }

    private bool IsTokenInvalidResponse(long responseCode, string responseText)
    {
        if (responseCode == 401)
        {
            return true;
        }

        if (responseCode != 403)
        {
            return false;
        }

        ApiErrorResponse errorResponse = JsonUtility.FromJson<ApiErrorResponse>(responseText);

        if (errorResponse == null)
        {
            return false;
        }

        return errorResponse.status_code == 403 &&
               !string.IsNullOrWhiteSpace(errorResponse.status_message) &&
               errorResponse.status_message.Contains("Token invalide");
    }

    private string ExtractJwtFromResponse(string responseText)
    {
        AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(responseText);

        if (authResponse != null)
        {
            if (!string.IsNullOrWhiteSpace(authResponse.token))
            {
                return authResponse.token;
            }

            if (!string.IsNullOrWhiteSpace(authResponse.jwt))
            {
                return authResponse.jwt;
            }

            if (!string.IsNullOrWhiteSpace(authResponse.access_token))
            {
                return authResponse.access_token;
            }

            if (authResponse.data != null)
            {
                if (!string.IsNullOrWhiteSpace(authResponse.data.token))
                {
                    return authResponse.data.token;
                }

                if (!string.IsNullOrWhiteSpace(authResponse.data.jwt))
                {
                    return authResponse.data.jwt;
                }

                if (!string.IsNullOrWhiteSpace(authResponse.data.access_token))
                {
                    return authResponse.data.access_token;
                }
            }
        }

        Match match = Regex.Match(
            responseText,
            "\"(?:token|jwt|access_token)\"\\s*:\\s*\"([^\"]+)\""
        );

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
    }

    // ------------------------------------------------------------
    // JSON CLASSES
    // ------------------------------------------------------------

    [Serializable]
    private class AuthRequest
    {
        public string login;
        public string password;
    }

    [Serializable]
    private class AuthResponse
    {
        public int status_code;
        public string status_message;
        public AuthData data;

        public string token;
        public string jwt;
        public string access_token;
    }

    [Serializable]
    private class AuthData
    {
        public string token;
        public string jwt;
        public string access_token;
    }

    [Serializable]
    private class PlayerScorePostRequest
    {
        public string pseudo;
        public int score;
    }

    [Serializable]
    private class PlayerScorePutRequest
    {
        public int score;
    }

    [Serializable]
    private class ApiErrorResponse
    {
        public int status_code;
        public string status_message;
        public string data;
    }
}