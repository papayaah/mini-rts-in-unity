using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighlighter : MonoBehaviour
{
	public Tilemap tilemap;
	public Color highlightColor = Color.yellow;
	private Color originalColor = Color.white;
	private TileBase currentTile;
	private Vector3Int previousCellPosition = Vector3Int.down;
	
	public GameObject housePrefab; 	
	public GameObject pawnPrefab;

	void Update()
	{
		HighlightTile();
		if (Input.GetMouseButtonDown(0)) // 0 is for left click
		{
			CreateHouseAtTile();
		}
	}

	void HighlightTile()
	{
		Vector3Int cellPosition = GetCurrentCellPosition();

		if (cellPosition != previousCellPosition)
		{
			if (tilemap.HasTile(previousCellPosition))
			{
				// Reset the previous tile's color
				tilemap.SetTileFlags(previousCellPosition, TileFlags.None);
				tilemap.SetColor(previousCellPosition, originalColor);
			}

			if (tilemap.HasTile(cellPosition))
			{
				// Highlight the new tile
				tilemap.SetTileFlags(cellPosition, TileFlags.None);
				tilemap.SetColor(cellPosition, highlightColor);
			}

			previousCellPosition = cellPosition;
		}
	}

	Vector3Int GetCurrentCellPosition()
	{
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);
		return cellPosition;
	}

	void ResetHighlight()
	{
		if (currentTile != null)
		{
			// Reset the tile's color or remove the highlight
			// tilemap.SetColor(previousCellPosition, originalColor);
			currentTile = null;
		}
	}
	
	
	void CreateHouseAtTile()
	{
		Vector3Int cellPosition = GetCurrentCellPosition();
		if (tilemap.HasTile(cellPosition))
		{
			Vector3 cellWorldPos = tilemap.CellToWorld(cellPosition);
			cellWorldPos += new Vector3(tilemap.cellSize.x / 2, tilemap.cellSize.y, -20);
			
			GameObject house = Instantiate(housePrefab, cellWorldPos, Quaternion.identity);
			// Access the HouseScript component and call SwitchSprite
			Building script = house.GetComponent<Building>();
			if (script != null)
			{
			}
			
			Vector3 pawnSpawnPosition = GetPawnSpawnPosition(cellWorldPos);
			GameObject pawnInstance = Instantiate(pawnPrefab, pawnSpawnPosition, Quaternion.identity);

			// Initiate pawn movement towards the house
			Movement movement = pawnInstance.GetComponent<Movement>();
			if (movement != null)
			{
				movement.MoveToTarget(house.transform.position + new Vector3(0, -0.5f, 0));
			}
			
			StartCoroutine(ChangeHouseSpriteAfterDelay(house, 3f));
			
		}
	}	
	
	IEnumerator ChangeHouseSpriteAfterDelay(GameObject house, float delay)
	{
		// Wait for the specified delay
		yield return new WaitForSeconds(delay);

		// Get the script component and call SwitchSprite
		Building script = house.GetComponent<Building>();
		if (script != null)
		{
			script.SwitchSprite(1); 
		}
	}
	
	Vector3 GetPawnSpawnPosition(Vector3 housePosition)
	{
		// Define the range for random spawn around the house
		float spawnRadius = 3.0f; // Adjust this value as needed

		// Generate a random angle
		float angle = Random.Range(0f, 360f);
		float radian = angle * Mathf.Deg2Rad;

		// Calculate the random spawn position
		Vector3 spawnPosition = new Vector3(
			housePosition.x + spawnRadius * Mathf.Cos(radian), 
			housePosition.y + spawnRadius * Mathf.Sin(radian), 
			housePosition.z
		);

		return spawnPosition;
	}
}
