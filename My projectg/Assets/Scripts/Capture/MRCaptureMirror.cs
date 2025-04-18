using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MRCaptureMirror : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Camera cam;
    private Texture2D tex;

    void Start()
    {
        cam = GetComponent<Camera>();
        tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
    }

    void OnPostRender()
    {
        // �J�����̕`�挋�ʂ� RenderTexture �� Blit�i�R�s�[�j
        Graphics.Blit(null, renderTexture);

        // RenderTexture ���� Texture2D �֓ǂݎ��
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        // JPEG�ɕϊ�����ROS���M�Ȃ�
        byte[] jpg = tex.EncodeToJPG(75);
        // SendToROS(jpg);
    }
}
