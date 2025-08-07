using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Game ID y Ad Units")]
    [SerializeField] private string androidGameId = "5918466";
    [SerializeField] private string bannerAdUnitId = "Banner_Android";
    [SerializeField] private string interstitialAdUnitId = "Interstitial_Android";

    [Header("Modo de prueba")]
    [SerializeField] private bool testMode = true;

    private void Awake()
    {
        Advertisement.Initialize(androidGameId, testMode, this);
    }

    
    public void OnInitializationComplete()
    {
        Debug.Log("✅ Unity Ads initialized");

        
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                BannerLoadOptions options = new BannerLoadOptions
                {
                    loadCallback = OnBannerLoaded,
                    errorCallback = OnBannerError
                };

                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Load(bannerAdUnitId, options);
                Debug.Log("📡 Banner loading on Android UI thread.");
            }));
        }
#else
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load(bannerAdUnitId, options);
        Debug.Log("📡 Banner loading on Editor or non-Android platform.");
#endif

        
        Advertisement.Load(interstitialAdUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"❌ Unity Ads Init Error: {error} - {message}");
    }

    
    private void OnBannerLoaded()
    {
        Debug.Log("✅ Banner loaded. Now showing...");
        Advertisement.Banner.Show(bannerAdUnitId, new BannerOptions
        {
            showCallback = () => Debug.Log("🟢 Banner shown"),
            hideCallback = () => Debug.Log("🟡 Banner hidden"),
            clickCallback = () => Debug.Log("🔵 Banner clicked")
        });
    }

    private void OnBannerError(string message)
    {
        Debug.LogWarning($"❌ Banner error: {message}");
    }

    
    public void ShowInterstitialAd()
    {
        Advertisement.Show(interstitialAdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"✅ Interstitial ad loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"❌ Failed to load {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"🟩 Interstitial {placementId} completed with state: {showCompletionState}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"❌ Interstitial show failed: {placementId} - {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
