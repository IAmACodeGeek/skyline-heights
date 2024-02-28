using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockSpawner : MonoBehaviour{
    public GameObject blockPrefab;
    public List<GameObject> blocks;
    public GameObject Container;
    
    public float spawnDelay = 1.5f;
    public float spawnOffsetY = 5;
    
    public GameObject container;
    public float maxContainerOffset = 2;

    public GameObject scoreObject;
    private TextMeshProUGUI scoreText;

    void Start(){
        scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        blocks.Insert(0, Instantiate(blockPrefab, transform.position, Quaternion.identity));
        blocks[0].SetActive(true);
        blocks[0].GetComponent<Block>().isFirstBlock = true;
        blocks[0].GetComponent<Block>().startIndex = 0;
        blocks[0].GetComponent<Block>().endIndex = 19;
        dropBlock();
    }

    void Update(){
        if(blocks[0] == null){
            GameStateController.gsc.LoadScene(Tags.MAIN_MENU_STATE);
            return;
        }
        if(Input.GetMouseButtonDown(0) && Block.hasSpawned){
            dropBlock();
        }
        if(blocks.Count > 1
        && !container.GetComponent<ContainerMovement>().isMoving 
        && blocks[1].transform.position.y - container.transform.position.y > maxContainerOffset){
            container.GetComponent<ContainerMovement>().startMoving(blocks[3].transform.position);
        }

        scoreText.SetText("Score: " + blocks[0].GetComponent<Block>().score.ToString());
    }
    
    IEnumerator spawnBlock(){
        yield return new WaitForSeconds(spawnDelay);
        Vector2 spawnPosition = new Vector2(Random.Range(-3f, 3f), blocks[0].transform.position.y + spawnOffsetY);
        blocks.Insert(0, Instantiate(blockPrefab, spawnPosition, Quaternion.identity));
        blocks[0].SetActive(true);
        blocks[0].GetComponent<Block>().xDirection = (Random.Range(-1, 2) >= 0)? 1:-1;
        blocks[0].GetComponent<Block>().startIndex = blocks[1].GetComponent<Block>().startIndex;
        blocks[0].GetComponent<Block>().endIndex = blocks[1].GetComponent<Block>().endIndex;
        blocks[0].GetComponent<Block>().score = blocks[1].GetComponent<Block>().score;

    }

    void dropBlock(){
        blocks[0].GetComponent<Block>().state = Block.DROP;
        Block.hasSpawned = false;
        blocks[0].GetComponent<Block>().psTop.Play();
        StartCoroutine(spawnBlock());
    }
}
