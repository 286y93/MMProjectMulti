
$dllPath = "C:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\bin\Debug\MMMarkx641Lib.dll"
$axDllPath = "C:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\bin\Debug\AxMMMark_x64_1.dll"

Write-Host "Inspecting: $dllPath"
try {
    $assembly = [System.Reflection.Assembly]::LoadFrom($dllPath)
    $types = $assembly.GetTypes()
    foreach ($type in $types) {
        Write-Host "Type: $($type.Name)"
        $methods = $type.GetMethods()
        foreach ($method in $methods) {
            if ($method.Name -match "Pen|Layer|Power|Speed|Freq|Pulse|Param|Set|Get") {
                Write-Host "  Method: $($method.Name)"
            }
        }
    }
}
catch {
    Write-Host "Error loading DLL: $_"
}

Write-Host "`nInspecting: $axDllPath"
try {
    $assembly = [System.Reflection.Assembly]::LoadFrom($axDllPath)
    $types = $assembly.GetTypes()
    foreach ($type in $types) {
        Write-Host "Type: $($type.Name)"
        $methods = $type.GetMethods()
        foreach ($method in $methods) {
            if ($method.Name -match "Pen|Layer|Power|Speed|Freq|Pulse|Param|Set|Get") {
                Write-Host "  Method: $($method.Name)"
            }
        }
    }
}
catch {
    Write-Host "Error loading AxDLL: $_"
}
