



using System.Collections.Generic;
using UnityEngine;
using UnityFx.Outline;

public class OutlineOnView : MonoBehaviour
{
    public Camera playerCamera;
    private float maxRaycastDistance = 0.5f;
    private Ray ray;
    private RaycastHit hitObj;
    public GameObject hitObject;
    private GameObject previousHitObject;

    public Vector3 bottlePosition = Vector3.zero;

    private GameObject[] masterBottles;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>(); // �{�g���̌��̐F��ێ�
    public Dictionary<GameObject, GameObject> masterToSubMapping = new Dictionary<GameObject, GameObject>(); // �}�X�^�[�ƃT�u�̑Ή��֌W

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // �}�X�^�[�{�g�����擾
        masterBottles = GameObject.FindGameObjectsWithTag("bottle");
        foreach (var masterBottle in masterBottles)
        {
            var renderer = masterBottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                //originalColors[masterBottle] = renderer.material.color;
                renderer.material = new Material(renderer.material); // �C���X�^���X��
                originalColors[masterBottle] = renderer.material.GetColor("_BaseColor");
            }

            // �Ή�����T�u�{�g����T��
            GameObject subBottle = GameObject.Find(masterBottle.name.Replace("Master", "Sub"));
            if (subBottle != null)
            {
                masterToSubMapping[masterBottle] = subBottle;
            }
        }
    }

    void Update()
    {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out hitObj, maxRaycastDistance))
        {
            hitObject = hitObj.collider.gameObject;

            if (hitObject != previousHitObject)
            {
                // �O�̃I�u�W�F�N�g�̃A�E�g���C���𖳌���
                if (previousHitObject != null)
                {
                    ToggleOutline(previousHitObject, false);
                }

                // ���݂̃I�u�W�F�N�g�̃A�E�g���C����L����
                ToggleOutline(hitObject, true);

                previousHitObject = hitObject;
            }

            // �y�b�g�{�g���ɓ��������ꍇ�A���̍��W���i�[
            if (hitObject != null && hitObject.CompareTag("bottle"))
            {
                bottlePosition = hitObject.transform.position;
                //UnityEngine.Debug.Log("Pet Bottle Position: " + bottlePosition);
            }
        }
        else
        {
            // ���C�����ɂ�������Ȃ��ꍇ
            if (previousHitObject != null)
            {
                ToggleOutline(previousHitObject, false);
                previousHitObject = null;
            }

            hitObject = null;
            bottlePosition = Vector3.zero;
        }

        // �F�̍X�V����
        foreach (var masterBottle in masterBottles)
        {
            var renderer = masterBottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color newColor;

                if (hitObject != null && masterBottle == hitObject)
                {
                    // �q�b�g�����{�g���͒ʏ�̐F
                    //renderer.material.color = originalColors[masterBottle];
                    newColor = originalColors[masterBottle];
                }
                else if (hitObject != null)
                {
                    // �q�b�g���Ă��Ȃ��{�g���̓O���[�A�E�g
                    //renderer.material.color = Color.gray;
                    newColor = Color.gray;
                }
                else
                {
                    // �S�Ēʏ�̐F
                    //renderer.material.color = originalColors[masterBottle];
                    newColor = originalColors[masterBottle];

                }

                /*// �T�u�{�g���ɐF�𓯊�
                if (masterToSubMapping.ContainsKey(masterBottle))
                {
                    var subBottle = masterToSubMapping[masterBottle];
                    var subRenderer = subBottle.GetComponent<Renderer>();
                    if (subRenderer != null)
                    {
                        subRenderer.material.color = renderer.material.color;
                    }
                }*/

                renderer.material.SetColor("_BaseColor", newColor);
            }
        }
    }

    private void ToggleOutline(GameObject obj, bool enable)
    {
        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null)
        {
            outline.enabled = enable;
        }

        // �T�u�{�g���̃A�E�g���C��������
        if (masterToSubMapping.ContainsKey(obj))
        {
            var subBottle = masterToSubMapping[obj];
            var subOutline = subBottle.GetComponent<OutlineBehaviour>();
            if (subOutline != null)
            {
                subOutline.enabled = enable;
            }
        }
    }
}
