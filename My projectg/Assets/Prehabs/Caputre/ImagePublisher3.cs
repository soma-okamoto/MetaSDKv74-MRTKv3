using UnityEngine;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using System.Collections;
// ���O��ԏՓ˂̉��
using SensorImage = RosSharp.RosBridgeClient.MessageTypes.Sensor.Image;

public class ImagePublisher3 : UnityPublisher<SensorImage>
{
    public Texture2D texture;
    private SensorImage imageMessage;

    protected override void Start()
    {
        base.Start();  // UnityPublisher �� Start
        InitializeImageMessage();
    }

    private void InitializeImageMessage()
    {
        if (texture != null)
        {
            imageMessage = new SensorImage
            {
                height = (uint)texture.height,
                width = (uint)texture.width,
                encoding = "png",
                is_bigendian = 0,
                step = (uint)(texture.width * 3)
            };
        }
    }

    void Update()
    {
        if (texture == null)
            return;

        if (imageMessage == null)
            InitializeImageMessage();

        // PNG�G���R�[�h
        byte[] imageData = texture.EncodeToPNG();
        imageMessage.data = imageData;

        // Publish
        try
        {
            Publish(imageMessage);
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"Publish failed: {ex.Message}");
        }
    }
}
