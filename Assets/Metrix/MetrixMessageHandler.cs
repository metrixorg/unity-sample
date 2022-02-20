using System;
using ir.metrix.unity;
using UnityEngine;

public class MetrixMessageHandler : MonoBehaviour {

	public void OnDeferredDeeplink(String uri) {
		Metrix.OnDeferredDeeplink(uri);
	}
	
	public void OnAttributionChangeListener(String attributionDataString) {
		Metrix.OnAttributionChangeListener(attributionDataString);
	}

	public void OnReceiveUserIdListener(String userId) {
		Metrix.OnReceiveUserIdListener(userId);
	}

	public void SessionIDChangeListener(String sessionId) {
		Metrix.SessionIDChangeListener(sessionId);
	}

	public void SessionNumberChangeListener(String sessionNum) {
		Metrix.SessionNumberChangeListener(sessionNum);
	}

}