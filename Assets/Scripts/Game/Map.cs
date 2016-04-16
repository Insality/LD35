﻿using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject CellPrefab;
    public GameObject SpikesPrefab;
    public GameObject KeyPrefab;
    public GameObject SpawnPrefab;
    public GameObject BoxPrefab;
    public GameObject TeslaPrefab;
    public GameObject PlatePrefab;
    public GameObject DoorPrefab;
    public GameObject RabbitPrefab;
    public GameObject HunterPrefab;
    public GameObject TurretPrefab;
    public GameObject ArrowPrefab;
    public GameObject BossPrefab;
    
    public Cell[,] Cells;
    public List<LevelEntity> Items;
    public List<LevelEntity> _itemsToRemove;

   [HideInInspector] public Vector2 MapSize;
   [HideInInspector] public Vector2 SpawnPoint;

    void Start()
    {
        GameController.GetInstance().GameStep += GameStep;
        Items = new List<LevelEntity>();
        _itemsToRemove = new List<LevelEntity>();
    }

    private void GameStep(int counter)
    {
        #region GameStart

        if (counter == 1)
        {
            Cells[(int) SpawnPoint.x + 2, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 2, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y + 2].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y - 2].SetState(CellState.Normal);
        }
        if (counter == 2)
        {
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
        }
        if (counter == 3)
        {
            Cells[(int) SpawnPoint.x + 1, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x - 1, (int) SpawnPoint.y].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y + 1].SetState(CellState.Normal);
            Cells[(int) SpawnPoint.x, (int) SpawnPoint.y - 1].SetState(CellState.Normal);
        }

        #endregion



        for (int i = 0; i < MapSize.y; i++) {
            for (int j = 0; j < MapSize.x; j++) {
                Cells[i, j].OnGameBeat(counter);
            }
        }

        foreach (var item in Items)
        {
            if (item.gameObject != null)
            {
                item.OnGameBeat(counter);
            }
        }

        foreach (var item in _itemsToRemove)
        {
            Items.Remove(item);
            Destroy(item.gameObject);
        }
        _itemsToRemove.Clear();


    }

    public bool IsCanStep(Vector2 coords, MoveDirection dir)
    {
        if (GetCell(coords).State != CellState.Disabled && (GetCell(coords).LevelItem == null || GetCell(coords).LevelItem.CanStepOn))
        {
            if (GetCell(coords).CanMoveItem(dir))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void LoadMap()
    {
        var level = Resources.Load<TextAsset>("Level/Level");
        var lines = level.text.Split('\n');

        MapSize.y = lines[0].Length-1;
        MapSize.x = lines.Length;

        print(MapSize);
        // init:
        Cells = new Cell[(int)MapSize.y, (int)MapSize.x];
        for (int i = 0; i < MapSize.y; i++)
        {
            for (int j = 0; j < MapSize.x; j++)
            {
                Cells[i, j] = Instantiate(CellPrefab).GetComponent<Cell>();
                Cells[i, j].transform.parent = transform;
                Cells[i, j].Coords = new Vector2(i, j);
                Cells[i, j].SetPos();
                Cells[i, j].Map = this;
            }
        }

        
        for (int i = 0; i < MapSize.y; i++)
        {
            for (int j = 0; j < MapSize.x; j++)
            {
                var ch = lines[(int)MapSize.x - j - 1][i];
                switch (ch)
                {
                    case '#':
                        Cells[i, j].SetState(CellState.Disabled);
                        break;
                    case '.':
                        Cells[i, j].SetState(CellState.Normal);
                        break;
                    case 'K':
                        Cells[i, j].AddItem(KeyPrefab);
                        break;
                    case 'B':
                        Cells[i, j].AddItem(BoxPrefab);
                        break;
                    case 'T':
                        Cells[i, j].AddItem(TurretPrefab);
                        break;
                    case 'S':
                        Cells[i, j].AddItem(SpikesPrefab);
                        break;
                    case '>':
                        SpawnPoint = new Vector2(i, j);
                        Cells[i, j].AddItem(SpawnPrefab);
                        break;
                    case '~':
                        Cells[i, j].AddItem(TeslaPrefab);
                        break;
                    case 'P':
                        Cells[i, j].AddItem(PlatePrefab);
                        break;
                    case 'A':
                        Cells[i, j].AddItem(ArrowPrefab);
                        break;
                }
            }
        }
        
    }

    public void MoveFromEvent(Vector2 coords, MoveDirection dir) {
        Cells[(int)coords.x, (int)coords.y].OnMoveFrom();
    }

    public void MoveToEvent(Vector2 coords, MoveDirection dir)
    {
        Cells[(int)coords.x, (int)coords.y].OnMoveOn(dir);
    }

    public void StayEvent(Vector2 coords) {
        Cells[(int)coords.x, (int)coords.y].OnStay();
    }

    public Cell GetCell(Vector2 v)
    {
        return Cells[(int) v.x, (int) v.y];
    }

    public void DestroyItem(TurretProjectile item)
    {
        _itemsToRemove.Add(item);
    }
}