using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class ADSample : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener {
    
    [field: SerializeField] private string _androidGameId { get; set; }
    [field: SerializeField] private string _iOSGameId { get; set; }
    private string _gameId { get; set; }
    [field: SerializeField] private bool _testMode { get; set; }
    [field: SerializeField] private Text debugText { get; set; } // Para el debug en pantalla durante la build.

    private void Awake() {
#if UNITY_EDITOR
        _testMode = true;
#else
        _testMode = false;
#endif

        debugText.text = "El anuncio no se ha cargado.";
        if (Advertisement.isInitialized) {

            Debug.Log("Advertisement is Initialized");
            //LoadRewardedAd();
        }
        else {
            InitializeAds();
        }
    }
    public void InitializeAds() {
        //_gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(_androidGameId, _testMode, this);

    }

    public void OnInitializationComplete() {
        debugText.text = "El anuncio se ha cargado.";
        Debug.Log("Unity Ads initialization complete.");
        // LoadInerstitialAd();
        // LoadBannerAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) {
        debugText.text = $"Unity Ads Initialization Failed: {error} - {message}.";
    }

    public void LoadInerstitialAd() {

        //Esta funcion carga un Inertitial(una pantalla de anuncio) (no es video, no hay recompensa)
        Advertisement.Load("Interstitial_Android", this);
    }

    public void LoadRewardedAd() {
        //Esta funcion carga un video con recompensa
        Advertisement.Load("Rewarded_Android", this);
    }

    public void OnUnityAdsAdLoaded(string placementId) {
        Debug.Log("OnUnityAdsAdLoaded");
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) {
        Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}.");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) {
        Debug.Log("OnUnityAdsShowFailure");
    }

    public void OnUnityAdsShowStart(string placementId) {
        Debug.Log("OnUnityAdsShowStart");
        Time.timeScale = 0;
        Advertisement.Banner.Hide();
    }

    public void OnUnityAdsShowClick(string placementId) {
        Debug.Log("OnUnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) {
        Debug.Log("OnUnityAdsShowComplete " + showCompletionState);
        if (placementId.Equals("Rewarded_Android") && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState)) {
            //Esta condicion sirve para que cuando el anuncio se ha visto al completo recompensemos al jugador;
            Debug.Log("Recompensamos al jugador");
        }
        Time.timeScale = 1;
    }



    public void LoadBannerAd() {

        /*
         * Esta funcion carga un Banner
         */
        // Podemos determinar la posicion del banner cambiando BannerPosition..
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load("Banner_Android",
            new BannerLoadOptions {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            }
            );
    }

    public void LoadBannerAdUpdated() {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        BannerLoadOptions options = new BannerLoadOptions {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
        Advertisement.Banner.Load("Banner_Android", options);
    }

    void OnBannerLoaded() {
        //Advertisement.Banner.Show("Banner_Android");
        debugText.text = $"Banner Loaded.";
    }

    void OnBannerError(string message) {
        debugText.text = $"Banner Error: {message}";
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd() {
        LoadBannerAdUpdated();
        
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show("Banner_Android", options);
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    void ClearListeners() {
        // Clean up the listeners:
        /*_loadBannerButton.onClick.RemoveAllListeners();
        _showBannerButton.onClick.RemoveAllListeners();
        _hideBannerButton.onClick.RemoveAllListeners();*/
    }

    private void OnDisable() {
        ClearListeners();
    }

    void OnDestroy() {
        ClearListeners();
    }

}