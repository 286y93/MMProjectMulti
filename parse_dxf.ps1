Write-Host "Reading DXF file..."
$dxfPath = "c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\File\上翼板-2.dxf"
$content = Get-Content $dxfPath

$points = @()
$x1 = $null
$y1 = $null
$x2 = $null
$y2 = $null

for ($i = 0; $i -lt $content.Count - 1; $i++) {
    $code = $content[$i].Trim()
    $val = $content[$i + 1].Trim()

    if ($code -eq '10') { $x1 = $val }
    elseif ($code -eq '20') { $y1 = $val }
    elseif ($code -eq '11') { $x2 = $val }
    elseif ($code -eq '21') { 
        $y2 = $val
        if ($x1 -ne $null -and $y1 -ne $null -and $x2 -ne $null -and $y2 -ne $null) {
            $points += "$x1,$y1,$x2,$y2"
            $x1 = $null
            $y1 = $null
            $x2 = $null
            $y2 = $null
        }
    }
}

$lineStr = $points -join ';'
"$lineStr" | Out-File -Encoding UTF8 "c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\dxf_lines.txt"
Write-Host "Done. Lines saved to dxf_lines.txt"
