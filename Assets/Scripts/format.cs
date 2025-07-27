using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using Unity.Mathematics;

public class Format : MonoBehaviour
{
 


    void Start()
    {
         BigDouble[] testValues = {
            new BigDouble(1,431794361786543),   // Very large value to test the limits
        };

        foreach (var value in testValues)
        {
            string abbreviation = GetAbbreviation(value);
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string formatNumber(BigDouble n) {



        return GetAbbreviation(n);
    }


public static string GetAbbreviation(BigDouble e)
{
    // Arrays for abbreviation prefixes
    string[][] prefixes = new string[][]
    {
        new string[] { "", "U", "D", "T", "Qa", "Qt", "Sx", "Sp", "O", "N" },
        new string[] { "", "Dc", "Vg", "Tg", "Qd", "Qi", "Se", "St", "Og", "Nn" },
        new string[] { "", "Ce", "Dn", "Tc", "Qe", "Qu", "Sc", "Si", "Oe", "Ne" }
    };

    string[] prefixes2 = {"","MI-","MC-","NA-","PC-","FM-","AT-"};
    

    // Calculate the logarithm of 'e'
    double exp = BigDouble.Log10(e);

    BigDouble mantissa = BigDouble.Divide(e, BigDouble.Pow(1000, math.floor(exp / 3)));

    exp = exp - 3;

    string ret = "";



if (exp >= 3000) 
{
    // Calculate the divisor index based on the logarithm of exp
    int divisor_index = (int)(math.log10(exp/3)/3);

    // Calculate the divisor as 1000 raised to the power of divisor_index
    double divisor = math.pow(1000, divisor_index);

    // Debugging: Log divisor index and value
    Debug.Log($"Divisor Index: {divisor_index}, Divisor: {divisor}");

    // Calculate indices with bounds checking
    int index11 = (int)((exp / 3 / divisor) % 10);
    int index21 = (int)((exp / 30 / divisor) % 10);
    int index31 = (int)((exp / 300 / divisor) % 10);

    // Debugging: Log calculated indices
    Debug.Log($"Indices - index11: {index11}, index21: {index21}, index31: {index31}");

    // Construct prefix1 safely
    string prefix1 = prefixes[0][index11] + prefixes[1][index21] + prefixes[2][index31] + prefixes2[divisor_index];
    string prefix2 = "";

    // Update exp
    exp = exp - math.floor(exp / 3 / divisor) * divisor * 3;

    // Debugging: Log updated exp
    Debug.Log($"Updated exp after modulus: {exp}");

    // Calculate the second divisor
    double divisor2 = math.pow(1000, divisor_index - 1);

    // Ensure divisor2 is valid and not zero before proceeding
    if (exp != 0) 
    {
        int index12 = (int)((exp / 3 / divisor2) % 10);
        int index22 = (int)((exp / 30 / divisor2) % 10);
        int index32 = (int)((exp / 300 / divisor2) % 10);

        // Debugging: Log calculated indices for the second divisor
        Debug.Log($"Second Divisor: {divisor2}");
        Debug.Log($"Second Indices - index12: {index12}, index22: {index22}, index32: {index32}");

        // Construct prefix2 safely
        prefix2 = prefixes[0][index12] + prefixes[1][index22] + prefixes[2][index32] + prefixes2[divisor_index - 1];

        // Combine prefixes
        

        // Debugging: Log final prefixes
        Debug.Log($"Prefix1: {prefix1}, Prefix2: {prefix2}, Combined Result: {ret}");
    }
    else
    {
        // Debugging: Log if divisor2 is zero
        Debug.Log("exponent is now 0, Skipping prefix2 calculation.");
    }
    if (exp >= 3000003) {
        ret = prefix1 + prefix2;
    } else {
        ret = mantissa + prefix1 + prefix2;
    }

}
else {
    int index1 = (int)((exp / 3) % 10);
    int index2 = (int)((exp / 30) % 10);
    int index3 = (int)((exp / 300) % 10);
ret += mantissa.ToString() + prefixes[0][index1] + prefixes[1][index2] + prefixes[2][index3];
}

    // Remove trailing hyphen if it exists
    if (ret.EndsWith("-")) ret = ret.Substring(0, ret.Length - 1);

    if (ret == "U") ret = "M";
    if (ret == "D") ret = "B";

    // Handle specific replacements
    return ret.Replace("UM", "M")
              .Replace("UNA", "NA")
              .Replace("UPC", "PC")
              .Replace("UFM", "FM")
              .Replace("UAT", "AT");
}

}

