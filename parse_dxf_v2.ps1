Write-Host "Reading DXF file..."
$dxfFiles = Get-ChildItem "c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\File\*.dxf"

if ($dxfFiles.Count -eq 0) {
    Write-Error "No DXF file found!"
    exit
}

# 優先找出 "上翼板-2.dxf"
$targetFile = $null
foreach ($file in $dxfFiles) {
    if ($file.Name -like "*上翼板-2.dxf") {
        $targetFile = $file
        break
    }
}

if ($targetFile -eq $null) {
    $targetFile = $dxfFiles[0]
}

Write-Host "Using file: $($targetFile.FullName)"

$content = Get-Content $targetFile.FullName

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
        if ($null -ne $x1 -and $null -ne $y1 -and $null -ne $x2) {
            $points += "$x1,$y1,$x2,$y2"
            $x1 = $null
            $y1 = $null
            $x2 = $null
            $y2 = $null
        }
    }
}

$lineStr = $points -join ';'
Set-Content "c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\dxf_lines.txt" -Value $lineStr
Write-Host "Done. Lines saved to dxf_lines.txt"
Write-Host "Extracted $($points.Count) lines."
