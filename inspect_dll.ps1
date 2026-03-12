$dllPath = "c:\Users\AlexYang_VW\source\repos\MMProject\MMProjectMulti\WindowsFormsApp1\bin\x64\Debug\AxMMMark_x64_1.dll"
$asm = [System.Reflection.Assembly]::LoadFrom($dllPath)
foreach ($t in $asm.GetTypes()) {
    foreach ($m in $t.GetMethods()) {
        $params = ($m.GetParameters() | ForEach-Object { "$($_.ParameterType.Name) $($_.Name)" }) -join ', '
        Write-Host "$($t.Name).$($m.Name)($params)"
    }
}
