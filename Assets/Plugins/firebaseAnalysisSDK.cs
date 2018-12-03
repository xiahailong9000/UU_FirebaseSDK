﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace topifish.sdk{
    public class firebaseAnalysisSDK  {

        private AndroidJavaObject jarInstance;
        public  Action<string,string> bannerEventHandler;
        public  Action<string, string> interstitialEventHandler;
        public  Action<string, string> rewardedVideoEventHandler;
        public  Action<string, string> nativeBannerEventHandler;
        public void Init() {
            if (jarInstance == null) {
                AndroidJavaClass admobUnityPluginClass = new AndroidJavaClass("com.firebase.unity3dlibrary.AnalysisSDK");
                jarInstance = admobUnityPluginClass.CallStatic<AndroidJavaObject>("getInstance");
                InnerListener innerlistener = new InnerListener();
                innerlistener.admobInstance = this;
                jarInstance.Call("setListener", new object[] { new AdmobListenerProxy(innerlistener) });
                jarInstance.Call("init");
            }
        }
        public void logEvent(string var1, string var2, string nnn) {
            jarInstance.Call("logEvent",var1,var2,nnn);
        }

    }
    class InnerListener : IListener {
        internal firebaseAnalysisSDK admobInstance;
        public void onEvent(string adtype, string eventName, string paramString) {
            //if (adtype == "banner") {
            //    if (admobInstance.bannerEventHandler != null)
            //        admobInstance.bannerEventHandler(eventName, paramString);
            //} else if (adtype == "interstitial") {
            //    if (admobInstance.interstitialEventHandler != null)
            //        admobInstance.interstitialEventHandler(eventName, paramString);
            //} else if (adtype == "rewardedVideo") {
            //    if (admobInstance.rewardedVideoEventHandler != null)
            //        admobInstance.rewardedVideoEventHandler(eventName, paramString);
            //} else if (adtype == "nativeBanner") {
            //    if (admobInstance.nativeBannerEventHandler != null)
            //        admobInstance.nativeBannerEventHandler(eventName, paramString);
            //}
            Debug.LogErrorFormat("收到消息___{0}___{1}___{2}", adtype, eventName, paramString);
        }
    }

    public class AdmobListenerProxy : AndroidJavaProxy {
        private IListener listener;
        internal AdmobListenerProxy(IListener listener) : base("com.xia.firebase.analysis.IListener") {
            this.listener = listener;
        }
        void onEvent(string adtype, string eventName, string paramString) {
            //  Debug.Log("c# admoblisterproxy "+adtype+"   "+eventName+"   "+paramString);
            if (listener != null) {
                listener.onEvent(adtype, eventName, paramString);
            }
        }
        override public string toString() {
            return "ListenerProxy";
        }
    }
    internal interface IListener {
        void onEvent(string adtype, string eventName, string paramString);
    }
}