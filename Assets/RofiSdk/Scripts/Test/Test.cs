using System;
using System.Collections.Generic;
using RofiSdk;
using RofiSdk.Models;
using TinyMessenger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown endPointDropdown;
    [Space]
    [SerializeField] private Text _textField;
    private List<TinyMessageSubscriptionToken> _tokens;

    private string accessToken =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiIxNDZiN2VlNi00ODgxLTRmM2ItYWIyMi1kNmU5Y2I5N2ViM2UiLCJlbWFpbCI6IiIsIndhbGxldCI6IiIsIm5iZiI6MTY2OTc4NjgyMiwiZXhwIjoxNjY5ODIyODIyLCJpYXQiOjE2Njk3ODY4MjIsImlzcyI6ImlkLnJvZmkuZ2FtZXMiLCJhdWQiOiJhcGkucm9maS5nYW1lcyJ9.eXZ-THGv0x-ZYji31t3RZdseG6Q5ocPffAXYP3HZ4g0";

    private void Start()
    {
        var options = new List<string>();
        options.Add("PROD");
        options.Add("TEST");
        options.Add("DEMO");
        options.Add("DEMO_v1");
        endPointDropdown.ClearOptions();
        endPointDropdown.AddOptions(options);
        endPointDropdown.onValueChanged.AddListener(arg0 =>
        {
            Debug.Log("On Change to " + arg0);
            RofiSdkHelper.Instance.NativeBridge.SetMode(arg0);
        });
        
        // RofiSdkHelper.Instance.NativeBridge.WarmUp();
        _tokens = new List<TinyMessageSubscriptionToken>();
        RofiSdkHelper.Instance.NativeBridge.SetDebugMode(true);
        // RofiSdkHelper.Instance.NativeBridge.SetMode((int) EvaiModes.DEMO_V1);

        _tokens.Add(RofiSdkHelper.Instance.MessageHub.Subscribe<RofiSdkCallbackMessage>(OnGetCallBackMessage));
        _tokens.Add(RofiSdkHelper.Instance.MessageHub.Subscribe<AdsCallback>(OnAdsCallback));
    }

    private void OnAdsCallback(AdsCallback callbackData)
    {
        Debug.Log("OnAdsCallback: " + callbackData.requestCode);
    }

    private void OnDestroy()
    {
        foreach (var token in _tokens)
        {
            RofiSdkHelper.Instance.MessageHub.Unsubscribe<RofiSdkCallbackMessage>(token);
        }
    }

    private void OnGetCallBackMessage(RofiSdkCallbackMessage message)
    {
        _textField.text = (message.Content);
    }

    public void IsRewardVideoAvailable()
    {
        var available = RofiSdkHelper.Instance.NativeBridge.IsVideoRewardAvailable();
        Debug.Log("IsRewardVideoAvailable " + available);
    }

    public void ShowAds()
    {
        RofiSdkHelper.Instance.NativeBridge.ShowAds();
    }

    public void LogEvent1()
    {
        // RofiSdkHelper.Instance.NativeBridge.LogEvent("Event_Name_1", new Dictionary<string, string>()
        // {
        //     {"click_to_button","btnLogEvent"} 
        // });
        RofiSdkHelper.Instance.NativeBridge.GetUserInfo(
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiIxNDZiN2VlNi00ODgxLTRmM2ItYWIyMi1kNmU5Y2I5N2ViM2UiLCJlbWFpbCI6IiIsIndhbGxldCI6IiIsIm5iZiI6MTY2OTc4NjgyMiwiZXhwIjoxNjY5ODIyODIyLCJpYXQiOjE2Njk3ODY4MjIsImlzcyI6ImlkLnJvZmkuZ2FtZXMiLCJhdWQiOiJhcGkucm9maS5nYW1lcyJ9.eXZ-THGv0x-ZYji31t3RZdseG6Q5ocPffAXYP3HZ4g0");
    }

    public void LogEvent2()
    {
        RofiSdkHelper.Instance.NativeBridge.LogEvent("Event_Name_2", new Dictionary<string, string>()
        {
            {"test_param_name", "test_param_value"}
        });
    }

    public void ShowLogin()
    {
        RofiSdkHelper.Instance.NativeBridge.OpenLoginScene();
    }

    public void CallRefCheckin()
    {
        accessToken = RofiSdkHelper.Instance.NativeBridge.GetCurrentAccessToken();
        RofiSdkHelper.Instance.NativeBridge.RefCheckIn(accessToken,
            RofiSdkHelper.Instance.NativeBridge.GetRefCodeCached());
    }

    public void GetRefCode()
    {
        _textField.text = string.Format("Ref code: {0}", RofiSdkHelper.Instance.NativeBridge.GetRefCodeCached());
    }

    public void JoinCampaign()
    {
        accessToken = RofiSdkHelper.Instance.NativeBridge.GetCurrentAccessToken();
        RofiSdkHelper.Instance.NativeBridge.JoinCampaign(accessToken);
    }

    public void CopyInviteLink()
    {
    }

    private int requestCode = 100;

    public void ShowRewardAdsWithCode()
    {
        RofiSdkHelper.Instance.NativeBridge.ShowVideoAds(null, requestCode);
    }

    public void ShowInterAdsWithCode()
    {
        RofiSdkHelper.Instance.NativeBridge.ShowInterAds(requestCode);
    }
}