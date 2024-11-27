# 定义编辑器的路径
$editorPath = "C:\Program Files\MyEditor\MyEditor.exe"

# 检查编辑器路径是否存在
if (-Not (Test-Path $editorPath)) {
    Write-Host "编辑器路径无效：$editorPath" -ForegroundColor Red
    exit
}

# 注册表路径
$regPath = "HKCR\*\shell\Open with MyEditor"
$commandPath = "$regPath\command"

# 创建上下文菜单键
New-Item -Path $regPath -Force | Out-Null
Set-ItemProperty -Path $regPath -Name "(Default)" -Value "Open with MyEditor"

# 添加图标（可选）
Set-ItemProperty -Path $regPath -Name "Icon" -Value $editorPath

# 设置命令
New-Item -Path $commandPath -Force | Out-Null
Set-ItemProperty -Path $commandPath -Name "(Default)" -Value "`"$editorPath`" `"%1`""

Write-Host "上下文菜单已成功添加。" -ForegroundColor Green
