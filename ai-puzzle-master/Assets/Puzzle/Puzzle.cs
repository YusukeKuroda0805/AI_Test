﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    private const int Width = 4;
    private const int Height = 4;
    public const int Area = Width * Height;

    [SerializeField]
    private PuzzleCellView _cellViewPrefab;

    // セルを均等に配置するためにGridLayoutGroupを使う（オブジェクトを均等に配置出来る）
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    private PuzzleCellView[][] _cellViews;
    private int[][] _puzzle;

    // 空白マスの位置を覚えておく
    private Vector2Int _emptyPos;

    // 一度だけ実行すれば良い初期化をAwakeで行う
    void Awake()
    {
        _gridLayoutGroup.constraintCount = Width;
        _cellViews = new PuzzleCellView[Height][];
        for (int i = 0; i < Height; i++)
        {
            _cellViews[i] = new PuzzleCellView[Width];
            for (int j = 0; j < Width; j++)
            {
                var cellView = Instantiate(_cellViewPrefab, _gridLayoutGroup.transform, false);
                _cellViews[i][j] = cellView;
            }
        }
    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            Slide(2);
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            Slide(3);
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            Slide(0);
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            Slide(1);
//        }
//    }

    // ゲームの初期化（ゲームのリセットの度に呼ばれる）
    public void Initialize()
    {
        var cnt = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var cellView = _cellViews[i][j];
                cellView.SetNumber(cnt++);
            }
        }

        _emptyPos = new Vector2Int(Width - 1, Height - 1);

        // 適当な回数ランダムでスライドして初期配置を作る
        for (int i = 0; i < Area * Area * 3; i++)
        {
            Slide(Random.Range(0, 4));
        }
    }

    public List<int> GetCellNumbers()
    {
        var res = new List<int>();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                res.Add(_cellViews[i][j].Number);
            }
        }

        return res;
    }

    // 上下左右の移動を簡潔に記述するための補助配列
    private static readonly int[] dy = { -1, 0, 1, 0 };
    private static readonly int[] dx = { 0, 1, 0, -1 };

    // スライドさせるためのメソッド
    // dirは0～3の整数で、それぞれ空白マスを上右下左に動かすことに対応する
    // スライドに成功したらtrueが返る
    public bool Slide(int dir)
    {
        var x = _emptyPos.x + dx[dir];
        var y = _emptyPos.y + dy[dir];
        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        var cellView = _cellViews[_emptyPos.y][_emptyPos.x];
        var number = _cellViews[y][x].Number;
        _cellViews[y][x].SetNumber(cellView.Number);
        cellView.SetNumber(number);
        _emptyPos = new Vector2Int(x, y);
        return true;
    }
    　//目的の配置にどれだけ近づいているかを所得するメソッド
    public int GetFirstDifferentIndex()
    {
        var cnt = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (cnt != _cellViews[i][j].Number)
                {
                    return cnt;
                }

                cnt++;
            }
        }

        return cnt;
    }
}
