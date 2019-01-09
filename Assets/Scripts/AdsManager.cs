﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YumiMediationSDK.Api;
using YumiMediationSDK.Common;

public class AdsManager : MonoBehaviour {

	private YumiBannerView bannerView;
	private YumiInterstitialAd interstitialAd;
	private YumiRewardVideoAd rewardedAd;
	private YumiDebugCenter debugCenter;

	string gameVersion;
	bool bannerShowing;
	bool bannerLoaded;

#if UNITY_IOS
	private const string CHANNEL_ID = "iOS";

	private const string APP_ID = "khpunuj9";
	private const string BANNER_ID = "cveiflo7";
	private const string INTERSTITIAL_ID = "eksi16dm";
	private const string REWARDED_ID = "ajo0ji3j";
	private const string SPLASH_ID = "trlkbkn1";
#elif UNITY_ANDROID
	private const string CHANNEL_ID = "Google";

	private const string APP_ID = "8rpuontc";
	private const string BANNER_ID = "n44uwwn7";
	private const string INTERSTITIAL_ID = "1kx5ljxb";
	private const string REWARDED_ID = "fstbpmm8";
	private const string SPLASH_ID = "btrda6bv";
#else
	private const string CHANNEL_ID = "error";

	private const string APP_ID = "error";
	private const string BANNER_ID = "error";
	private const string INTERSTITIAL_ID = "error";
	private const string REWARDED_ID = "error";
	private const string SPLASH_ID = "error";
#endif


	// Use this for initialization
	void Start () {
		bannerShowing = false;
		bannerLoaded = false;
		gameVersion = Application.version;

		//Init banner
		this.bannerView = new YumiBannerView(BANNER_ID, CHANNEL_ID, gameVersion, YumiAdPosition.Bottom);

		this.bannerView.OnAdLoaded += this.HandleBannerLoaded;
		this.bannerView.OnAdFailedToLoad += this.HandleBannerFailedToLoad;
		this.bannerView.OnAdClick += this.HandleBannerClicked;


		//Request first ads
		this.bannerView.LoadAd(true);
		RequestInterstitial();
		RequestRewarded();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenTestSuite() {
		if (this.debugCenter == null){
            this.debugCenter = new YumiDebugCenter();
        }

        this.debugCenter.PresentYumiMediationDebugCenter(BANNER_ID, INTERSTITIAL_ID, REWARDED_ID, CHANNEL_ID, gameVersion);
	}

	public void RequestInterstitial() {
		Debug.Log("[NCTEST] REQUESTING INTERSTITIAL AD");

		if (this.interstitialAd != null) {
			this.interstitialAd.DestroyInterstitial();
			this.interstitialAd = null;
		}

		this.interstitialAd = new YumiInterstitialAd(INTERSTITIAL_ID, CHANNEL_ID, gameVersion);
		this.interstitialAd.OnAdLoaded += HandleInterstitialAdLoaded;
    	this.interstitialAd.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
    	this.interstitialAd.OnAdClicked += HandleInterstitialAdClicked;
    	this.interstitialAd.OnAdClosed += HandleInterstitialAdClosed;
	}

	public void ShowVideo() {
		if (this.interstitialAd != null && this.interstitialAd.IsInterstitialReady()) {
			this.interstitialAd.ShowInterstitial();
		} else {
			Debug.Log("[NCTEST] NO INTERSTITIAL AD AVAILABLE - REQUESTING NEW");
			RequestInterstitial();
		}
	}

	public void RequestRewarded() {
		Debug.Log("[NCTEST] REQUESTING REWARDED AD");
		if (this.rewardedAd != null) {
			this.rewardedAd.DestroyRewardVideo();
		}

		this.rewardedAd = new YumiRewardVideoAd();
		this.rewardedAd.OnAdOpening += HandleRewardVideoAdOpened;
		this.rewardedAd.OnAdStartPlaying += HandleRewardVideoAdStartPlaying;
		this.rewardedAd.OnAdRewarded += HandleRewardVideoAdReward;
		this.rewardedAd.OnAdClosed += HandleRewardVideoAdClosed;
		this.rewardedAd.LoadRewardVideoAd(REWARDED_ID, CHANNEL_ID, gameVersion);
	}

	public void ShowRewarded() {
		if (this.rewardedAd != null && this.rewardedAd.IsRewardVideoReady()) {
			this.rewardedAd.PlayRewardVideo();
		} else {
			Debug.Log("[NCTEST] NO REWARDED AD AVAILABLE - REQUESTING NEW");
			RequestRewarded();
		}
	}

	public void ToggleBanner() {
		if (!bannerShowing && !bannerLoaded) {
			this.bannerView.LoadAd(true);
		} else if (!bannerShowing) {
			this.bannerView.Show();
			bannerShowing = true;
		} else {
			this.bannerView.Hide();
			bannerShowing = false;
		}
	}

	//CALLBACKS
	public void HandleBannerLoaded( object sender, EventArgs args )
	{
		Debug.Log("[NCTEST] HandleBannerLoaded event received" );
		bannerLoaded = true;
	}

	public void HandleBannerFailedToLoad( object sender, YumiAdFailedToLoadEventArgs args )
	{
		Debug.Log("[NCTEST] HandleBannerFailedToLoad event received with message: " + args.Message );
	}

	public void HandleBannerClicked( object sender, EventArgs args )
	{
		Debug.Log("[NCTEST] Handle Banner Clicked" );
	}

	public void HandleInterstitialAdLoaded(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleInterstitialAdLoaded event received");
	}
	public void HandleInterstitialAdFailedToLoad(object sender, YumiAdFailedToLoadEventArgs args) 
	{
		Debug.Log("[NCTEST] HandleInterstitialAdFailedToLoad event received with message: " + args.Message);
	}
	public void HandleInterstitialAdClicked(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleInterstitialAdClicked Clicked");
	}
	public void HandleInterstitialAdClosed(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleInterstitialAdClosed Ad closed");
	}

	public void HandleRewardVideoAdOpened(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleRewardVideoAdOpened event opened");
	}
	public void HandleRewardVideoAdStartPlaying(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleRewardVideoAdStartPlaying event start playing ");
	}
	public void HandleRewardVideoAdReward(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleRewardVideoAdReward reward");
	}
	public void HandleRewardVideoAdClosed(object sender, EventArgs args) 
	{
		Debug.Log("[NCTEST] HandleRewardVideoAdClosed Ad closed");
	}

}
