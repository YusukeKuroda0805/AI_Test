using UnityEngine;
using UnityEngine.UI;

public class PuzzleCellView : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    public int Number { get; private set; }//読み取り専用プロパティ

    public void SetNumber(int number)
    {
        _label.text = $"{(number != Puzzle.Area - 1 ? (number + 1).ToString() : "")}";//$ 文字列補間式　
                                                                                      //? ブール式を評価,true または falseに応じて、2 つの式のいずれかの評価結果を返す
        Number = number;
    }
}
