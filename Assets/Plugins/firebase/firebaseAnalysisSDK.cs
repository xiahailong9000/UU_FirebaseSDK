﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace topifish.sdk{
    public class FirebaseAnalysisSDK  {
        private AndroidJavaObject jarInstance;
        public  Action<string,string> bannerEventHandler;
        public  Action<string, string> interstitialEventHandler;
        public  Action<string, string> rewardedVideoEventHandler;
        public  Action<string, string> nativeBannerEventHandler;
        FirebaseAnalysisSDK() { }
        static FirebaseAnalysisSDK instance;
        public static FirebaseAnalysisSDK GetInstance {
            get {
                if (instance==null) {
                    instance = new FirebaseAnalysisSDK();
                    AndroidJavaClass admobUnityPluginClass = new AndroidJavaClass("com.firebase.unityfirebase.Analysis");
                    instance.jarInstance = admobUnityPluginClass.CallStatic<AndroidJavaObject>("getInstance");
                    InnerListener innerlistener = new InnerListener();
                    innerlistener.admobInstance = instance;
                    instance.jarInstance.Call("setListener", new object[] { new AdmobListenerProxy(innerlistener) });
                    instance.jarInstance.Call("init");
                }
                return instance;
            }
        }
        public void LogEvent(string var1, string var2, string nnn) {
            jarInstance.Call("logEvent",var1,var2,nnn);
        }
        public void SetUserId(string var1) {
            jarInstance.Call("setUserId", var1);
        }
        public void SetUserProperty(string var1, string var2) {
            jarInstance.Call("setUserProperty", var1, var2);
        }
        public void SetCurrentScreen(string var1, string var2) {
            jarInstance.Call("setCurrentScreen", var1, var2);
        }
    }
    class InnerListener : IListener {
        internal FirebaseAnalysisSDK admobInstance;
        public void onEvent(string type, string eventName, string paramString) {
            Debug.LogErrorFormat("收到消息___{0}___{1}___{2}", type, eventName, paramString);
        }
    }

    public class AdmobListenerProxy : AndroidJavaProxy {
        private IListener listener;
        internal AdmobListenerProxy(IListener listener) : base("com.firebase.unityfirebase.IListener") {
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