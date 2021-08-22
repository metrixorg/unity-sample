using UnityEngine;
using System.Collections.Generic;
using ir.metrix.unity;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Metrix.Initialize("lcqmfsnvhzznvhe");

        Metrix.SetOnAttributionChangedListener(AttributionChangedCallback);

        Metrix.NewEvent("jqgjh");
        var attributes = new Dictionary<string, string>();
        attributes.Add("one", "two");
        attributes.Add("three", "four");
        Metrix.NewEvent("jqgjh", attributes);

        Metrix.NewRevenue("prfrn", 23.44);
        Metrix.NewRevenue("prfrn", 23.44, 1);
        Metrix.NewRevenue("prfrn", 23.44, "someOrderId");
        Metrix.NewRevenue("prfrn", 23.44, 2, "someOtherOrderId");

        Metrix.SetPushToken("token");

        Metrix.SetShouldLaunchDeeplink(true);
        Metrix.SetOnDeeplinkResponseListener(DeeplinkCallback);

        Metrix.SetDefaultTracker("uevt4h");
        
        Metrix.SetAppSecret(1, 429751687, 1057026454, 796046595, 610423971);
        
        Metrix.SetStore("App Store");

        Metrix.SetUserIdListener(UserIdCallback);

        Debug.Log("[MetrixExample]: SessionId: " + Metrix.GetSessionId());
        Debug.Log("[MetrixExample]: SessionNum: " + Metrix.GetSessionNum());

        var attributes2 = new Dictionary<string, string>();
        attributes2.Add("first", "Ken");
        attributes2.Add("last", "Adams");
        Metrix.AddUserAttributes(attributes2);
    }

    public void UserIdCallback(string userId)
    {
        Debug.Log("[MetrixExample]: UserId listener called. ID: " + userId);
    }

    public void SessionIdCallback(string sessionId)
    {
        Debug.Log("[MetrixExample]: SessionId listener called. ID: " + sessionId);
    }

    public void AttributionChangedCallback(MetrixAttribution attribution)
    {
        Debug.Log("[MetrixExample]: Attribution callback received.");
        Debug.Log("[MetrixExample]: acquisitionAd = " + attribution.acquisitionAd);
        Debug.Log("[MetrixExample]: acquisitionAdSet = " + attribution.acquisitionAdSet);
        Debug.Log("[MetrixExample]: acquisitionCampaign = " + attribution.acquisitionCampaign);
        Debug.Log("[MetrixExample]: acquisitionSource = " + attribution.acquisitionSource);
        Debug.Log("[MetrixExample]: attributionStatus = " + attribution.attributionStatus);
    }

    public void DeeplinkCallback(string uri)
    {
        Debug.Log("[MetrixExample]: Deeplink callback received. deeplink: " + uri);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
