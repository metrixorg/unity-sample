using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace ir.metrix.unity
{
    public class Metrix
    {

    #if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass metrixAndroid = new AndroidJavaClass("ir.metrix.unity.MetrixUnity");
    #endif

    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]
        private static extern void _Initialize(string appId);
        [DllImport ("__Internal")]
        private static extern int _GetSessionNum();
        [DllImport ("__Internal")]
        private static extern string _GetSessionId();
        [DllImport ("__Internal")]
        private static extern void _NewEvent(string slug);
        [DllImport ("__Internal")]
        private static extern void _NewAttributedEvent(string slug, string customAttributes);
        [DllImport ("__Internal")]
        private static extern void _NewRevenue(string slug, double revenue);
        [DllImport ("__Internal")]
        private static extern void _NewRevenueCurrency(string slug, double revenue, int currency);
        [DllImport ("__Internal")]
        private static extern void _NewRevenueFull(string slug, double revenue, int currency, string orderId);
        [DllImport ("__Internal")]
        private static extern void _NewRevenueOrderId(string slug, double revenue, string orderId);
        [DllImport ("__Internal")]
        private static extern void _AddUserAttributes(string userAttrs);
        [DllImport ("__Internal")]
        private static extern void _SetPushToken(string pushToken);
        [DllImport ("__Internal")]
        private static extern void _SetStore(string storeName);
        [DllImport ("__Internal")]
        private static extern void _SetAppSecret(int secretId, long info1, long info2, long info3, long info4);
        [DllImport ("__Internal")]
        private static extern void _SetDefaultTracker(string trackerToken);
        [DllImport ("__Internal")]
        private static extern void _SetUserIdListener();
        [DllImport ("__Internal")]
        private static extern void _SetOnDeeplinkResponseListener(bool shouldLaunchDeferredDeeplink);
        [DllImport ("__Internal")]
        private static extern void _SetOnAttributionChangedListener();
    #endif

        private static GameObject metrixManager = null;
        
        private static Action<string> deferredDeeplinkDelegate = null;
        private static Action<MetrixAttribution> userAttributionDelegate = null;
        private static Action<string> userIdDelegate = null;
        
        private static bool shouldLaunchDeferredDeeplink = true;

        // Not used in Android
        public static void Initialize(string appId)
        {
        #if UNITY_IOS && !UNITY_EDITOR
            _Initialize(appId);
        #endif
        }

        public static int GetSessionNum()
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return metrixAndroid.CallStatic<Int32>("getSessionNum");
        #elif UNITY_IOS && !UNITY_EDITOR
            return _GetSessionNum();
        #else
            return 0;
        #endif
        }
        
        public static string GetSessionId()
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            return metrixAndroid.CallStatic<String>("getSessionId");
        #elif UNITY_IOS && !UNITY_EDITOR
            return _GetSessionId();
        #else
            return "";
        #endif
        }
        
        public static void NewEvent(string slug)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
			metrixAndroid.CallStatic("newEvent", slug);
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewEvent(slug);
        #endif
        }
        
        public static void NewEvent(string slug,
                                    Dictionary<string, string> customAttributes)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("newEvent", slug, ConvertDictionaryToMap(customAttributes));
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewAttributedEvent(slug, ConvertDictionaryToString(customAttributes));
        #endif
        }
        
        public static void NewRevenue(string slug, double revenue)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("newRevenueSimple", slug, revenue);
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewRevenue(slug, revenue);
        #endif
        }
        
        public static void NewRevenue(string slug, double revenue, int currency)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            string cr = "IRR";
            if (currency == 0)
            {
                cr = "IRR";
            }
            else if (currency == 1)
            {
                cr = "USD";
            }
            else if (currency == 2)
            {
                cr = "EUR";
            }
            metrixAndroid.CallStatic("newRevenueCurrency", slug, revenue, cr);
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewRevenueCurrency(slug, revenue, currency);    
        #endif
        }
        
        public static void NewRevenue(string slug, double revenue, int currency, string orderId)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            string cr = "IRR";
            if (currency == 0)
            {
                cr = "IRR";
            }
            else if (currency == 1)
            {
                cr = "USD";
            }
            else if (currency == 2)
            {
                cr = "EUR";
            }
            metrixAndroid.CallStatic("newRevenueFull", slug, revenue, cr, orderId);
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewRevenueFull(slug, revenue, currency, orderId);
        #endif
        }
        
        public static void NewRevenue(string slug, double revenue, string orderId)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("newRevenueOrderId", slug, revenue, orderId);
        #elif UNITY_IOS && !UNITY_EDITOR
            _NewRevenueOrderId(slug, revenue, orderId);
        #endif
        }
        
        public static void AddUserAttributes(Dictionary<string, string> userAttrs)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("addUserAttributes", ConvertDictionaryToMap(userAttrs));
        #elif UNITY_IOS && !UNITY_EDITOR
            _AddUserAttributes(ConvertDictionaryToString(userAttrs));
        #endif
        }
        
        public static void SetShouldLaunchDeeplink(bool launch)
        {
            shouldLaunchDeferredDeeplink = launch;
        }
        
        public static void SetPushToken(string pushToken)
        {
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("setPushToken", pushToken);
        #elif UNITY_IOS && !UNITY_EDITOR
            _SetPushToken(pushToken);
        #endif
        }
        
        // Not used in Android
        public static void SetStore(string storeName)
        {
        #if UNITY_IOS && !UNITY_EDITOR
            _SetStore(storeName);
        #endif
        }
        
        // Not used in Android
        public static void SetAppSecret(int secretId, long info1, long info2, long info3, long info4)
        {
        #if UNITY_IOS && !UNITY_EDITOR
            _SetAppSecret(secretId, info1, info2, info3, info4);
        #endif
        }
        
        // Not used in Android
        public static void SetDefaultTracker(string trackerToken)
        {
        #if UNITY_IOS && !UNITY_EDITOR
            _SetDefaultTracker(trackerToken);
        #endif
        }
        
        public static void SetUserIdListener(Action<string> callback)
        {
            if (metrixManager == null)
            {
                metrixManager = new GameObject("MetrixManager");
                UnityEngine.Object.DontDestroyOnLoad(metrixManager);
                metrixManager.AddComponent<MetrixMessageHandler>();
            }

            userIdDelegate = callback;
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("setUserIdListener");
        #elif UNITY_IOS && !UNITY_EDITOR
            _SetUserIdListener();
        #endif
        }

        public static void SetOnDeeplinkResponseListener(Action<string> callback)
        {
            if (metrixManager == null)
            {
                metrixManager = new GameObject("MetrixManager");
                UnityEngine.Object.DontDestroyOnLoad(metrixManager);
                metrixManager.AddComponent<MetrixMessageHandler>();
            }

            deferredDeeplinkDelegate = callback;
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("setOnDeeplinkResponseListener", shouldLaunchDeferredDeeplink);
        #elif UNITY_IOS && !UNITY_EDITOR
            _SetOnDeeplinkResponseListener(shouldLaunchDeferredDeeplink);
        #endif
        }

        public static void SetOnAttributionChangedListener(Action<MetrixAttribution> callback)
        {
            if (metrixManager == null)
            {
                metrixManager = new GameObject("MetrixManager");
                UnityEngine.Object.DontDestroyOnLoad(metrixManager);
                metrixManager.AddComponent<MetrixMessageHandler>();
            }

            userAttributionDelegate = callback;
        #if UNITY_ANDROID && !UNITY_EDITOR
            metrixAndroid.CallStatic("setOnAttributionChangedListener");
        #elif UNITY_IOS && !UNITY_EDITOR
            _SetOnAttributionChangedListener();
        #endif
        }

        public static void OnDeferredDeeplink(string uri)
        {
            if (deferredDeeplinkDelegate != null)
            {
                deferredDeeplinkDelegate(uri);
            }
        }
        
        public static void OnReceiveUserIdListener(string userId)
        {
            if (userIdDelegate != null)
            {
                userIdDelegate(userId);
            }
        }

        public static void OnAttributionChangeListener(string attributionDataString)
        {
            if (userAttributionDelegate != null)
            {
                userAttributionDelegate(new MetrixAttribution(attributionDataString));
            }
        }

        private static string ConvertDictionaryToString(IDictionary<string, string> parameters)
        {
            string parametersString = "";
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                parametersString = parametersString + kvp.Key;
                parametersString = parametersString + "=";
                parametersString = parametersString + kvp.Value;
                parametersString = parametersString + "\n";
            }

            return parametersString;   
        }
        
        private static AndroidJavaObject ConvertDictionaryToMap(IDictionary<string, string> parameters)
        {
            AndroidJavaObject javaMap = new AndroidJavaObject("java.util.HashMap");
            IntPtr putMethod = AndroidJNIHelper.GetMethodID(
                javaMap.GetRawClass(), "put",
                    "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in parameters)
            {
                using (AndroidJavaObject k = new AndroidJavaObject(
                    "java.lang.String", kvp.Key))
                {
                    using (AndroidJavaObject v = new AndroidJavaObject(
                        "java.lang.String", kvp.Value))
                    {
                        args[0] = k;
                        args[1] = v;
                        AndroidJNI.CallObjectMethod(javaMap.GetRawObject(),
                                putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }

            return javaMap;
        }
    }
}