using RosSharp.RosBridgeClient.MessageTypes.Nav;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class SuggestionRouteSubscriber : UnitySubscriber<Path>
    {
        public Path messagePath;

        private List<GameObject> instantiatedObjects = new List<GameObject>(); // ���������I�u�W�F�N�g���Ǘ�

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(Path message)
        {
            messagePath = message;
            Debug.Log("Received Path Message");

        }

        
    }
}
