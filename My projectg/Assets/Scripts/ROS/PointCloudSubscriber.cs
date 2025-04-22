/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class PointCloudSubscriber : RosSharp.RosBridgeClient.UnitySubscriber<RosSharp.RosBridgeClient.MessageTypes.Sensor.PointCloud2>
    {
        private byte[] byteArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        private int size;

        private Vector3[] pcl;
        private Color[] pcl_color;
        public Color _color;

        int width;
        int height;
        int row_step;
        int point_step;

        List<Vector3> pcl_list = new List<Vector3>();
        List<Color> pcl_color_list = new List<Color>();

        protected override void Start()
        {
            base.Start();

        }

        public void Update()
        {

            if (isMessageReceived)
            {
                Debug.Log(width);
                StartCoroutine(PointCloudRendering());
                isMessageReceived = false;
            }
        }

        protected override void ReceiveMessage(PointCloud2 message)
        {

            
            size = message.data.GetLength(0);

            byteArray = new byte[size];
            byteArray = message.data;

            
            width = (int)message.width;
            height = (int)message.height;
            Debug.Log("width : " + width.ToString() + " height : " + height.ToString());

            row_step = (int)message.row_step;
            point_step = (int)message.point_step;
            
            size = size / point_step;
            isMessageReceived = true;
        }

        //�_�Q�̍��W��ϊ�
        IEnumerator PointCloudRendering()
        {
            pcl = new Vector3[size];
            
            pcl_color = new Color[size];

            int x_posi;
            int y_posi;
            int z_posi;

            float x;
            float y;
            float z;

            int rgb_posi;
            int rgb_max = 255;

            float r;
            float g;
            float b;

            //���̕�����byte�^��float�ɕϊ�         
            for (int n = 0; n < size; n++)
            {
                x_posi = n * point_step + 0;
                y_posi = n * point_step + 4;
                z_posi = n * point_step + 8;
                
                
                x = BitConverter.ToSingle(byteArray, x_posi);
                y = BitConverter.ToSingle(byteArray, y_posi);
                z = BitConverter.ToSingle(byteArray, z_posi);

*//*                rgb_posi = n * point_step + 16;
                
                b = byteArray[rgb_posi + 0];
                g = byteArray[rgb_posi + 1];
                r = byteArray[rgb_posi + 2];

                r = r / rgb_max;
                g = g / rgb_max;
                b = b / rgb_max;*//*
                pcl_list.Add(new Vector3(x, z, y));
                //pcl[n] = new Vector3(x, z, y);
                //pcl_color_list.Add(new Color(r, g, b));
                //pcl_color_list.Add(_color);
                //pcl_color[n] = _color;
                //pcl_color[n] = new Color(r, g, b);


            }
            pcl = pcl_list.ToArray();
            pcl_color = pcl_color_list.ToArray();

            yield return null;
        }

        public Vector3[] GetPCL()
        {
            return pcl;
        }

        public Color[] GetPCLColor()
        {
            return pcl_color;
        }

        public Vector2 GetSize()
        {
            return new Vector2(width, height);
        }
    }
}
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class PointCloudSubscriber : RosSharp.RosBridgeClient.UnitySubscriber<RosSharp.RosBridgeClient.MessageTypes.Sensor.PointCloud2>
    {
        private byte[] byteArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        private int size;

        private Vector3[] pcl;
        private Color[] pcl_color;
        public Color _color = Color.white;  // �����F�͔�

        int width;
        int height;
        int row_step;
        int point_step;

        List<Vector3> pcl_list = new List<Vector3>();
        List<Color> pcl_color_list = new List<Color>();

        protected override void Start()
        {
            base.Start();
        }

        public void Update()
        {
            if (isMessageReceived)
            {
                Debug.Log(width);
                StartCoroutine(PointCloudRendering());
                isMessageReceived = false;
            }
        }

        protected override void ReceiveMessage(PointCloud2 message)
        {
            size = message.data.GetLength(0);

            byteArray = new byte[size];
            byteArray = message.data;

            width = (int)message.width;
            height = (int)message.height;
            Debug.Log("width : " + width.ToString() + " height : " + height.ToString());

            row_step = (int)message.row_step;
            point_step = (int)message.point_step;

            size = size / point_step;
            isMessageReceived = true;
        }

        // �_�Q�̍��W��ϊ�
        IEnumerator PointCloudRendering()
        {
            pcl = new Vector3[size];
            pcl_color = new Color[size];

            int x_posi;
            int y_posi;
            int z_posi;

            float x;
            float y;
            float z;

            int rgb_posi;
            int rgb_max = 255;

            float r;
            float g;
            float b;

            // ���̕�����byte�^��float�ɕϊ�         
            for (int n = 0; n < size; n++)
            {
                x_posi = n * point_step + 0;
                y_posi = n * point_step + 4;
                z_posi = n * point_step + 8;

                x = BitConverter.ToSingle(byteArray, x_posi);
                y = BitConverter.ToSingle(byteArray, y_posi);
                z = BitConverter.ToSingle(byteArray, z_posi);

                pcl_list.Add(new Vector3(x, z, y));

                // �F�𓮓I�ɕύX�i�f�t�H���g�ł͔��j
                pcl_color_list.Add(_color); // _color ���g�p���ĐF��ύX
            }

            pcl = pcl_list.ToArray();
            pcl_color = pcl_color_list.ToArray();

            yield return null;
        }

        // �C�ӂ̓_�̐F��ύX���郁�\�b�h
        public void UpdatePointColor(int index, Color newColor)
        {
            if (index >= 0 && index < pcl_color.Length)
            {
                pcl_color[index] = newColor;
            }
        }

        // �S�Ă̓_�Q�̐F���ꊇ�ŕύX����֐�
        public void UpdateAllColors(Color newColor)
        {
            for (int i = 0; i < pcl_color.Length; i++)
            {
                pcl_color[i] = newColor;
            }
        }

        public Vector3[] GetPCL()
        {
            return pcl;
        }

        public Color[] GetPCLColor()
        {
            return pcl_color;
        }

        public Vector2 GetSize()
        {
            return new Vector2(width, height);
        }
    }
}
