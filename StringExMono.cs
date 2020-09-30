using UnityEngine;
using static StringEx;


public class StringExMono : MonoBehaviour
{
    public int value = 10;
    void Start()
    {
        Debug.Log($" IsSaving :C:b:18; hello {Apply(Color.red, FontStyle.Bold, value)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.yellow, FontStyle.BoldAndItalic, value)}".Interpolate());
        Debug.Log($" IsSaving :white:i; hello :BLACK:{FontStyle.BoldAndItalic}:19; Thus ReStarting :i; SaveRoutine  :yellow:10;".Interpolate());
        Debug.Log($" IsSaving :W:I:15; hello :R:BI:19; Thus ReStarting :i;  \"SaveRoutine\" :M:10:I;".Interpolate());
        Debug.Log($" IsSaving :{Color.gray}:b; hello {Apply("Bl", "BI", 25)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.yellow, FontStyle.BoldAndItalic, value)}".Interpolate());

        Log('*');
        Log($" NoTrace :C:b:18; hello {Apply(Color.red, FontStyle.Bold, value)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.green, FontStyle.BoldAndItalic, value)}");

        Log($"tesTing bAd Case HELLO :G:19:Bi:u; ");
        Log($"tesTing :W:u; SuPerbAd :G:19:Bi:u; CaSe :B:u; HELLO :{Color.magenta}:19:{FontStyle.Bold}:u;");

        LogError($" NoTrace :white:i; hello :BLACK:{FontStyle.BoldAndItalic}:19; Thus ReStarting :i; SaveRoutine  :yellow:10;");
        LogWarning($" NoTrace :W:I:15; hello :R:BI:19; Thus ReStarting :i;  \"SaveRoutine\" :M:10:I;");
        LogAssert($" NoTrace :{Color.gray}:b; hello {Apply("Bl", "BI", 25)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.magenta, FontStyle.BoldAndItalic, value)}");

        Log(Color.green);

        LogT($" WithTrace :C:b:10:I; HELLO {Apply(Color.red, FontStyle.Bold, value)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.yellow, FontStyle.BoldAndItalic, value)}");
        LogErrorT($" WithTrace :white:i; hello :BLACK:{FontStyle.BoldAndItalic}:19; Thus ReStarting :i; SaveRoutine  :yellow:10;");
        LogWarningT($" WithTrace :W:I:15; HELLO :R:BI:19:l; Thus ReStarting :i;  \"SaveRoutine\" :M:10:I;");
        LogAssertT($" WithTrace :{Color.gray}:b; hello {Apply("Bl", "BI", 25)} Thus ReStarting {Apply("green,bi")}   \"SaveRoutine\"{Apply(Color.magenta, FontStyle.BoldAndItalic, value)}");

        Log(Color.blue, '#', 200);
    }


}
