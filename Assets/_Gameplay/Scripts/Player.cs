using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform tf;
    [SerializeField] private float speed;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private Transform stackHolder;
    private int numberOfStack;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(direction == Vector3.zero)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector3.left;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector3.right;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector3.forward;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector3.back;
            }
            TargetPosition(direction);
        }
        else
        {
            tf.position = Vector3.MoveTowards(tf.position, targetPos, speed * Time.deltaTime);
            if(Vector3.Distance(tf.position, targetPos) < 0.01f)
            {
                tf.position = targetPos;
                direction = Vector3.zero;
            }
        }
    }

    public void TargetPosition(Vector3 direction)
    {
        int groundLayerMask = LayerMask.GetMask(Constant.LAYER_GROUND);
        if(Physics.Raycast(tf.position, direction, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            targetPos = tf.position + direction * (hit.distance - 0.5f);
        }
    }

    public void AddStack()
    {
        player.position += Vector3.up * 0.25f;
        Instantiate(stackPrefab, stackHolder.position + Vector3.up * numberOfStack * 0.3f, Quaternion.Euler(new Vector3(-90, 0, 180)), stackHolder);
        numberOfStack += 1;
    }

    public void RemoveStack()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_STACK))
        {
            AddStack();
            Destroy(other.gameObject);
        }

        if (other.CompareTag(Constant.TAG_UNSTACK))
        {
            RemoveStack();
            Destroy(other.gameObject);
        }

        if (other.CompareTag(Constant.TAG_WIN))
        {

        }
    }
}
